using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : IDataHandler
{
    [Header("Plant Data")]
    public object scale = 1;
    public object fruiting = false;

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

    public void FillData(Dictionary<string, object> data)
    {
        name = data.ContainsKey("name") ? data["name"].ToString() : "unknown";
        scale = data.ContainsKey("scale") ? float.Parse(data["scale"].ToString()) : 1;
        fruiting = data.ContainsKey("fruiting") ? bool.Parse(data["fruiting"].ToString()) : false;
    }

    public Dictionary<string, object> CreateDefaultData(float heightAlter, string name, float maxDistanceToSpawn = 30)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["name"] = name;
        data["scale"] = Random.Range(1f, 2f).ToString();
        data["fruiting"] = (Random.value > 0.5f).ToString();

        return data;
    }

    public Dictionary<string, object> TurnDataIntoDictionary()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();

        data["name"] = name;
        data["scale"] = scale;
        data["fruiting"] = fruiting;

        return data;
    }
}
