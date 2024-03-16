using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : CreationData
{
    public float waterLevel = 0;
    public float windLevel = 0;
    public float humidity = 0;

    internal override void FillDataArgs(Dictionary<string, string> data)
    {
        waterLevel = data.ContainsKey("water level") ? float.Parse(data["water level"]) : 0;
        windLevel = data.ContainsKey("wind level") ? float.Parse(data["wind level"]) : 0;
        humidity = data.ContainsKey("humidity") ? float.Parse(data["humidity"]) : 0;
    }

    internal override Dictionary<string, string> FillDefaultDataArgs(Dictionary<string, string> dataAlreadyCreated)
    {
        dataAlreadyCreated["wind level"] = Random.Range(0, 20).ToString();
        dataAlreadyCreated["water level"] = Random.Range(0, 20).ToString();
        dataAlreadyCreated["humidity"] = Random.Range(0, 5).ToString();

        return dataAlreadyCreated;
    }
}
