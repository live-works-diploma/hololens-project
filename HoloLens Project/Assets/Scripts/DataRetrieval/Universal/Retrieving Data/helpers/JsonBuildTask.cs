using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class JsonBuildTask<T> where T : class
{
    public static async Task<Dictionary<string, List<T>>> BuildData(string json, Func<Dictionary<string, object>, Type, T> howToBuildTask, Dictionary<string, Type> expectedTypes, Action<string> logger = null)
    {
        if (json == null)
        {
            throw new Exception("Json was null. Check the names of the class you are searching for match the name of a table in the database. Most likely the reason. A" +
                "Also you don't need [dbo] stuff for the table name it does it for you on the other end.");
        }

        return await Task.Run(() =>
        {
            try
            {
                Dictionary<string, List<Dictionary<string, object>>> foundData = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(json);

                Dictionary<string, List<T>> builtData = new Dictionary<string, List<T>>();

                foreach (var key in foundData.Keys)
                {
                    if (!expectedTypes.ContainsKey(key))
                    {
                        foreach (var kvp in expectedTypes)
                        {
                            logger?.Invoke($"expected types ({kvp.Key}:{kvp.Value})");
                        }

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
            catch (JsonException ex)
            {
                throw new Exception($"Json Error: {ex}");
            }
        });
    }
}
