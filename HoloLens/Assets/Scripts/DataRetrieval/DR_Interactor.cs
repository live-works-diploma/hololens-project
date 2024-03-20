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
    
    void Start()
    {
        List<Type> expectedTypes = new List<Type>() 
        { 
            typeof(Plant),
            typeof(Sensor),
        };

        dataRetrieval = new DR_Dummy<IDataHandler>()    // switch instance created to whatever you want aslong as it implements IDataRetrival interface (use DR_Dummy as example)
        {
            expectedTypes = expectedTypes,  // this is only for generating default data, dont need to implement when using real data
        };

        StartCoroutine(CheckForDataRoutine());
    }

    public void AddListener(IDRHandler<IDataHandler>.VoidDelegate methodToCallWhenFoundData, string nameToListenFor)
    {
        if (!listeners.ContainsKey(nameToListenFor))
        {
            listeners[nameToListenFor] = null;
        }

        listeners[nameToListenFor] += methodToCallWhenFoundData;
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

        dataRetrieval.Retrieve(PopulateData, type =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            instance.FillData(instance.CreateDefaultData(0));

            return instance;
        });
    }

    void PopulateData(string json)
    {
        Dictionary<string, List<IDataHandler>> foundData = JsonConvert.DeserializeObject<Dictionary<string, List<IDataHandler>>>(json);      
        
        foreach (var key in foundData.Keys)
        {
            if (!listeners.ContainsKey(key))
            {
                PrintMessage($"found key without any listeners for it: {key}");
                continue;
            }

            listeners[key]?.Invoke(foundData[key]);
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
