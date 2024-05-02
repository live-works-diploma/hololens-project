using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// The class that acts as an abstract layer for retrieving data from an azure database. This class creates an instance of the class that will be used to retrieve the data
/// and then passes that instance to another abstract layer which makes it so it retrieves data in a controlled infinite loop
/// </summary>
public class Interactor_AzureDB : MonoBehaviour, IDRInteractor<IDataHandler>
{
    public DRInteractor<IDataHandler> dataRetrieval { get; set; }

    [Tooltip("True means will access blob storage to get data and False means will access database")]
    public bool accessBlobStorage = true;   
    [Tooltip("Not in seconds, think miliseconds. Used so the other classes have time to add in their own listeners and you don't waste a call. Won't make much different since loops anyway.")]
    public int initialDelay = 500;
    public int deley = 5000;

    [Header("Links")]
    public AzureFunctionAccess azureAccount;
    public TextMeshProUGUI errorText;

    [Header("Debuging")]
    [SerializeField] Logger logger;
    Action<string> log;
    Action<string> errorLog;

    private void Start()
    {
        log = message => logger?.Log(message);
        errorLog = log;

        dataRetrieval = new DRInteractor<IDataHandler>(CreateDataRetrieval())
        {
            delayBetweenCalls = deley,

            logger = log,
            errorLogger = errorLog,
        };
        dataRetrieval.SearchForData(initialDelay);
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

            useBlobStorage = accessBlobStorage,

            howToBuildTask = (data, type) =>
            {
                IDataHandler instance = (IDataHandler)Activator.CreateInstance(type);
                return instance.BuildTask(data);
            },

            logger = log,
            ErrorLogger = errorLog,
        };
    }
}
