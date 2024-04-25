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

    Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>()
    {
        { "Sensor", typeof(Sensor) },
        { "Plant", typeof(Plant) },
        { "TelemetryData", typeof(TelemetryData) },
    };

    [UnityTest]
    public IEnumerator NullArguments()
    {
        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

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
        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

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
        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

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
        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

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

        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

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
        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance)
        {
            amountOfInstancesToCreatePerType = 2,

            logger = message =>
            {
                // Debug.Log(message);
            },
        };

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
        int numberOfInstancesPerType = 1;      

        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance)
        {
            amountOfInstancesToCreatePerType = numberOfInstancesPerType,

            logger = message =>
            {
                // Debug.Log(message);
            },
        };
        
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
