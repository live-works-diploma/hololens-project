using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Microsoft.MixedReality.GraphicsTools.MeshInstancer;

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
    void FillData(Dictionary<string, string> dataNeeded);

    /// <summary>
    /// genereates random data for you to add to an instance of this object
    /// </summary>
    /// <param name="maxDistanceToSpawn"></param>
    /// <returns></returns>
    Dictionary<string, string> CreateDefaultData(float heightAlter, string name, float maxDistanceToSpawn = 30);

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

    static Func<IDataHandler, Dictionary<string, string>> howToTurnIntoDictionary
    {
        get
        {
            return instance => instance.TurnDataIntoDictionary();
        }
    }

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

    public string name { get; set; }
}
