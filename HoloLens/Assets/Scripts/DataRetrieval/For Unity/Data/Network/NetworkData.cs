using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Doesn't work, might be used to connect through sockets
/// </summary>
[CreateAssetMenu(fileName = "Network", menuName = "Connection Info/Network")]
public class NetworkData : ScriptableObject
{
    public string ipAddress = "192.168.1.100";
    public int port = 8080;

    public string url
    {
        get
        {
            return $"http://{ipAddress}:{port}";
        }
    }
}
