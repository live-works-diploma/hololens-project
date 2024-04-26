using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestIDataHandlerBuildTask
{
    Dictionary<string, object> data = new()
    {
        { "type", "MockIDataHandler" }
    };

    IDataHandler mock;

    [SetUp]
    public void SetUp()
    {
        mock = new MockIDataHandler();
    }

    [Test]
    public void NullArgument()
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            mock.BuildTask(null);
        });
        
    }

    [Test]
    public void ValidArgument()
    {
        Assert.DoesNotThrow(() =>
        {
            mock.BuildTask(data);
        });
    }

    [Test]
    public void FillsData()
    {
        MockIDataHandler mockChild = (MockIDataHandler)mock;

        try
        {
            mock.BuildTask(data);
        }
        catch (Exception ex)
        {
            Assert.Fail(ex.ToString());
        }

        Dictionary<string, object> instanceData = mockChild.data;

        if (data.Count != instanceData.Count)
        {
            Assert.Fail("Counts of data and instance data don't match");
        }

        foreach (var kvp in data)
        {
            Assert.IsTrue(instanceData.ContainsKey(kvp.Key));
            Assert.AreEqual(kvp.Value, instanceData[kvp.Key]);
        }
    }
}
