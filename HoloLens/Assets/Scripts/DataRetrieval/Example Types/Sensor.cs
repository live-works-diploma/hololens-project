using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : IDataHandler
{
    [Header("Sensor Data")]
    public float waterLevel = 0;
    public float windLevel = 0;
    public float humidity = 0;

    public void FillData(Dictionary<string, string> data)
    {
        waterLevel = data.ContainsKey("water level") ? float.Parse(data["water level"]) : 0;
        windLevel = data.ContainsKey("wind level") ? float.Parse(data["wind level"]) : 0;
        humidity = data.ContainsKey("humidity") ? float.Parse(data["humidity"]) : 0;
    }

    public Dictionary<string, string> CreateDefaultData(float heightAlter, float maxDistanceToSpawn = 30)
    {
        Dictionary<string, string> data = new Dictionary<string, string>(); 

        data["wind level"] = Random.Range(0, 20).ToString();
        data["water level"] = Random.Range(0, 20).ToString();
        data["humidity"] = Random.Range(0, 5).ToString();

        return data;
    }

    public Dictionary<string, string> TurnDataIntoDictionary()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        data["water level"] = waterLevel.ToString();
        data["wind level"] = windLevel.ToString();
        data["humidity"] = humidity.ToString();

        return data;
    }
}
