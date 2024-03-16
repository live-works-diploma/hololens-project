using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteMe : MonoBehaviour
{
    public delegate void TestDelegate(List<IFillData> data);

    Dictionary<string, TestDelegate> toCall = new();

    void Start()
    {
        toCall["ObjectOne"] = (List<IFillData> data) => CreateObjectOne((List<ObjectTypeOne>)(object)data);
        toCall["ObjectTwo"] = (List<IFillData> data) => CreateObjectTwo((List<ObjectTypeTwo>)(object)data);

        List<IFillData> dataList = new();

        toCall["ObjectOne"]?.Invoke(dataList);
        toCall["ObjectTwo"]?.Invoke(dataList);
    }

    void CreateObjectOne(List<ObjectTypeOne> ObjectOne) { }

    void CreateObjectTwo(List<ObjectTypeTwo> ObjectTwo) { }
}

public class ObjectTypeOne : IFillData
{
    float variableFloat = 10;

    public void FillData(Dictionary<string, string> dataNeeded)
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, string> FillDefaultData(float maxDistanceToSpawn = 30)
    {
        throw new System.NotImplementedException();
    }
}

public class ObjectTypeTwo : IFillData
{
    bool variableBool;

    public void FillData(Dictionary<string, string> dataNeeded)
    {
        throw new System.NotImplementedException();
    }

    public Dictionary<string, string> FillDefaultData(float maxDistanceToSpawn = 30)
    {
        throw new System.NotImplementedException();
    }
}
