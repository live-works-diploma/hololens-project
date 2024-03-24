using UnityEngine;

[CreateAssetMenu(fileName = "New Azure DB", menuName = "Connection Info/Azure/Azure Database")]
public class AzureDBAccess : ScriptableObject
{
    [SerializeField] string serverName;
    [SerializeField] string databaseName;
    public bool persistentSecurityInfo = false;
    [SerializeField] string userId;
    [SerializeField] string password;
    public bool multipleActiveResultSets = false;
    public bool encrypt = true;
    public bool trustServerCertificate = false;

    public string BuildConnectionString(int timeToWaitForConnection = 30)
    {
        return $"Server=tcp:{serverName}.database.windows.net,1433;Initial Catalog={databaseName};Persist Security Info={persistentSecurityInfo};User ID={userId};Password={password};MultipleActiveResultSets={multipleActiveResultSets};Encrypt={encrypt};TrustServerCertificate={trustServerCertificate};Connection Timeout={timeToWaitForConnection};";       
    }
}
