using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TelemetryData : IDataHandler
{
    string _name = "Table Setup";
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

    Dictionary<string, object> _data = new();

    public Dictionary<string, object> CreateDefaultData(float heightAlter, string name, float maxDistanceToSpawn = 30)
    {
        Dictionary<string, object> data = new();

        data["TelemetryDataId"] = name;
        data["water Level"] = UnityEngine.Random.Range(0, 10);
        data["ph levels"] = UnityEngine.Random.Range(0, 5);
        data["temp."] = UnityEngine.Random.Range(-40, 40);

        return data;
    }

    public void FillData(Dictionary<string, object> dataNeeded)
    {
        // name = dataNeeded.ContainsKey("DeviceSent") ? dataNeeded["DeviceSent"].ToString() : "Table Setup";             
        _data = dataNeeded;
    }

    public Dictionary<string, object> TurnDataIntoDictionary()
    {
        return _data;
    }
}
