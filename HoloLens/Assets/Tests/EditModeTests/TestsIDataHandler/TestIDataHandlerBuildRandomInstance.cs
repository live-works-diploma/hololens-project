using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestIDataHandlerBuildRandomInstance
{
    IDataHandler mock;

    [SetUp]
    public void SetUp()
    {
        mock = new MockIDataHandler();
    }

    [Test]
    public void NullArgument()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            mock.BuildRandomInstance(null);
        });
    }

    [Test]
    public void ValidArgument()
    {
        Assert.DoesNotThrow(() =>
        {
            mock.BuildRandomInstance("");
        });
    }
}
