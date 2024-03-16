using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CreationData : IFillData
{
    public float locationX = 0;
    public float locationY = 0;
    public float locationZ = 0;

    public GameObject prefab;

    public void FillData(Dictionary<string, string> data)
    {
        locationX = data.ContainsKey("locationX") ? float.Parse(data["locationX"]) : 0;
        locationY = data.ContainsKey("locationY") ? float.Parse(data["locationY"]) : 1;
        locationZ = data.ContainsKey("locationZ") ? float.Parse(data["locationZ"]) : 0;

        FillDataArgs(data);
    }

    internal virtual void FillDataArgs(Dictionary<string, string> data) { }

    public Dictionary<string, string> FillDefaultData(float maxDistanceToSpawn = 30)
    {
        Dictionary<string, string> defaultData = new();

        defaultData["locationX"] = Random.Range(-(maxDistanceToSpawn), maxDistanceToSpawn).ToString();
        defaultData["locationY"] = "0";
        defaultData["locationZ"] = Random.Range(-(maxDistanceToSpawn), maxDistanceToSpawn).ToString();

        return FillDefaultDataArgs(defaultData);
    }

    internal virtual Dictionary<string, string> FillDefaultDataArgs(Dictionary<string, string> defaultData) 
    {
        return defaultData;
    }

    
}
