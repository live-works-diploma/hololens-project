using UnityEngine;

/// <summary>
/// This is not used currently. It holds information about how to connect to a blob storage directly.
/// </summary>
[CreateAssetMenu(fileName = "New Azure Blob", menuName = "Connection Info/Azure/Azure Blob")]
public class AzureBlobAccess : ScriptableObject
{
    public string connectionString;
    public string containerName;
}
