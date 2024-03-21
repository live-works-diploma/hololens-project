using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DR_Dummy<T> : IDataRetrieval<T>, IJsonHandler<T> where T : class
{
    [HideInInspector] public Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>();
    [SerializeField] int amountOfInstancesToCreatePerType = 5;

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Func<Dictionary<string, string>, Type, T> howToBuildTask,Func<T, Dictionary<string, string>> howToTurnIntoDictionary = null,  Func<Type, T> buildDefaultData = null)
    {
        callWhenFoundData?.Invoke(await Search(howToBuildTask, buildDefaultData, howToTurnIntoDictionary));
    }

    async Task<Dictionary<string, List<T>>> Search( Func<Dictionary<string, string>, Type, T> howToBuildTask, Func<Type, T> howToCreateDefaultData = null, Func<T, Dictionary<string, string>> howToTurnIntoDictionary = null)
    {
        await Task.Delay(100);  // just to see what happens

        string foundDataJson = RetrieveJson(howToCreateDefaultData, howToTurnIntoDictionary);

        return BuildData(foundDataJson, howToBuildTask);
    }

    string RetrieveJson(Func<Type, T> howToBuildTask, Func<T, Dictionary<string, string>> howToTurnIntoDictionary)
    {
        Dictionary<string, List<Dictionary<string, string>>> allInstances = new Dictionary<string, List<Dictionary<string, string>>>();

        foreach (var type in expectedTypes.Keys)
        {
            List<Dictionary<string, string>> typeInstances = new List<Dictionary<string, string>>();

            for (int i = 0; i < amountOfInstancesToCreatePerType; i++)
            {
                T instance = howToBuildTask(expectedTypes[type]);

                if (instance == null)
                {
                    throw new Exception("instance == null");
                }

                typeInstances.Add(howToTurnIntoDictionary(instance));
            }

            allInstances[expectedTypes[type].Name] = typeInstances;
        }

        return JsonConvert.SerializeObject(allInstances);
    }

    Dictionary<string, List<T>> BuildData(string json, Func<Dictionary<string, string>, Type, T> howToBuildTask)
    {
        Dictionary<string, List<Dictionary<string, string>>> foundData = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(json);

        Dictionary<string, List<T>> builtData = new Dictionary<string, List<T>>();

        foreach (var key in foundData.Keys)
        {
            List<T> instancesFound = new List<T>();

            for (int i = 0; i < foundData[key].Count; i++)
            {
                T instance = howToBuildTask(foundData[key][i], expectedTypes[key]);
                instancesFound.Add(instance);
            }

            builtData[key] = instancesFound;
        }

        return builtData;
    }    
}
