using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DR_Network<T> : IDataRetrieval<T>, IJsonHandler<T> where T : class
{
    public void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Dictionary<string, Type> expectedTypes, string query)
    {
        throw new NotImplementedException();
    }

    public Task<string> RetrieveJson(string queries)
    {
        throw new NotImplementedException();
    }
}
