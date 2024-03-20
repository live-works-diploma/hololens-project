using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class DR_Dummy<T> : IDataRetrieval<T> where T : class
{
    [HideInInspector] public List<Type> expectedTypes = new List<Type>();
    [SerializeField] int amountOfInstancesToCreatePerType = 5;

    public void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Func<T, Dictionary<string, string>> howToTurnIntoDictionary, Func<Dictionary<string, string>, Type, T> howToBuildTask)
    {
        throw new Exception("I'm sorry for confusion, you need to use build default data for this class");
    }

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Func<T, Dictionary<string, string>> howToTurnIntoDictionary, Func<Type, T> buildDefaultData)
    {
        callWhenFoundData?.Invoke(await Search(buildDefaultData, howToTurnIntoDictionary));
    }

    async Task<string> Search(Func<Type, T> howToBuildTask, Func<T, Dictionary<string, string>> howToTurnIntoDictionary)
    {
        await Task.Delay(100);  // just to see what happens

        Dictionary<string, List<Dictionary<string, string>>> allInstances = new Dictionary<string, List<Dictionary<string, string>>>();

        for (int i = 0; i < expectedTypes.Count; i++)
        {
            List<Dictionary<string, string>> typeInstances = new List<Dictionary<string, string>>();

            for (int _ = 0; _ < amountOfInstancesToCreatePerType; _++)
            {
                T instance = howToBuildTask(expectedTypes[i]);

                if (instance == null)
                {
                    throw new Exception("instance == null");
                }

                typeInstances.Add(howToTurnIntoDictionary(instance));
            }

            allInstances[expectedTypes[i].Name] = typeInstances;
        }

        return JsonConvert.SerializeObject(allInstances);
    }

    public void Send(string json)
    {
        
    }

    
}
