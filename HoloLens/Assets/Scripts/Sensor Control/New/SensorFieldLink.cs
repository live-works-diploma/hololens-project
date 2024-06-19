using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SensorFieldLink : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI key = null;   
    public TextMeshProUGUI Key
    {
        get => key;
    }
    
    [SerializeField] TextMeshProUGUI value = null;
    public TextMeshProUGUI Value
    {
        get => value;
    }

    public void AssignKey(string key)
    {
        if (Key == null)
        {
            Debug.LogWarning("Key hasn't been assigned");
            return;
        }

        Key.text = key;
    }

    public void AssignValue(string value)
    {
        if (Value == null)
        {
            Debug.LogError("Key hasn't been assigned");
            return;
        }

        Value.text = value;
    }
}
