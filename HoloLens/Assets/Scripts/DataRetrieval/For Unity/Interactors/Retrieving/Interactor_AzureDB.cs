using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The class that acts as an abstract layer for retrieving data from an azure database. This class creates an instance of the class that will be used to retrieve the data
/// and then passes that instance to another abstract layer which makes it so it retrieves data in a controlled infinite loop
/// </summary>
public class Interactor_AzureDB : MonoBehaviour, IInteractor<IDataHandler>
{
    public DRInteractor<IDataHandler> dataRetrieval { get; set; }
    public AzureFunctionAccess azureAccount;
    public TextMeshProUGUI errorText;


    private void Start()
    {
        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval());
        dataRetrieval.SearchForData(500);
    }

    /// <summary>
    /// Creating the instance that will be used to retrieve data.
    /// </summary>
    /// <returns></returns>
    DR_AzureDB<IDataHandler> CreateDataRetrieval()
    {
        return new DR_AzureDB<IDataHandler>()
        {
            functionKey = azureAccount.functionKey,
            functionUrl = azureAccount.functionUrl,
            defaultKey = azureAccount.defaultKey,

            howToBuildTask = (data, type) =>
            {
                IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
                return instance.BuildTask(data);
            },

            logger = error =>
            {
                errorText.text = error;
                print(error);
            },
        };
    }
}
