using System;
using System.Collections;
using System.Collections.Generic;
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
        return new DR_Dummy<IDataHandler>(IDataHandler.howToBuildTask, IDataHandler.howToTurnIntoDictionary, IDataHandler.howToBuildDefaultTask)
        {
            amountOfInstancesToCreatePerType = numberOfInstancesPerType
        };
    }
}
