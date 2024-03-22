using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Just a Demo class. It listens for any data involving Plant or Sensor and then a method gets called which iterates over the found data and prints the type.
/// </summary>
public class Interactor_Dummy : MonoBehaviour, IInteractor<IDataHandler>
{
    IDRHandler<IDataHandler> dataRetrieval;

    void Start()
    {
        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval());
        dataRetrieval.SearchForData();
    }

    DR_Dummy<IDataHandler> CreateDataRetrieval()
    {
        Func<Dictionary<string, string>, Type, IDataHandler> howToBuildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            instance.FillData(data);

            return instance;
        };

        Func<IDataHandler, Dictionary<string, string>> howToTurnIntoDictionary = instance =>
        {
            return instance.TurnDataIntoDictionary();
        };

        Func<Type, IDataHandler> howToBuildDefaultTask = type =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            instance.FillData(instance.CreateDefaultData(0));

            return instance;
        };

        return new DR_Dummy<IDataHandler>(howToBuildTask, howToTurnIntoDictionary, howToBuildDefaultTask);
    }

    public void AddListener<T>(IDRHandler<IDataHandler>.VoidDelegate methodToCall) where T : IDataHandler
    {
        dataRetrieval.AddListener<T>(methodToCall);
    }
}
