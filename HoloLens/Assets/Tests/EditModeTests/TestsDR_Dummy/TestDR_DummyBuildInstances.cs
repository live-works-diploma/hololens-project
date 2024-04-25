using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.TestTools;

public class TestDR_DummyBuildInstances
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
            { "name", instance.GetType().Name },
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

        Task task = dummy.BuildInstances(null);

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

        Task task = dummy.BuildInstances(expectedTypes);

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
        catch (TimeoutException)
        {
            Assert.Fail("Operation timed out.");
        }
    }

    [UnityTest]
    public IEnumerator ValidInstances()
    {
        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance)
        {
            amountOfInstancesToCreatePerType = 1,

            logger = message =>
            {
                Debug.Log(message);
            },
        };

        var task = dummy.BuildInstances(expectedTypes);

        yield return new WaitUntil(() => task.IsCompleted || task.IsFaulted || task.IsCanceled);

        try
        {
            // Check if the task threw an exception
            task.GetAwaiter().GetResult();
        }
        catch (ArgumentNullException)
        {
            Assert.Fail("Throw argument null exception");
        }
        catch (TimeoutException)
        {
            Assert.Fail("Operation timed out.");
        }

        var data = task.Result;

        bool error = false;

        foreach (var type in data.Keys)
        {
            for (int i = 0; i < data[type].Count; i++)
            {
                var instance = data[type][i];
                if (instance == null || instance["name"]?.ToString() != type)
                {
                    Assert.Fail($"Instance is null or name does not match type. Type: {type}, Index: {i}");
                }
            }
        }

        Assert.Pass("Instances were correct");
    }
}
