using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Interactor_Dummy : MonoBehaviour, IInteractor<IDataHandler>
{
    public DRInteractor<IDataHandler> dataRetrieval { get; set; }

    public int initialDelay = 500;
    public int delayInbetweenCalls = 5000;
    public int numberOfInstancesPerType = 1;

    void Start()
    {
        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval())
        {
            delayBetweenCalls = delayInbetweenCalls
        };
        dataRetrieval.SearchForData(initialDelay);
    }

    DR_Dummy<IDataHandler> CreateDataRetrieval()
    {
        Func<Dictionary<string, string>, Type, IDataHandler> buildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildTask(data);
        };

        Func<IDataHandler, Dictionary<string, string>> turnIntoDictionary = instance =>
        {
            return instance.TurnDataIntoDictionary();
        };

        Func<Type, string, IDataHandler> buildRandomInstance = (type, name) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            return instance.BuildRandomInstance();
        };

        return new DR_Dummy<IDataHandler>(buildTask, turnIntoDictionary, buildRandomInstance)
        {
            amountOfInstancesToCreatePerType = numberOfInstancesPerType
        };
    }
}
