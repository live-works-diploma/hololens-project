using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// An interface for retrieving json strings and then converting it into a dictionary. Allows other classes do what they want with the Dicionary.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IJsonHandler<T> where T : class
{
    static Dictionary<string, List<T>> BuildData(string json, Func<Dictionary<string, string>, Type, T> howToBuildTask, Dictionary<string, Type> expectedTypes)
    {
        Dictionary<string, List<Dictionary<string, string>>> foundData = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(json);

        Dictionary<string, List<T>> builtData = new Dictionary<string, List<T>>();

        foreach (var key in foundData.Keys)
        {
            if (!expectedTypes.ContainsKey(key))
            {
                throw new Exception($"Key found isn't in expected keys: {key}");
            }

            if (!typeof(T).IsAssignableFrom(expectedTypes[key])) 
            {
                throw new Exception($"Expected type doesn't implement needed type of: {typeof(T).Name}");
            }

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

    abstract Task<string> RetrieveJson();        
}
