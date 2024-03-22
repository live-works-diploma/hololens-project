using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : IDataHandler
{
    [Header("Plant Data")]
    public float scale = 1;
    public bool fruiting = false;

    public void FillData(Dictionary<string, string> data)
    {
        scale = data.ContainsKey("scale") ? float.Parse(data["scale"]) : 1;
        fruiting = data.ContainsKey("fruiting") ? bool.Parse(data["fruiting"]) : false;
    }

    public Dictionary<string, string> CreateDefaultData(float heightAlter, float maxDistanceToSpawn = 30)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        data["scale"] = Random.Range(1f, 2f).ToString();
        data["fruiting"] = (Random.value > 0.5f).ToString();

        return data;
    }

    public Dictionary<string, string> TurnDataIntoDictionary()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        data["scale"] = scale.ToString();
        data["fruiting"] = fruiting.ToString();

        return data;
    }
}
