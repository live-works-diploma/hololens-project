using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Microsoft.MixedReality.GraphicsTools.MeshInstancer;

/// <summary>
/// Says this class is a type to listen for when retrieving data. This interfaces makes the class that implements it implement a method which takes in data and builds the class
/// based off that data, a method which gathers all the data from the class and returns it (used for displayed data on a canvas) and a method which randomly generates data
/// (only used if wanting to create random default data). This also has the Funcs needed for telling other classes how to interact with the methods (so can call one Func 
/// and each class can chose what that Func does)
/// </summary>
public interface IDataHandler
{
    /// <summary>
    /// takes in a list of data needed to be added to the class and allows the class to manually chose where the data goes
    /// </summary>
    /// <param name="dataNeeded"></param>
    void FillData(Dictionary<string, string> dataNeeded);

    /// <summary>
    /// genereates random data for you to add to an instance of this object
    /// </summary>
    /// <param name="maxDistanceToSpawn"></param>
    /// <returns></returns>
    Dictionary<string, string> CreateDefaultData(float heightAlter, string name, float maxDistanceToSpawn = 30);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Dictionary<string, string> TurnDataIntoDictionary();

    IDataHandler BuildTask(Dictionary<string, string> data)
    {
        FillData(data);
        return this;
    }

    Dictionary<string, string> TurnIntoDictionary()
    {
        return TurnDataIntoDictionary();
    }

    IDataHandler BuildRandomInstance(string name)
    {
        FillData(CreateDefaultData(0, name));
        return this;
    }

    /// <summary>
    /// Used for the canvases
    /// </summary>
    public string name { get; set; }
}
