using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestDR_DummyRetrieve
{  
    DR_Dummy<IDataHandler> dummy;
    int numberOfInstancesPerType = 1;

    Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>()
    {
        { "Sensor", typeof(Sensor) },
        { "Plant", typeof(Plant) },
        { "TelemetryData", typeof(TelemetryData) },
    };

    [SetUp]
    public void SetUp()
    { 
        Func<Dictionary<string, object>, Type, IDataHandler> buildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance;
        };

        Func<IDataHandler, Dictionary<string, object>> turnIntoDictionary = instance =>
        {
            return new();
        };

        Func<Type, string, IDataHandler> buildRandomInstance = (type, name) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildRandomInstance(name);
        };       

        dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance)
        {
            amountOfInstancesToCreatePerType = numberOfInstancesPerType,

            logger = message => Debug.Log(message),
        };
    }

    [UnityTest]
    public IEnumerator NullArguments()
    {
        Task retrieveTask = dummy.Retrieve(null, null);

        yield return new WaitUntil(() => retrieveTask.IsCompleted || retrieveTask.IsFaulted || retrieveTask.IsCanceled);

        try
        {
            retrieveTask.GetAwaiter().GetResult();
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (ArgumentNullException ex)
        {
            Assert.Pass("Throw argument null exception");
        }
        catch (TimeoutException)
        {
            Assert.Fail("Operation timed out."); // The Wait operation timed out
        }
    }

    [UnityTest]
    public IEnumerator NullExpectedTypes()
    {
        Task retrieveTask = dummy.Retrieve(data => { }, null);

        yield return new WaitUntil(() => retrieveTask.IsCompleted || retrieveTask.IsFaulted || retrieveTask.IsCanceled);

        try
        {
            retrieveTask.GetAwaiter().GetResult();
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (ArgumentNullException)
        {
            Assert.Pass("Throw argument null exception");
        }
    }


    [UnityTest]
    public IEnumerator NullDelegate()
    {
        Task retrieveTask = dummy.Retrieve(null, expectedTypes);

        yield return new WaitUntil(() => retrieveTask.IsCompleted || retrieveTask.IsFaulted || retrieveTask.IsCanceled);

        try
        {
            retrieveTask.GetAwaiter().GetResult();
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (ArgumentNullException)
        {
            Assert.Pass("Throw argument null exception");
        }
        catch (TimeoutException)
        {
            Assert.Fail("Operation timed out."); // The Wait operation timed out
        }
    }


    [UnityTest]
    public IEnumerator ValidArguments()
    {
        Task retrieveTask = dummy.Retrieve(data => Debug.Log("Data found"), expectedTypes);

        yield return new WaitUntil(() => retrieveTask.IsCompleted || retrieveTask.IsFaulted || retrieveTask.IsCanceled);

        if (retrieveTask.IsFaulted || retrieveTask.IsCanceled)
        {
            Assert.Fail("Operation failed or was canceled.");
        }

        Assert.Pass("Data retrieved successfully.");
    }

    [UnityTest]
    public IEnumerator InvalidExpectedTypes()
    {
        var expectedTypes = new Dictionary<string, Type>
        {
            { "int", typeof(int) },
            { "string", typeof(string) },
            { "float", typeof(float) },
        };

        Task retrieveTask = dummy.Retrieve(data => { }, expectedTypes);

        yield return new WaitUntil(() => retrieveTask.IsCompleted || retrieveTask.IsFaulted || retrieveTask.IsCanceled);

        try
        {
            // Check if the task threw an exception
            retrieveTask.GetAwaiter().GetResult();
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
            Assert.Pass("Throw argument null exception");
        }
        catch (TimeoutException)
        {
            Assert.Fail("Operation timed out."); // The Wait operation timed out
        }
    }

    [UnityTest]
    public IEnumerator ValidExpectedTypes()
    {
        Task retrieveTask = dummy.Retrieve(data => Debug.Log($"different types of data found: {data.Count}"), expectedTypes);

        yield return new WaitUntil(() => retrieveTask.IsCompleted || retrieveTask.IsFaulted || retrieveTask.IsCanceled);

        if (retrieveTask.IsFaulted || retrieveTask.IsCanceled)
        {
            Assert.Fail("Operation failed or was canceled.");
        }

        Assert.Pass("Data retrieved successfully.");
    }

    [UnityTest]
    public IEnumerator CorrectAmountOfDataReturned()
    {
        int totalInstancesFound = 0;

        IDataRetrieval<IDataHandler>.VoidDelegate dele = null;

        dele += data => Debug.Log($"different types of data found: {data.Count}");

        dele += data =>
        {
            foreach (var kvp in data)
            {
                totalInstancesFound += kvp.Value.Count;
            }
        };

        Task retrieveTask = dummy.Retrieve(dele, expectedTypes);

        yield return new WaitUntil(() => retrieveTask.IsCompleted || retrieveTask.IsFaulted || retrieveTask.IsCanceled);

        if (retrieveTask.IsFaulted || retrieveTask.IsCanceled)
        {
            Assert.Fail("Operation failed or was canceled.");
        }

        Assert.AreEqual(expectedTypes.Count * numberOfInstancesPerType, totalInstancesFound);
    }
}
