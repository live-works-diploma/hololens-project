using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFillData
{
    void FillData(Dictionary<string, string> dataNeeded);

    Dictionary<string, string> FillDefaultData(float maxDistanceToSpawn = 30);
}
