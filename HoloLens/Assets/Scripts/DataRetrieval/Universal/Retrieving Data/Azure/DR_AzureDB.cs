using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DR_AzureDB<T> : IDataRetrieval<T>, IAzure where T : class
{
    public void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData)
    {
        throw new NotImplementedException();
    }

    Dictionary<string, Type> expectedTypes;
    public void SetExpectedTypes(Dictionary<string, Type> typesToListenFor)
    {
        expectedTypes = typesToListenFor;
    }
}
