using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Azure : MonoBehaviour, IInteractor<IDataHandler>
{
    DRInteractor<IDataHandler> dataRetrieval;
    public AzureData azureData;

    void Start()
    {
        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval());
        dataRetrieval.SearchForData();
    }

    DR_Azure<IDataHandler> CreateDataRetrieval()
    {
        Func<Dictionary<string, string>, Type, IDataHandler> howToBuildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            instance.FillData(data);

            return instance;
        };

        DR_Azure<IDataHandler> dataRetriever = new DR_Azure<IDataHandler>(howToBuildTask, azureData.connectionString, azureData.containerName);

        return dataRetriever;
    }

    public void AddListener<type>(IDRHandler<IDataHandler>.VoidDelegate methodToCall) where type : IDataHandler
    {
        dataRetrieval?.AddListener<type>(methodToCall);
    }
}
