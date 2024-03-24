using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DRInteractor<DataHandler> : IDRHandler<DataHandler> where DataHandler : class
{
    public int delayBetweenCalls = 5000;

    IDataRetrieval<DataHandler> dataRetrieval;

    Dictionary<string, IDRHandler<DataHandler>.VoidDelegate> listeners = new();
    Dictionary<string, Type> typesToListenFor = new();

    int _anchors = 0;
    public int anchors
    {
        get
        {
            return _anchors;
        }
        set
        {
            _anchors = value;
            if (_anchors == 0)
            {
                SearchForData(delayBetweenCalls);
            }
        }
    }

    public DRInteractor(IDataRetrieval<DataHandler> dataRetrival)
    {
        this.dataRetrieval = dataRetrival;
        dataRetrieval.SetExpectedTypes(typesToListenFor);
    }

    public void AddListener<type>(IDRHandler<DataHandler>.VoidDelegate methodToCallWhenFoundData) where type : DataHandler
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
    void AssignListener(string name, IDRHandler<DataHandler>.VoidDelegate methodToCallWhenFoundData)
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
    /// <typeparam name="T">The type you are listening in for. It needs to implement DataHandler since this class is set up for it or will cause errors.</typeparam>
    /// <param name="name"></param>
    void AssignTypes<T>(string name)
    {
        typesToListenFor[name] = typeof(T);
    }

    public async void SearchForData(int delay)
    {
        if (!Application.isPlaying)
        {
            return;
        }

        await FindData(dataRetrieval, delay);
    }

    async Task FindData(IDataRetrieval<DataHandler> dataRetrieval, int delay = 500)
    {
        if (dataRetrieval == null)
        {
            throw new Exception("data retrieval hasn't been assigned");
        }

        await Task.Delay(delay);
        dataRetrieval.Retrieve(PopulateData);   
    }

    /// <summary>
    /// Gets passed in for RetrieveBuiltData which takes the found data and invokes the delegates listening for data. Iterates through each of the types and passes only the
    /// needed data through.
    /// </summary>
    /// <param name="foundData"></param>
    /// <exception cref="Exception"></exception>
    void PopulateData(Dictionary<string, List<DataHandler>> foundData)
    {
        anchors++;
        foreach (var key in foundData.Keys)
        {
            if (!Application.isPlaying)
            {
                return;
            }

            if (!listeners.ContainsKey(key))
            {
                continue;
            }

            if (!typesToListenFor.ContainsKey(key))
            {
                throw new Exception($"Key wasn't present in types to listen for: {key}");
            }

            Type type = typesToListenFor[key];

            listeners[key]?.Invoke(foundData[key]);
        }
        anchors--;
    }
}
