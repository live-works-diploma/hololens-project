using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestDR_DummyInstantiate
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

    [SetUp]
    public void SetUp()
    {

    }

    [Test]
    public void NullArguments()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new DR_Dummy<IDataHandler>(null, null, null);
        });
    }

    [Test]
    public void NullArgumentBuildTask()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new DR_Dummy<IDataHandler>(null, turnIntoDictionary, buildRandomInstance);
        });
    }

    [Test]
    public void NullArgumentTurnIntoDictionary()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new DR_Dummy<IDataHandler>(buildTask, null, buildRandomInstance);
        });
    }

    [Test]
    public void NullArgumentBuildRandomInstances()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, null);
        });
    }

    [Test]
    public void ValidArguments()
    {
        Assert.DoesNotThrow(() =>
        {
            new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance);
        });
    }
}
