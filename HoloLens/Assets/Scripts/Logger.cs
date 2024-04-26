using Codice.CM.Client.Differences.Merge;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnknownLogger", menuName = "Loggers/Base Logger")]
public class Logger : ScriptableObject
{
    [SerializeField] bool allowLogging = true;

    public void Log(string message)
    {
        if (!allowLogging)
        {
            return;
        }

        Debug.Log(message);
    }
}
