using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : CreationData
{
    public float scale = 1;
    public bool fruiting = false;

    internal override void FillDataArgs(Dictionary<string, string> data)
    {
        scale = data.ContainsKey("scale") ? float.Parse(data["scale"]) : 1;
        fruiting = data.ContainsKey("fruiting") ? bool.Parse(data["fruiting"]) : false;
    }

    internal override Dictionary<string, string> FillDefaultDataArgs(Dictionary<string, string> dataAlreadyCreated)
    {
        dataAlreadyCreated["scale"] = Random.Range(1, 10).ToString();
        dataAlreadyCreated["fruiting"] = (Random.value > 0.5f).ToString();

        return dataAlreadyCreated;
    }
}
