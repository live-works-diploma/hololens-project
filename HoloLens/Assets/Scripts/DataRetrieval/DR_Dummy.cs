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
    [SerializeField] int amountOfInstancesToCreatePerType = 5;

    Dictionary<string, Type> expectedTypes = new Dictionary<string, Type>();
    public void SetExpectedTypes(Dictionary<string, Type> typesToListenFor)
    {
        this.expectedTypes = typesToListenFor;
    }

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Func<Dictionary<string, string>, Type, T> howToBuildTask,Func<T, Dictionary<string, string>> howToTurnIntoDictionary = null,  Func<Type, T> buildDefaultData = null)
    {
        callWhenFoundData?.Invoke(await Search(howToBuildTask, buildDefaultData, howToTurnIntoDictionary));
    }

    /// <summary>
    /// Calls a method which retrieves the json string and then builds the instances based off that string. It returns the instances with in the correct format.
    /// </summary>
    /// <param name="howToBuildTask">Takes in the data and type for an instances and makes you build the instance yourself. Then returns that instance.</param>
    /// <param name="howToCreateDefaultData">Takes in a type and makes you create an instance based off that type. Doesn't force there to be default data just an instance.</param>
    /// <param name="howToTurnIntoDictionary">Takes in an instance and turns it into Dictionary format so it can be serialized.</param>
    /// <returns>Each instance. The key is the name of the instance type and the list is each instance related to that type. It returns all instances as type T.</returns>
    async Task<Dictionary<string, List<T>>> Search(Func<Dictionary<string, string>, Type, T> howToBuildTask, Func<Type, T> howToCreateDefaultData = null, Func<T, Dictionary<string, string>> howToTurnIntoDictionary = null)
    {
        await Task.Delay(100);  // just to see what happens

        string foundDataJson = RetrieveJson(howToCreateDefaultData, howToTurnIntoDictionary);

        return BuildData(foundDataJson, howToBuildTask);
    }

    /// <summary>
    /// Creates a list of instances based off the expected types and add its to a dictionary. The dictionary key is the name of the instance type. It then turns it into a 
    /// json string.
    /// </summary>
    /// <param name="howToBuildDefaultTask">Takes in the type and builds default data based off it. Then returns the new instance.</param>
    /// <param name="howToTurnIntoDictionary">Takes in an instance and converts it to dictionary format. Then returns that Dictionary.</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    string RetrieveJson(Func<Type, T> howToBuildDefaultTask, Func<T, Dictionary<string, string>> howToTurnIntoDictionary)
    {
        Dictionary<string, List<Dictionary<string, string>>> allInstances = new Dictionary<string, List<Dictionary<string, string>>>();

        foreach (var type in expectedTypes.Keys)
        {
            List<Dictionary<string, string>> typeInstances = new List<Dictionary<string, string>>();

            for (int i = 0; i < amountOfInstancesToCreatePerType; i++)
            {
                T instance = howToBuildDefaultTask(expectedTypes[type]);

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

    /// <summary>
    /// Takes in a json string and then converts that string into a of instances. This list is added to a dictionary where the key is the is the name of the type of instance.
    /// </summary>
    /// <param name="json">The json string which contains the data found</param>
    /// <param name="howToBuildTask">Takes in the data and type for the instance and creates a new instance. It then returns that instance and adds it to a list.</param>
    /// <returns>All the instances found.</returns>
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
