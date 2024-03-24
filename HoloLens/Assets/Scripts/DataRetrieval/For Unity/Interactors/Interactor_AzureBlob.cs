using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Interactor_AzureBlob<T> : MonoBehaviour, IInteractor<IDataHandler>
{ 
    public DRInteractor<IDataHandler> dataRetrieval { get; set; }

    public AzureBlobAccess azureData;
    public int initialDelay = 500;   

    void Start()
    {
        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval());
        dataRetrieval.SearchForData(initialDelay);
    }

    DR_AzureBlob<IDataHandler> CreateDataRetrieval()
    {
        return new DR_AzureBlob<IDataHandler>(IDataHandler.howToBuildTask, azureData.connectionString, azureData.containerName);
    }
}
