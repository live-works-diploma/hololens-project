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
        List<Dictionary<string, string>> allPlants = new();

        for (int i = 0; i < numberOFEstimatedPlants; i++)
        {
            Dictionary<string, string> plant = new();

            plant["locationX"] = UnityEngine.Random.Range(-(maxDistanceToSpawn), maxDistanceToSpawn).ToString();
            plant["locationY"] = "0";
            plant["locationZ"] = UnityEngine.Random.Range(-(maxDistanceToSpawn), maxDistanceToSpawn).ToString();

            plant["scale"] = UnityEngine.Random.Range(1, 3).ToString();
            plant["fruiting"] = (UnityEngine.Random.value > 0.5).ToString();

            allPlants.Add(plant);
        }

        allData["plant"] = allPlants;

        List<Dictionary<string, string>> allSesnors = new();

        for (int i = 0; i < numberOfEstimatedSensors; i++)
        {
            Dictionary<string, string> sensor = new();

            sensor["locationX"] = UnityEngine.Random.Range(-(maxDistanceToSpawn), maxDistanceToSpawn).ToString();
            sensor["locationY"] = "-3";
            sensor["locationZ"] = UnityEngine.Random.Range(-(maxDistanceToSpawn), maxDistanceToSpawn).ToString();

            sensor["wind level"] = UnityEngine.Random.Range(0, 20).ToString();
            sensor["water level"] = UnityEngine.Random.Range(0, 20).ToString();
            sensor["humidity"] = UnityEngine.Random.Range(0, 5).ToString();

            allSesnors.Add(sensor);
        }

        allData["sensor"] = allSesnors;

        foundData?.Invoke(allData);
    }
}
