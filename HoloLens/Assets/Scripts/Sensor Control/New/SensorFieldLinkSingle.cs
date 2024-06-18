using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorFieldLinkSingle : SensorFieldLink
{
    [SerializeField] DataTypes.DatabaseTypes fieldWanted;
    public string FieldWanted => fieldWanted.ToString();
}
