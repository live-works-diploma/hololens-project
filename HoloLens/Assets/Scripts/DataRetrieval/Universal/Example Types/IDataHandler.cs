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

    static Func<Dictionary<string, string>, Type, IDataHandler> howToBuildTask 
    {
        get
        {
            return (data, type) =>
            {
                IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
                instance.FillData(data);
                
                return instance;
            };
        }
    }

    /// <summary>
    /// Used to turn all the data from the class into dictionary format
    /// </summary>
    static Func<IDataHandler, Dictionary<string, string>> howToTurnIntoDictionary
    {
        get
        {
            return instance => instance.TurnDataIntoDictionary();
        }
    }

    /// <summary>
    /// Used to get data and fill the class based off that data.
    /// </summary>
    static Func<Type, string, IDataHandler> howToBuildDefaultTask
    {
        get
        {
            return (type, name) =>
            {
                IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
                instance.FillData(instance.CreateDefaultData(0, name));

                return instance;
            };
        }
    }

    /// <summary>
    /// Used for the canvases
    /// </summary>
    public string name { get; set; }
}
