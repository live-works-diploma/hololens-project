using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorCanvasControl : MonoBehaviour
{
    [SerializeField] GameObject singleInstance;
    [SerializeField] GameObject historyInstance;

    [SerializeField] bool singleShownFirst = true;

    void Start()
    {
        singleInstance.SetActive(singleShownFirst);
        historyInstance.SetActive(!singleShownFirst);
    }

    public void ToggleShown()
    {
        singleInstance.SetActive(!singleInstance.activeSelf);
        historyInstance.SetActive(!historyInstance.activeSelf);
    }
}
