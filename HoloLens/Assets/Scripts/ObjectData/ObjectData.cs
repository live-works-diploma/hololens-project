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
        SetPositionAndScale(gameObject.transform.position, gameObject.transform.localScale);
    }

    public virtual void SetPositionAndScale(Vector3 position, Vector3 scale)
    {
        positionLastSet = position;
        scaleLastSet = scale;

        ResetPosition();
    }

    public virtual void ResetPosition()
    {        
        gameObject.transform.position = positionLastSet;
        gameObject.transform.localScale = scaleLastSet;
    }
}
