using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : IDataHandler
{
    [Header("Plant Data")]
    public float scale = 1;
    public bool fruiting = false;

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

    public void FillData(Dictionary<string, string> data)
    {
        name = data.ContainsKey("name") ? data["name"] : "unknown";
        scale = data.ContainsKey("scale") ? float.Parse(data["scale"]) : 1;
        fruiting = data.ContainsKey("fruiting") ? bool.Parse(data["fruiting"]) : false;
    }

    public Dictionary<string, string> CreateDefaultData(float heightAlter, string name, float maxDistanceToSpawn = 30)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        data["name"] = name;
        data["scale"] = Random.Range(1f, 2f).ToString();
        data["fruiting"] = (Random.value > 0.5f).ToString();

        return data;
    }

    public Dictionary<string, string> TurnDataIntoDictionary()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        data["name"] = name;
        data["scale"] = scale.ToString();
        data["fruiting"] = fruiting.ToString();

        return data;
    }
}
