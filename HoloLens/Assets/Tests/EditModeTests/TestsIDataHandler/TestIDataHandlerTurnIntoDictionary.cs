using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestIDataHandlerTurnIntoDictionary
{
    [Test]
    public void DoesNotThowWhenCalled()
    {
        IDataHandler mock = new MockIDataHandler();

        Assert.DoesNotThrow(() =>
        {
            mock.TurnDataIntoDictionary();
        });
    }
}
