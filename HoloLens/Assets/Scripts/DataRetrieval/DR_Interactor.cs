using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DR_Interactor : MonoBehaviour, IDRHandler<IDataHandler>
{
    [SerializeField] bool allowPrintStatements = true;

    IDataRetrieval<IDataHandler> dataRetrieval;

    Dictionary<string, IDRHandler<IDataHandler>.VoidDelegate> listeners = new();
    Dictionary<string, Type> typesToListenFor = new();

    Func<Dictionary<string, string>, Type, IDataHandler> buildClassFromData = (data, type) =>
    {
        IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
        instance.FillData(data);

        return instance;
    };

    Func<Type, IDataHandler> buildRandomGeneratedClass = (type) =>
    {
        // data isnt used its just so it accepts it as an arg for dummy data since prob will need data for real class
        IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
        instance.FillData(instance.CreateDefaultData(0));

        return instance;
    };

    Func<IDataHandler, Dictionary<string, string>> howToBreakDownClass = instance =>
    {
        return instance.TurnDataIntoDictionary();
    };

    void Start()
    {
        List<Type> expectedTypes = new List<Type>();

        foreach (var type in typesToListenFor.Keys)
        {
            expectedTypes.Add(typesToListenFor[type]);
        }

        dataRetrieval = new DR_Dummy<IDataHandler>()    // switch instance created to whatever you want aslong as it implements IDataRetrival interface (use DR_Dummy as example)
        {
            expectedTypes = expectedTypes,  // this is only for generating default data, dont need to implement when using real data
        };

        StartCoroutine(CheckForDataRoutine());
    }

    public void AddListener<type>(IDRHandler<IDataHandler>.VoidDelegate methodToCallWhenFoundData)
    {
        string nameToListenFor = typeof(type).Name;
        
        AssignListener(nameToListenFor, methodToCallWhenFoundData);
        AssignTypes<type>(nameToListenFor);
    }

    void AssignListener(string name, IDRHandler<IDataHandler>.VoidDelegate methodToCallWhenFoundData)
    {
        if (!listeners.ContainsKey(name))
        {
            listeners[name] = null;
        }

        listeners[name] += methodToCallWhenFoundData;
    }

    void AssignTypes<T>(string name)
    {
        typesToListenFor[name] = typeof(T);
    }

    IEnumerator CheckForDataRoutine()
    {
        yield return null;
        RetrieveBuiltData();
    }

    void RetrieveBuiltData()
    {
        if (dataRetrieval == null)
        {
            throw new Exception("data retrieval hasn't been assigned");
        }

        dataRetrieval.Retrieve(PopulateData, howToBreakDownClass, buildRandomGeneratedClass);
    }

    void PopulateData(string json)
    {
        Dictionary<string, List<Dictionary<string, string>>> foundData = JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, string>>>>(json);      
        
        foreach (var key in foundData.Keys)
        {
            if (!listeners.ContainsKey(key))
            {
                PrintMessage($"found key without any listeners for it: {key}");
                continue;
            }

            if (!typesToListenFor.ContainsKey(key))
            {
                throw new Exception($"Key wasn't present in types to listen for: {key}");
            }

            Type type = typesToListenFor[key];

            List<IDataHandler> newInstances = new List<IDataHandler>();

            for (int i = 0; i < foundData[key].Count; i++)
            {
                newInstances.Add(buildClassFromData(foundData[key][i], type));
            }

            listeners[key]?.Invoke(newInstances);
        }
    }

    public void SendData()
    {
        throw new NotImplementedException();
    }

    void PrintMessage(string message)
    {
        if (!allowPrintStatements)
        {
            return;
        }

        print(message);
    }
}
