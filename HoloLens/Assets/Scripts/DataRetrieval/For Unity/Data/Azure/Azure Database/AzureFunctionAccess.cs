using UnityEngine;

[CreateAssetMenu(fileName = "New Azure DB", menuName = "Connection Info/Azure/Azure Database")]
public class AzureFunctionAccess : ScriptableObject
{
    public string functionUrl;
    public string functionKey;
    public string defaultKey;
}
