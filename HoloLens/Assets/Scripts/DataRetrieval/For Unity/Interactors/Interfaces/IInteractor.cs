using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface IInteractor<T> where T : class
{
    DRInteractor<T> dataRetrieval { get; set; }

    public void AddListener<type>(MonoBehaviour monoBehaviour, IDRHandler<T>.VoidDelegate methodToCall) where type : T
    {
        monoBehaviour.StartCoroutine(DelayedAddListener<type>(methodToCall));
    }

    private IEnumerator DelayedAddListener<type>(IDRHandler<T>.VoidDelegate methodToCall) where type : T
    {
        while (dataRetrieval == null)
        {
            yield return null;
        }

        dataRetrieval.AddListener<type>(methodToCall);
    }

    void AlterAnchors(int alterAmount = 1)
    {
        if (dataRetrieval == null)
        {
            throw new System.Exception("Data retrieval hasn't been assigned");
        }

        dataRetrieval.anchors += alterAmount;
    }
}
