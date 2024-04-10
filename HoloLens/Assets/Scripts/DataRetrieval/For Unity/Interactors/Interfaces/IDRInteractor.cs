using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Saying the class that implements this interface is an abstract layer designed as a chokepoint for accessing data retrieved. This deals with all the needed code, all the 
/// class has to implement is where it gets its data from. For that to happen all it needs to do is fill out data retrieval. This is the class that deals with the retrieval
/// of data.
/// </summary>
/// <typeparam name="T">The parent type of the data you wish to listen for. Every class that you add to listen for needs to implement this type.</typeparam>
public interface IDRInteractor<T> where T : class
{
    DRInteractor<T> dataRetrieval { get; set; }

    /// <summary>
    /// Allows other classes to say what type of data they are listening for.
    /// </summary>
    /// <typeparam name="type">The type of data you wish to listen for. It needs to have T as a parent type.</typeparam>
    /// <param name="monoBehaviour">Just used to start the routine. I don't like it but it is what it is</param>
    /// <param name="methodToCall">When there is data found with the same name as the type being passed in, it feeds that data back through this method.</param>
    public void AddListener<type>(MonoBehaviour monoBehaviour, IDRHandler<T>.VoidDelegate methodToCall) where type : T
    {
        monoBehaviour.StartCoroutine(DelayedAddListener<type>(methodToCall));
    }

    /// <summary>
    /// Acts as a way to stop race time conditions where a class is saying it is listening for a class but the class that listens for data hasn't been set yet.
    /// </summary>
    /// <typeparam name="type">The type of data you wish to listen for.</typeparam>
    /// <param name="methodToCall">The method that is called when data is found with the same name.</param>
    /// <returns></returns>
    IEnumerator DelayedAddListener<type>(IDRHandler<T>.VoidDelegate methodToCall) where type : T
    {
        while (dataRetrieval == null)
        {
            yield return null;
        }

        dataRetrieval.AddListener<type>(methodToCall);
    }

    /// <summary>
    /// Use when you have something that needs to finish before the next call is made. The timer for retrieving new data doesn't start until anchors is 0 so if you increase
    /// make sure you decrease the same amount of anchors or it will lead to silent errors.
    /// </summary>
    /// <param name="alterAmount">The amount of anchors you wish to increase or decrease. Use negative numbers for decreasing.</param>
    /// <exception cref="System.Exception">
    /// Throws error if the class for retrieving data hasn't been set yet. This shouldn't happen since you should only change
    /// the anchors inside the method that is being passed in through the listener.
    /// </exception>
    void AlterAnchors(int alterAmount = 1)
    {
        if (dataRetrieval == null)
        {
            throw new System.Exception("Data retrieval hasn't been assigned");
        }

        dataRetrieval.anchors += alterAmount;
    }
}
