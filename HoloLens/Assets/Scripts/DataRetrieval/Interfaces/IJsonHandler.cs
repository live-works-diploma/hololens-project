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
    /// <summary>
    /// converts a json string into the needed format for the main class and creates instances based off the data retrieved
    /// </summary>
    /// <param name="json"></param>
    /// <param name="howToBuildTask"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    static Dictionary<string, List<T>> BuildData(string json, Func<Dictionary<string, string>, Type, T> howToBuildTask, Dictionary<string, Type> expectedTypes)
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

    /// <summary>
    /// Gets a json string and converts it into a dictionary.
    /// </summary>
    /// <param name="howToBuildTask"></param>
    /// <param name="howToTurnIntoDictionary"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    abstract Task<string> RetrieveJson(Func<Type, T> howToBuildTask, Func<T, Dictionary<string, string>> howToTurnIntoDictionary);        
}
