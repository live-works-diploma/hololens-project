using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Interactor_AzureCommand : MonoBehaviour
{
    public AzureFunctionAccess azureAccount;
    public DS_AzureCommands interactor;

    private void Start()
    {
        interactor = new DS_AzureCommands()
        {
            functionKey = azureAccount.functionKey,
            functionUrl = azureAccount.functionUrl,
            defaultKey = azureAccount.defaultKey,

            logger = message =>
            {
                print(message);
            }
        };

        SendCommand("Ping");
    }

    public async void SendCommand(string command)
    {
        await interactor.SendCommand(command);
    }
}
