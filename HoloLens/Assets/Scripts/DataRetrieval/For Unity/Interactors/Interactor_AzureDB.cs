using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor_AzureDB : MonoBehaviour, IInteractor<IDataHandler>
{
    public DRInteractor<IDataHandler> dataRetrieval { get; set; }
    public AzureDBAccess azureAccount;

    private void Start()
    {
        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval());
        dataRetrieval.SearchForData(500);
    }

    DR_AzureDB<IDataHandler> CreateDataRetrieval()
    {
        return new DR_AzureDB<IDataHandler>()
        {
            connectionString = azureAccount.BuildConnectionString()
        };
    }
}
