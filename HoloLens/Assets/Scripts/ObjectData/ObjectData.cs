using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    internal Vector3 positionLastSet;
    internal Vector3 scaleLastSet;

    void Start()
    {
        SetPositionAndScale(gameObject.transform.position, gameObject.transform.localScale);
        StartArg();
    }

    public virtual void SetPositionAndScale(Vector3 position, Vector3 scale)
    {
        positionLastSet = position;
        scaleLastSet = scale;

        gameObject.transform.position = position;
        gameObject.transform.localScale = scale;
    }

    public virtual void ResetPosition(GameObject objectToReset)
    {
        objectToReset.transform.position = gameObject.transform.position;
    }

    internal virtual void StartArg()
    {

    }
}
