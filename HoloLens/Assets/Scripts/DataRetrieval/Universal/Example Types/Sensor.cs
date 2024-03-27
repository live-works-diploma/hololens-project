using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : IDataHandler
{
    string _name;
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

    Dictionary<string, string> sensorData = new();

    public void FillData(Dictionary<string, string> data)
    {
        name = data["Name"];
        sensorData = data;
    }

    public Dictionary<string, string> CreateDefaultData(float heightAlter, string name, float maxDistanceToSpawn = 30)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        data["Name"] = name;
        data["wind level"] = Random.Range(0f, 20f).ToString();
        data["water level"] = Random.Range(0f, 10f).ToString();
        data["humidity"] = Random.Range(0f, 5f).ToString();

        return data;
    }

    public Dictionary<string, string> TurnDataIntoDictionary()
    {
        return sensorData;
    }
}
