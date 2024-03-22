using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractor<T> where T : class
{
    void AddListener<type>(IDRHandler<IDataHandler>.VoidDelegate methodToCall) where type : T;
}
