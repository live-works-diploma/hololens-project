using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJsonHandler<T> where T : class
{
    Dictionary<string, List<T>> BuildData(Dictionary<string, 
        List<Dictionary<string, string>>> foundData, 
        Func<Dictionary<string, string>, Type, T> howToBuildTask) 
    {
        throw new NotImplementedException();
    }

    string RetrieveJson(Func<Type, T> howToBuildTask, Func<T, Dictionary<string, string>> howToTurnIntoDictionary)
    {
        throw new NotImplementedException();
    }        
}
