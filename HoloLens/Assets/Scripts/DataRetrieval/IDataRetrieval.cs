using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataRetrieval<T> where T : class
{
    delegate void VoidDelegate(string json);

    void Retrieve(VoidDelegate callWhenFoundData, Func<T, Dictionary<string, string>> howToTurnIntoDictionary, Func<Dictionary<string, string>, Type, T> howToBuildTask);
    void Retrieve(VoidDelegate callWhenFoundData, Func<T, Dictionary<string, string>> howToTurnIntoDictionary, Func<Type, T> buildDefaultData);
        
         
        

    void Send(string json);
}
