using System.Collections;
using System.Collections.Generic;
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
        throw new System.NotImplementedException();
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
