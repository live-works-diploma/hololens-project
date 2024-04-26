using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestDR_DummyRetrieveJson
{
    Func<Dictionary<string, object>, Type, IDataHandler> buildTask = (data, type) =>
    {
        IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
        return instance;
    };

    Func<IDataHandler, Dictionary<string, object>> turnIntoDictionary = instance =>
    {
        Dictionary<string, object> data = new()
        {
            { "type", instance.GetType().Name },
        };

        return data;
    };

    Func<Type, string, IDataHandler> buildRandomInstance = (type, name) =>
    {
        IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
        return instance.BuildRandomInstance(name);
    };

    Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>()
    {
        { "Sensor", typeof(Sensor) },
    };

    [UnityTest] 
    public IEnumerator NullArguments()
    {
        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance)
        {
            amountOfInstancesToCreatePerType = 1,

            logger = message =>
            {
                Debug.Log(message);
            },
        };

        Task task = dummy.RetrieveJson(null);

        yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled);

        try
        {
            // Check if the task threw an exception
            task.GetAwaiter().GetResult();
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentNullException)
        {
            Assert.Pass("Throw argument null exception");
        }
        catch (TimeoutException)
        {
            Assert.Fail("Operation timed out.");
        }
    }

    [UnityTest]
    public IEnumerator ValidArguments()
    {
        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance)
        {
            amountOfInstancesToCreatePerType = 1,

            logger = message =>
            {
                Debug.Log(message);
            },
        };

        Task task = dummy.RetrieveJson(expectedTypes);

        yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled);

        try
        {
            // Check if the task threw an exception
            task.GetAwaiter().GetResult();
            Assert.Pass("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentNullException)
        {
            Assert.Fail("Throw argument null exception");
        }
        catch (ArgumentException)
        {
            Assert.Fail("Throw argument exception");
        }
        catch (TimeoutException)
        {
            Assert.Fail("Operation timed out.");
        }
    }

    [UnityTest]
    public IEnumerator ReturnsExpectedJsonString()
    {
        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance)
        {
            amountOfInstancesToCreatePerType = 1,

            logger = message =>
            {
                Debug.Log(message);
            },
        };

        string retrievedJsonString = "";

        Task task = dummy.RetrieveJson(expectedTypes).ContinueWith(jsonString =>
        {
            retrievedJsonString = jsonString.Result;
        });

        yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled);

        if (task.IsFaulted || task.IsCanceled)
        {
            Assert.Fail("Operation failed or was canceled.");
        }

        var data = new Dictionary<string, List<Dictionary<string, object>>>
        {
            {
                "Sensor", new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "type", "Sensor" },
                    }
                }
            },
        };

        // Serialize the data to JSON
        string expectedJsonString = JsonConvert.SerializeObject(data);

        Debug.Log($"Expected string: {expectedJsonString}");
        Debug.Log($"Found string: {retrievedJsonString}");

        Assert.AreEqual(expectedJsonString, retrievedJsonString, "Json strings were equal");
    }

    [UnityTest]
    public IEnumerator InvalidArgument()
    {
        Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>()
        {
            { "int", typeof(int) },
        };

        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance)
        {
            amountOfInstancesToCreatePerType = 1,

            logger = message =>
            {
                Debug.Log(message);
            },
        };

        Task task = dummy.RetrieveJson(expectedTypes);

        yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled);

        try
        {
            task.GetAwaiter().GetResult();
            Assert.Fail("Expected ArgumentException was not thrown.");
        }
        catch (ArgumentException)
        {
            Assert.Pass("Throw argument null exception");
        }
        catch (TimeoutException)
        {
            Assert.Fail("Operation timed out.");
        }
    }
}
