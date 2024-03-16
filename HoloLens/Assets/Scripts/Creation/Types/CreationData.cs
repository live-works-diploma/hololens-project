using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class CreationData : MonoBehaviour, IDataHandler
{
    [Header("Creation Data")]
    public float locationX = 0;
    public float locationY = 0;
    public float locationZ = 0;

    [HideInInspector] public GameObject prefab;

    public void FillData(Dictionary<string, string> data)
    {
        locationX = data.ContainsKey("locationX") ? float.Parse(data["locationX"]) : 0;
        locationY = data.ContainsKey("locationY") ? float.Parse(data["locationY"]) : 1;
        locationZ = data.ContainsKey("locationZ") ? float.Parse(data["locationZ"]) : 0;

        FillDataArgs(data);
    }

    internal virtual void FillDataArgs(Dictionary<string, string> data) { }

    public Dictionary<string, string> CreateDefaultData(float maxDistanceToSpawn = 30)
    {
        Dictionary<string, string> defaultData = new();

        defaultData["locationX"] = Random.Range(-(maxDistanceToSpawn), maxDistanceToSpawn).ToString();
        defaultData["locationY"] = "0";
        defaultData["locationZ"] = Random.Range(-(maxDistanceToSpawn), maxDistanceToSpawn).ToString();

        return CreateDefaultDataArgs(defaultData);
    }

    internal virtual Dictionary<string, string> CreateDefaultDataArgs(Dictionary<string, string> defaultData) 
    {
        return defaultData;
    }

    public Dictionary<string, string> TurnDataIntoDictionary()
    {
        Dictionary<string, string> classContents = new();

        classContents["locationX"] = locationX.ToString();
        classContents["locationY"] = locationY.ToString();
        classContents["locationZ"] = locationZ.ToString();

        return TurnDataIntoDictionaryArgs(classContents);
    }

    internal virtual Dictionary<string, string> TurnDataIntoDictionaryArgs(Dictionary<string, string> classContents)
    {
        return classContents;
    }
}
