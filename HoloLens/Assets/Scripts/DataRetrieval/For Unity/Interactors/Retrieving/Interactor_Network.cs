using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_Network : MonoBehaviour, IInteractor<IDataHandler>
{
    public DRInteractor<IDataHandler> dataRetrieval { get; set; }

    public NetworkData networkData;
    public int initialDelay = 500;
    public int delayInbetweenCalls = 5000;

    void Start()
    {
        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval())
        {
            delayBetweenCalls = delayInbetweenCalls
        };
        dataRetrieval.SearchForData(initialDelay);
    }

    DR_Network<IDataHandler> CreateDataRetrieval()
    {
        return new DR_Network<IDataHandler>();
    }
}
