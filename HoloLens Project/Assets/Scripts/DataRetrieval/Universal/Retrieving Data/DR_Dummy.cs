using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    Func<Dictionary<string, object>, Type, T> buildInstance;
    Func<T, Dictionary<string, object>> turnInstanceToDictionary;
    Func<Type, string, T> createRandomInstanceData;

    public int amountOfInstancesToCreatePerType = 5;
    public Action<string> logger;

    public DR_Dummy(Func<Dictionary<string, object>, Type, T> howToBuildTask, Func<T, Dictionary<string, object>> howToTurnIntoDictionary, Func<Type, string, T> howToBuildDefaultTask)
    {
        if (howToBuildTask == null || howToBuildDefaultTask == null || howToTurnIntoDictionary == null)
        {
            throw new ArgumentNullException("Funcs can't be null");
        }

        this.createRandomInstanceData = howToBuildDefaultTask;
        this.turnInstanceToDictionary = howToTurnIntoDictionary;
        this.buildInstance = howToBuildTask;
    }

    public async Task Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Dictionary<string, Type> expectedTypes)
    {
        if (expectedTypes == null || callWhenFoundData == null)
        {
            throw new ArgumentNullException("Arguments can't be null");
        }        

        foreach (var type in expectedTypes.Values)
        {
            if (!typeof(T).IsAssignableFrom(type))
            {
                throw new ArgumentException("Each type in expectedTypes must use T as a parent type or be the parent type");
            }
        }
        
        logger?.Invoke("Passed argument checks");

        string jsonData = await RetrieveJson(expectedTypes);

        logger?.Invoke($"found json data: {jsonData}");

        Dictionary<string, List<T>> data = await JsonBuildTask<T>.BuildData(jsonData, buildInstance, expectedTypes);

        logger?.Invoke("Built data");

        callWhenFoundData?.Invoke(data);
    }

    public async Task<string> RetrieveJson(Dictionary<string, Type> expectedTypes)
    {
        if (expectedTypes == null)
        {
            throw new ArgumentNullException("Expected types can't be null");
        }

        logger?.Invoke("Building random instance data");

        Dictionary<string, List<Dictionary<string, object>>> data = new();

        foreach (var kvp in expectedTypes)
        {
            if (!typeof(T).IsAssignableFrom(kvp.Value))
            {
                throw new ArgumentException($"{kvp.Key} doesn't implement {typeof(T).Name}");
            }

            logger?.Invoke($"Building data for {kvp.Key}");
            List<Dictionary<string, object>> instances = new List<Dictionary<string, object>>();

            for (int i = 0; i < amountOfInstancesToCreatePerType; i++)
            {
                logger?.Invoke($"Building instance ({i}) for {kvp.Key}");
                T instance = createRandomInstanceData(kvp.Value, $"{kvp.Key} ({i})");
                var instanceData = turnInstanceToDictionary(instance);
                instances.Add(turnInstanceToDictionary(instance));
            }

            data[kvp.Key] = instances;
        }

        logger?.Invoke($"Number of different types created: {data.Count}");

        return JsonConvert.SerializeObject(await Task.FromResult(data));
    }
}
