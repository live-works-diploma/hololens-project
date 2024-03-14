using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreationData : MonoBehaviour
{
    public float locationX = 0;
    public float locationY = 0;
    public float locationZ = 0;

    public GameObject prefab;

    public void SetPositionAndScale(Vector3 position, float scale)
    {
        transform.position = position;
        transform.localScale = Vector3.one * scale;
    }

    internal void ResetPosition()
    {
        Vector3 newLocation = new Vector3(locationX, locationY, locationZ);
        gameObject.transform.position = newLocation;
    }
}
