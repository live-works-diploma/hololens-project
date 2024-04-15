using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class JsonBuildTask<T> where T : class
{
    public static async Task<Dictionary<string, List<T>>> BuildData(string json, Func<Dictionary<string, string>, Type, T> howToBuildTask, Dictionary<string, Type> expectedTypes)
    {
        if (json == null)
        {
            throw new Exception("Json was null. Check the names of the class you are searching for match the name of a table in the database. Most likely the reason. A" +
                "Also you don't need [dbo] stuff for the table name it does it for you on the other end.");
        }

        return await Task.Run(() =>
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
        });
    }
}
