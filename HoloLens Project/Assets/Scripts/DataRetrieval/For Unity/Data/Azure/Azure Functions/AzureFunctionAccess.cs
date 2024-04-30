using UnityEngine;

/// <summary>
/// Used to access different azure functions. The default key is the key associated with the function app. The url and key is associated with the actual function
/// you wish to call.
/// </summary>
[CreateAssetMenu(fileName = "New Azure Function", menuName = "Connection Info/Azure/Azure Function")]
public class AzureFunctionAccess : ScriptableObject
{
    public string functionUrl;
    public string functionKey;
    public string defaultKey;
}
