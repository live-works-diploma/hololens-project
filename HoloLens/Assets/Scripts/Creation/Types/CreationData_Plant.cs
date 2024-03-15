using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationData_Plant : CreationData
{
    public float scale = 1;
    public bool fruiting = false;

    internal override void FillDataArgs(Dictionary<string, string> data)
    {
        scale = data.ContainsKey("scale") ? float.Parse(data["scale"]) : 1;
        fruiting = data.ContainsKey("fruiting") ? bool.Parse(data["fruiting"]) : false;
    }
}
