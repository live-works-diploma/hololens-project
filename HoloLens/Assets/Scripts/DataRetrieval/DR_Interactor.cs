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

    Func<Type, IDataHandler> buildRandomGeneratedClass = type =>
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
        dataRetrieval = new DR_Dummy<IDataHandler>();    // switch instance created to whatever you want aslong as it implements IDataRetrival interface (use DR_Dummy as example)
        dataRetrieval.SetExpectedTypes(typesToListenFor);
        typesToListenFor["Sensor"] = typeof(Sensor);

        StartCoroutine(CheckForDataRoutine());
    }

    public void AddListener<type>(IDRHandler<IDataHandler>.VoidDelegate methodToCallWhenFoundData) where type : IDataHandler
    {
        string nameToListenFor = typeof(type).Name;
        
        AssignListener(nameToListenFor, methodToCallWhenFoundData);
        AssignTypes<type>(nameToListenFor);
    }

    /// <summary>
    /// Takes in the name of a listener and adds a method to the delegate associated with that name so classes can listen for wanted data.
    /// </summary>
    /// <param name="name">Name of the type that is being listened for</param>
    /// <param name="methodToCallWhenFoundData">The method you wish to be called when data on an instance is found.</param>
    void AssignListener(string name, IDRHandler<IDataHandler>.VoidDelegate methodToCallWhenFoundData)
    {
        if (!listeners.ContainsKey(name))
        {
            listeners[name] = null;
        }

        listeners[name] += methodToCallWhenFoundData;
    }

    /// <summary>
    /// Adds a new expected type so when new data comes in there will be a type associated with a string.
    /// </summary>
    /// <typeparam name="T">The type you are listening in for. It needs to implement IDataHandler since this class is set up for it or will cause errors.</typeparam>
    /// <param name="name"></param>
    void AssignTypes<T>(string name)
    {
        typesToListenFor[name] = typeof(T);
    }

    /// <summary>
    /// The routine which checks for any data. Not finished
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckForDataRoutine()
    {
        yield return null;
        RetrieveBuiltData(dataRetrieval);
    }

    /// <summary>
    /// Calls a method on dataRetrieval which starts the process for looking for data.
    /// </summary>
    /// <param name="dataRetrieval">The class which searches for data.</param>
    /// <exception cref="Exception"></exception>
    void RetrieveBuiltData(IDataRetrieval<IDataHandler> dataRetrieval)
    {
        if (dataRetrieval == null)
        {
            throw new Exception("data retrieval hasn't been assigned");
        }

        dataRetrieval.Retrieve(PopulateData, buildClassFromData, howToBreakDownClass, buildRandomGeneratedClass);
    }

    /// <summary>
    /// Gets passed in for RetrieveBuiltData which takes the found data and invokes the delegates listening for data. Iterates through each of the types and passes only the
    /// needed data through.
    /// </summary>
    /// <param name="foundData"></param>
    /// <exception cref="Exception"></exception>
    void PopulateData(Dictionary<string, List<IDataHandler>> foundData)
    {
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

            listeners[key]?.Invoke(foundData[key]);
        }
    }

    /// <summary>
    /// An easy way to turn on or off messages.
    /// </summary>
    /// <param name="message"></param>
    void PrintMessage(string message)
    {
        if (!allowPrintStatements)
        {
            return;
        }

        print(message);
    }
}
