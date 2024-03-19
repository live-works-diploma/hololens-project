using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : CreationData
{
    [Header("Plant Data")]
    public float scale = 1;
    public bool fruiting = false;

    internal override void FillDataArgs(Dictionary<string, string> data)
    {
        scale = data.ContainsKey("scale") ? float.Parse(data["scale"]) : 1;
        fruiting = data.ContainsKey("fruiting") ? bool.Parse(data["fruiting"]) : false;
    }

    internal override Dictionary<string, string> CreateDefaultDataArgs(Dictionary<string, string> dataAlreadyCreated)
    {
        dataAlreadyCreated["scale"] = Random.Range(1f, 2f).ToString();
        dataAlreadyCreated["fruiting"] = (Random.value > 0.5f).ToString();

        return dataAlreadyCreated;
    }

    internal override Dictionary<string, string> TurnDataIntoDictionaryArgs(Dictionary<string, string> classContents)
    {
        classContents["scale"] = scale.ToString();
        classContents["fruiting"] = fruiting.ToString();

        return classContents;
    }
}
