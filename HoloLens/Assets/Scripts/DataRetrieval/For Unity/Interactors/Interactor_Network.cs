using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Network : MonoBehaviour, IInteractor<IDataHandler>
{
    DRInteractor<IDataHandler> dataRetrieval;
    public NetworkData networkData;

    void Start()
    {
        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval());
        dataRetrieval.SearchForData();
    }

    DR_Network<IDataHandler> CreateDataRetrieval()
    {
        Func<Dictionary<string, string>, Type, IDataHandler> howToBuildTask = (data, type) =>
        {
            IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
            instance.FillData(data);

            return instance;
        };

        DR_Network<IDataHandler> dataRetriever = new DR_Network<IDataHandler>(howToBuildTask, networkData.url);

        return dataRetriever;
    }

    public void AddListener<type>(IDRHandler<IDataHandler>.VoidDelegate methodToCall) where type : IDataHandler
    {
        dataRetrieval.AddListener<type>(methodToCall);
    }
}
