using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creation_Base : MonoBehaviour
{
    public float locationX = 0;
    public float locationY = 0;
    public float locationZ = 0;

    public GameObject prefab;

    internal void ResetPosition()
    {
        Vector3 newLocation = new Vector3(locationX, locationY, locationZ);
        gameObject.transform.position = newLocation;
    }
}
