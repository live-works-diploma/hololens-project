using log4net.Filter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SensorHistoryInstance : MonoBehaviour
{
    [SerializeField] DataTypes.DatabaseTypes fieldToRead;
    public string FieldToRead
    {
        get => fieldToRead.ToString();
    }

    [SerializeField] DataTypes.DatabaseTypes key;
    public string Key
    {
        get => key.ToString();
    }

    [SerializeField] int maxInstancesToShow = 10;
    public int MaxInstancesToShow
    {
        get => maxInstancesToShow;
    }

    [Header("Prefabs")]
    [SerializeField] GameObject spawnLocation;
    [SerializeField] GameObject dataPrefab;

    [Header("Spawning Control")]
    [SerializeField] int distanceBetweenEachData = 15;

    public void ShowData(List<IDataHandler> data)
    {       
        if (dataPrefab == null)
        {
            Debug.LogError("Need to assign data prefab");
            // return;
        }
        
        List<IDataHandler> dataToShow = data.Take(MaxInstancesToShow).ToList();

        for (int i = 0; i < dataToShow.Count; i++)
        {
            Dictionary<string, object> instanceData = dataToShow[i].TurnDataIntoDictionary();

            string prefix = instanceData[Key].ToString();
            string suffix = instanceData[FieldToRead].ToString();

            SpawnObject(prefix, suffix);
        }
    }

    void SpawnObject(string prefix, string data)
    {
        Debug.Log($"prefix: {prefix}, data: {data}");
    }
}
