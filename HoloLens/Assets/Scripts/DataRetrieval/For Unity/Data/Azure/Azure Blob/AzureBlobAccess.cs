using UnityEngine;

[CreateAssetMenu(fileName = "New Azure Blob", menuName = "Connection Info/Azure/Azure Blob")]
public class AzureBlobAccess : ScriptableObject
{
    public string connectionString;
    public string containerName;
}
