using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Dummy : Data_Base
{
    [SerializeField] int numberOfEstimatedSensors = 1;
    [SerializeField] int numberOFEstimatedPlants = 5;
    [SerializeField] float maxDistanceToSpawn = 20;

    /// <summary>
    /// Creates a dictionary which has a list of instances in dictionary format as the value.
    /// </summary>
    /// <param name="foundData"></param>
    internal override void GetData(DataRetrival foundData)
    {
        Dictionary<string, List<Dictionary<string, string>>> allData = new()
        {
            ["Plant"] = GenerateData<Plant>(numberOFEstimatedPlants),
            ["Sensor"] = GenerateData<Sensor>(numberOfEstimatedSensors)
        };

        foundData?.Invoke(allData);
    }

    /// <summary>
    /// Creates an instance of the wanted class and fills a list with the data returned from that instance in a dictionary format. The instance needs 
    /// an interface called IDataHandler and it calls the method CreateDefaultData to generete the random data.
    /// </summary>
    /// <typeparam name="T">The type of class you wish to generate random data for. Needs to have the IDataHandler interface for it to work.</typeparam>
    /// <param name="number">The amount of randomly generated instances you want (in dictionary format)</param>
    /// <returns>The list of randomly generated instances in dictionary format.</returns>
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
