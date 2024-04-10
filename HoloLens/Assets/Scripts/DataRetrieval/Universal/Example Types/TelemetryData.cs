using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TelemetryData : IDataHandler
{
    string _name = "Not set";
    public string name
    {
        get
        {
            return _name;
        }
        set
        {
            _name = value;
        }
    }

    Dictionary<string, string> _data = new();

    public Dictionary<string, string> CreateDefaultData(float heightAlter, string name, float maxDistanceToSpawn = 30)
    {
        Dictionary<string, string> data = new();

        data["TelemetryDataId"] = name;
        data["water Level"] = UnityEngine.Random.Range(0, 10).ToString();
        data["ph levels"] = UnityEngine.Random.Range(0, 5).ToString();
        data["temp."] = UnityEngine.Random.Range(-40, 40).ToString();

        return data;
    }

    public void FillData(Dictionary<string, string> dataNeeded)
    {
        name = dataNeeded["TelemetryDataId"];             
        _data = dataNeeded;
    }

    public Dictionary<string, string> TurnDataIntoDictionary()
    {
        return _data;
    }
}
