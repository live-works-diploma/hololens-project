using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Network : MonoBehaviour, IInteractor<IDataHandler>
{
    public DRInteractor<IDataHandler> dataRetrieval { get; set; }

    public NetworkData networkData;
    public int initialDelay = 500;

    void Start()
    {
        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval());
        dataRetrieval.SearchForData(initialDelay);
    }

    DR_Network<IDataHandler> CreateDataRetrieval()
    {
        return new DR_Network<IDataHandler>(IDataHandler.howToBuildTask, networkData.url);
    }
}
