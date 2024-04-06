using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

/// <summary>
/// A class meant for creating default data. It creates default data on the expected types, (to add expected types use SetExpectedTypes method). Then it creates a range of 
/// randomly generated instances of each type and turns them into dictionary format, (it uses the Funcs passed in to achieve this). It then takes that dictionary and converts
/// it back into the wanted instances and passes that back through the delegate argument in correct format.
/// it then deserializes that json string
/// </summary>
/// <typeparam name="T">
/// The type of data you wish to create. It is meant to be a parent type and you will limit yourself with using the base type since can only listen for one
/// type.
/// </typeparam>
public class DR_Dummy<T> : IDataRetrieval<T>, IJsonHandler<T> where T : class
{
    Func<Dictionary<string, string>, Type, T> howToBuildTask;
    Func<T, Dictionary<string, string>> howToTurnIntoDictionary;
    Func<Type, string, T> howToBuildDefaultTask;

    public int amountOfInstancesToCreatePerType = 5;

    public DR_Dummy(Func<Dictionary<string, string>, Type, T> howToBuildTask, Func<T, Dictionary<string, string>> howToTurnIntoDictionary, Func<Type, string, T> howToBuildDefaultTask)
    {
        if (howToBuildTask == null || howToBuildDefaultTask == null || howToTurnIntoDictionary == null)
        {
            throw new Exception("Funcs can't be null");
        }

        this.howToBuildDefaultTask = howToBuildDefaultTask;
        this.howToTurnIntoDictionary = howToTurnIntoDictionary;
        this.howToBuildTask = howToBuildTask;
    }

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Dictionary<string, Type> expectedTypes, string query)
    {
        callWhenFoundData?.Invoke(await Search(expectedTypes));
    }

    async Task<Dictionary<string, List<T>>> Search(Dictionary<string, Type> expectedTypes)
    {
        await Task.Delay(100);  // just to see what happens

        string foundDataJson = await RetrieveJson(expectedTypes);

        return JsonBuildTask<T>.BuildData(foundDataJson, howToBuildTask, expectedTypes);
    }

    public async Task<string> RetrieveJson(Dictionary<string, Type> expectedTypes)
    {
        Dictionary<string, List<Dictionary<string, string>>> allInstances = new Dictionary<string, List<Dictionary<string, string>>>();

        foreach (var type in expectedTypes.Keys)
        {
            List<Dictionary<string, string>> typeInstances = new List<Dictionary<string, string>>();

            for (int i = 0; i < amountOfInstancesToCreatePerType; i++)
            {
                T instance = howToBuildDefaultTask(expectedTypes[type], $"{type}: {i}");

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

    public Task<string> RetrieveJson(string queries)
    {
        throw new NotImplementedException();
    }
}
