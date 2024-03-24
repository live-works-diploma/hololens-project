using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleUsage : MonoBehaviour
{
    IInteractor<IDataHandler> interactor;

    void Start()
    {
        // find the instance created so there isn't multiple copies retrieving the same data (causes overhead). Its designed so there is an empty GameObject which
        // contains the interactor. Every interactor needs to implement an interface called IInteractor so you can find the instance by searching for the interface.
        // This way we can just switch out the interactor instance and you won't have to edit any code.
        interactor = GetComponent<IInteractor<IDataHandler>>();

        // This is how you add an type to listen for. When ever there is data found which shares the same name as the class it calls the method that is being passed into it.
        // The type being passed in needs to implement the interface IDataHandler. The method you pass it needs to have one argument which is a List of IDataHandler. Each
        // instance that gets passed into your method when the delegate holding the method gets invoked will be the same type just labeled as IDataHandler.
        interactor.AddListener<Plant>(this, Listener);
        interactor.AddListener<Sensor>(this, Listener);
    }

    void Listener(List<IDataHandler> foundData)
    {
        // Do whatever you want with the retrieved data. I might add in an anchors for the interactor so there is less racetime conditions but not implemented yet. If
        // added it will work where you increase the anchor for when you have something that needs to be completed before the next call for data is made and you decrease
        // when its finished.
        interactor.AlterAnchors(1);
        print($"found {foundData.Count} instances of {foundData[0].GetType().Name}");
        interactor.AlterAnchors(-1);
    }
}
