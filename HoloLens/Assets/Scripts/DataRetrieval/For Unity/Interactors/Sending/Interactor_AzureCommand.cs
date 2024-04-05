using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Interactor_AzureCommand : MonoBehaviour
{
    public AzureFunctionAccess azureAccount;
    public DR_AzureCommands interactor;

    private void Start()
    {
        interactor = new DR_AzureCommands()
        {
            functionKey = azureAccount.functionKey,
            functionUrl = azureAccount.functionUrl,
            defaultKey = azureAccount.defaultKey,
        };

        SendCommand("Ping");
    }

    public async void SendCommand(string command)
    {
        await interactor.SendCommand(command);
    }
}
