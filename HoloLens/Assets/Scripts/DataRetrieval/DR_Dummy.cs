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

    public async void Retrieve(IDataRetrieval<T>.VoidDelegate callWhenFoundData, Func<Type, T> howToBuildTask)
    {
        callWhenFoundData?.Invoke(await Search(howToBuildTask));
    }

    async Task<string> Search(Func<Type, T> howToBuildTask)
    {
        await Task.Delay(100);  // just to see what happens

        Dictionary<string, List<T>> allInstances = new Dictionary<string, List<T>>();

        for (int i = 0; i < expectedTypes.Count; i++)
        {
            List<T> typeInstances = new List<T>();

            for (int _ = 0; _ < amountOfInstancesToCreatePerType; _++)
            {
                T instance = howToBuildTask(expectedTypes[i]);

                if (instance == null)
                {
                    throw new Exception("instance == null");
                }

                typeInstances.Add(instance);
            }

            allInstances[expectedTypes[i].Name] = typeInstances;
        }

        return JsonConvert.SerializeObject(allInstances);
    }

    public void Send(string json)
    {
        
    }
}
