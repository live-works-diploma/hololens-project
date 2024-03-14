using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class ObjectData : MonoBehaviour
{
    internal Vector3 positionLastSet;
    internal Vector3 scaleLastSet;

    void Start()
    {
        SetPositionAndScale(gameObject.transform.position, gameObject.transform.localScale.y);
    }

    public virtual void SetPositionAndScale(Vector3 position, float scale)
    {
        Vector3 newScale = Vector3.one * scale;

        positionLastSet = position;
        scaleLastSet = newScale;

        ResetPosition();
    }

    public virtual void ResetPosition()
    {        
        gameObject.transform.position = positionLastSet;
        gameObject.transform.localScale = scaleLastSet;
    }
}
