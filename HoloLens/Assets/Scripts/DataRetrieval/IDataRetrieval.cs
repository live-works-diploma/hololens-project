using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataRetrieval<T> where T : class
{
    delegate void VoidDelegate(string json);

    void Retrieve(VoidDelegate callWhenFoundData, Func<Type, T> howToBuildTask);
    void Send(string json);
}
