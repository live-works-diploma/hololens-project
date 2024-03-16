using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a way to allow a class to listen to Data_Base scripts
/// </summary>
public interface ICreation
{
    /// <summary>
    /// takes in a list of the data needed to create a class. Each dictionary is the data for a different instance
    /// </summary>
    /// <param name="allData"></param>
    /// <returns></returns>
    IEnumerator CreateDataRoutine(List<Dictionary<string, string>> allData);
}
