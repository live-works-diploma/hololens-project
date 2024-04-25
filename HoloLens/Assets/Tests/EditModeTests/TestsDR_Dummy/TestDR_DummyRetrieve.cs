using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class TestDR_DummyRetrieve
{
    [Test]
    public void NullArguments()
    {
        Func<Dictionary<string, object>, Type, IDataHandler> buildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildTask(data);
        };

        Func<IDataHandler, Dictionary<string, object>> turnIntoDictionary = instance =>
        {
            return instance.TurnDataIntoDictionary();
        };

        Func<Type, string, IDataHandler> buildRandomInstance = (type, name) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildRandomInstance(name);
        };

        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

        try
        {
            dummy.Retrieve(null, null).Wait(TimeSpan.FromSeconds(1));
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (AggregateException ex) when (ex.InnerException is ArgumentNullException)
        {
            Assert.Pass();
        }
        catch (AggregateException ex) when (ex.InnerException is TimeoutException)
        {
            Assert.Fail("Operation timed out."); // The Wait operation timed out
        }
    }

    [Test]
    public void NullExpectedTypes()
    {
        Func<Dictionary<string, object>, Type, IDataHandler> buildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildTask(data);
        };

        Func<IDataHandler, Dictionary<string, object>> turnIntoDictionary = instance =>
        {
            return instance.TurnDataIntoDictionary();
        };

        Func<Type, string, IDataHandler> buildRandomInstance = (type, name) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildRandomInstance(name);
        };

        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

        try
        {
            dummy.Retrieve(data => { }, null).Wait(TimeSpan.FromSeconds(1));
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (AggregateException ex) when (ex.InnerException is ArgumentNullException)
        {
            Assert.Pass();
        }
        catch (AggregateException ex) when (ex.InnerException is TimeoutException)
        {
            Assert.Fail("Operation timed out."); // The Wait operation timed out
        }
    }

    [Test]
    public void NullDelegate()
    {
        Func<Dictionary<string, object>, Type, IDataHandler> buildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildTask(data);
        };

        Func<IDataHandler, Dictionary<string, object>> turnIntoDictionary = instance =>
        {
            return instance.TurnDataIntoDictionary();
        };

        Func<Type, string, IDataHandler> buildRandomInstance = (type, name) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildRandomInstance(name);
        };

        var expectedTypes = new Dictionary<string, Type>();

        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

        try
        {
            dummy.Retrieve(null, expectedTypes).Wait(TimeSpan.FromSeconds(1));
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (AggregateException ex) when (ex.InnerException is ArgumentNullException)
        {
            Assert.Pass();
        }
        catch (AggregateException ex) when (ex.InnerException is TimeoutException)
        {
            Assert.Fail("Operation timed out."); // The Wait operation timed out
        }
    }

    [Test]
    public void ValidArguments()
    {
        Func<Dictionary<string, object>, Type, IDataHandler> buildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildTask(data);
        };

        Func<IDataHandler, Dictionary<string, object>> turnIntoDictionary = instance =>
        {
            return instance.TurnDataIntoDictionary();
        };

        Func<Type, string, IDataHandler> buildRandomInstance = (type, name) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildRandomInstance(name);
        };

        var expectedTypes = new Dictionary<string, Type>();

        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

        Assert.DoesNotThrow(() =>
        {
            dummy.Retrieve(data => { }, expectedTypes).Wait();
        });
    }

    [Test]
    public void InvalidExpectedTypes()
    {
        Func<Dictionary<string, object>, Type, IDataHandler> buildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildTask(data);
        };

        Func<IDataHandler, Dictionary<string, object>> turnIntoDictionary = instance =>
        {
            return instance.TurnDataIntoDictionary();
        };

        Func<Type, string, IDataHandler> buildRandomInstance = (type, name) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildRandomInstance(name);
        };

        var expectedTypes = new Dictionary<string, Type>
        {
            { "int", typeof(int) },
            { "string", typeof(string) },
            { "float", typeof(float) },
        };

        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

        try
        {
            dummy.Retrieve(null, expectedTypes).Wait(TimeSpan.FromSeconds(1));
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (AggregateException ex) when (ex.InnerException is ArgumentNullException)
        {
            Assert.Pass();
        }
        catch (AggregateException ex) when (ex.InnerException is TimeoutException)
        {
            Assert.Fail("Operation timed out."); // The Wait operation timed out
        }
    }

    [Test]
    public void ValidExpectedTypes()
    {
        Func<Dictionary<string, object>, Type, IDataHandler> buildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildTask(data);
        };

        Func<IDataHandler, Dictionary<string, object>> turnIntoDictionary = instance =>
        {
            return instance.TurnDataIntoDictionary();
        };

        Func<Type, string, IDataHandler> buildRandomInstance = (type, name) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildRandomInstance(name);
        };

        var expectedTypes = new Dictionary<string, Type>()
        {
            { "Sensr", typeof(Sensor) },
            { "Plant", typeof(Plant) },
            { "TelemetryData", typeof(TelemetryData) },
        };

        var dummy = new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);

        try
        {
            dummy.Retrieve(null, expectedTypes).Wait(TimeSpan.FromSeconds(1));
            Assert.Fail("Expected ArgumentNullException was not thrown.");
        }
        catch (AggregateException ex) when (ex.InnerException is ArgumentNullException)
        {
            Assert.Pass();
        }
        catch (AggregateException ex) when (ex.InnerException is TimeoutException)
        {
            Assert.Fail("Operation timed out."); // The Wait operation timed out
        }
    }
}
