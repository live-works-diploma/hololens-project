using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface for retrieving data. It is meant to be a class which other classes listen in on and is meant to be paired with other interfaces for how it retrieves
/// the data, like IJsonHandler.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IDataRetrieval<T> where T : class
{
    /// <summary>
    /// Used to pass a list of each instance found / created back to the main class. Each key should say the name of the class and the list should be a list of instances
    /// reflecting the key.
    /// </summary>
    /// <param name="wantedTypes">The list of each instance</param>
    delegate void VoidDelegate(Dictionary<string, List<T>> wantedTypes);

    /// <summary>
    /// The method that is called by the main class to find data. It doesn't matter how its achieved aslong as when finished you call the delegate. The data should be
    /// constructed as { "name of instance": [ { "found data": "value", "found data", "value" } * number of instances found ] }. the name of the instances should 
    /// reflect a created class, like there is a class called "Plant" so that will be the name of the instance.
    /// </summary>
    /// <param name="callWhenFoundData">The method that is called when data has been found. Passes the data found through the argument.</param>
    /// <param name="howToBuildTask">
    /// Builds an instance of a wanted type. Passes in the data found for the instance and the type of instance it is then returns the created instance.
    /// </param>
    /// <param name="howToTurnIntoDictionary">
    /// Turns an instance into a dinctionary format so it can be serialized and deserialized. Only being used for creating default data.
    /// </param>
    /// <param name="buildDefaultData">
    /// Shows the class how to create default data for an instance. It passes in the type of instance and returns an instance populated with the default data. Is only needed
    /// when creating default data. 
    /// </param>
    void Retrieve(VoidDelegate callWhenFoundData);

    /// <summary>
    /// A way to tell the class which types are expected. It uses these types to create default data and create new instances of the type with default / found data
    /// </summary>
    /// <param name="typesToListenFor"></param>
    void SetExpectedTypes(Dictionary<string, Type> typesToListenFor);
}
