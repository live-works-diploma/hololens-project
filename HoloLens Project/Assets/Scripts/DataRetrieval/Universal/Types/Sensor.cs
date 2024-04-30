using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : IDataHandler
{
    string _name = "not set";
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

    Dictionary<string, object> sensorData = new();

    public void FillData(Dictionary<string, object> data)
    {
        name = data["Name"].ToString();
        sensorData = data;
    }

    public Dictionary<string, object> CreateDefaultData(float heightAlter, string name, float maxDistanceToSpawn = 30)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["Name"] = name;
        data["wind level"] = Random.Range(0f, 20f);
        data["water level"] = Random.Range(0f, 10f);
        data["humidity"] = Random.Range(0f, 5f);

        return data;
    }

    public Dictionary<string, object> TurnDataIntoDictionary()
    {
        return sensorData;
    }
}
