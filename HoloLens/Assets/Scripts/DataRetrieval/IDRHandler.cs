using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDRHandler<T> where T : class
{
    delegate void VoidDelegate(List<T> foundData);
    void AddListener<type>(VoidDelegate methodToCallWhenFoundData);
    void RetrieveBuiltData() { }
}
