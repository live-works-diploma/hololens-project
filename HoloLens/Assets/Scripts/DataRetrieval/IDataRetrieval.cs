using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataRetrieval<T> where T : class
{
    delegate void VoidDelegate(Dictionary<string, List<T>> wantedTypes);

    void Retrieve(VoidDelegate callWhenFoundData, 
        Func<Dictionary<string, string>, Type, T> howToBuildTask, 
        Func<T, Dictionary<string, string>> howToTurnIntoDictionary = null, 
        Func<Type, T> buildDefaultData = null);
}
