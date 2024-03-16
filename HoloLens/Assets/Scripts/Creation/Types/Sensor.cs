using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : CreationData
{
    [Header("Sensor Data")]
    public float waterLevel = 0;
    public float windLevel = 0;
    public float humidity = 0;

    internal override void FillDataArgs(Dictionary<string, string> data)
    {
        waterLevel = data.ContainsKey("water level") ? float.Parse(data["water level"]) : 0;
        windLevel = data.ContainsKey("wind level") ? float.Parse(data["wind level"]) : 0;
        humidity = data.ContainsKey("humidity") ? float.Parse(data["humidity"]) : 0;
    }

    internal override Dictionary<string, string> CreateDefaultDataArgs(Dictionary<string, string> dataAlreadyCreated)
    {
        dataAlreadyCreated["wind level"] = Random.Range(0, 20).ToString();
        dataAlreadyCreated["water level"] = Random.Range(0, 20).ToString();
        dataAlreadyCreated["humidity"] = Random.Range(0, 5).ToString();

        return dataAlreadyCreated;
    }

    internal override Dictionary<string, string> TurnDataIntoDictionaryArgs(Dictionary<string, string> classContents)
    {
        classContents["water level"] = waterLevel.ToString();
        classContents["wind level"] = windLevel.ToString();
        classContents["humidity"] = humidity.ToString();

        return classContents;
    }
}
