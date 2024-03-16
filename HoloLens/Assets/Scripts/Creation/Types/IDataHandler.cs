using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// a way to allow Data_Dummy to create default data, fill data without knowing the fields and transfering data from one class to another (class passes
/// the CreateDefaultData return straight into FillData)
/// </summary>
public interface IDataHandler
{
    /// <summary>
    /// takes in a list of data needed to be added to the class and allows the class to manually chose where the data goes
    /// </summary>
    /// <param name="dataNeeded"></param>
    public void FillData(Dictionary<string, string> dataNeeded);

    /// <summary>
    /// genereates random data for you to add to an instance of this object
    /// </summary>
    /// <param name="maxDistanceToSpawn"></param>
    /// <returns></returns>
    public Dictionary<string, string> CreateDefaultData(float maxDistanceToSpawn = 30);

    public Dictionary<string, string> TurnDataIntoDictionary();
}
