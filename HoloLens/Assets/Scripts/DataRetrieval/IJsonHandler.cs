using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An interface for retrieving json strings and then converting it into a dictionary. Allows other classes do what they want with the Dicionary.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IJsonHandler<T> where T : class
{
    /// <summary>
    /// converts a json string into the needed format for the main class and creates instances based off the data retrieved
    /// </summary>
    /// <param name="json"></param>
    /// <param name="howToBuildTask"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    Dictionary<string, List<T>> BuildData(string json, Func<Dictionary<string, string>, Type, T> howToBuildTask) 
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets a json string and converts it into a dictionary.
    /// </summary>
    /// <param name="howToBuildTask"></param>
    /// <param name="howToTurnIntoDictionary"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    string RetrieveJson(Func<Type, T> howToBuildTask, Func<T, Dictionary<string, string>> howToTurnIntoDictionary)
    {
        throw new NotImplementedException();
    }        
}
