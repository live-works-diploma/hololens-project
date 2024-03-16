using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Dummy : Data_Base
{
    [SerializeField] int numberOfEstimatedSensors = 1;
    [SerializeField] int numberOFEstimatedPlants = 5;
    [SerializeField] float maxDistanceToSpawn = 20;

    internal override void GetData(DataRetrival foundData)
    {
        Dictionary<string, List<Dictionary<string, string>>> allData = new();

        allData["Plant"] = GenerateData<Plant>(numberOFEstimatedPlants);
        allData["Sensor"] = GenerateData<Sensor>(numberOfEstimatedSensors);

        foundData?.Invoke(allData);
    }

    List<Dictionary<string, string>> GenerateData<T>(float number) where T : IDataHandler, new()
    {
        List<Dictionary<string, string>> listOfAllData = new();

        T instance = new();

        for (int i = 0; i < number; i ++)
        {
            listOfAllData.Add(instance.CreateDefaultData());
        }

        return listOfAllData;
    }
}
