using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// An interface for interacting with classes which retrieve data. 
/// </summary>
/// <typeparam name="T">
/// The type of class you are creating with the data. It is meant to be a parent class so you can add in different classes to listen for but doesn't matter since its just 
/// used as a type for a list that gets used as an argument for a delegate.
/// </typeparam>
public interface IDRHandler<T> where T : class
{
    /// <summary>
    /// Allows listeners to add in the methods they want to be called when the data they are looking for is found.
    /// </summary>
    /// <param name="foundData">
    /// A list of each instance found. Each instance should be the same type even though it uses a parent type.
    /// </param>
    delegate void VoidDelegate(List<T> foundData);

    /// <summary>
    /// How to allow other classes to listen for data being retrieved.
    /// </summary>
    /// <typeparam name="type">
    /// The type of data you are looking for. It takes the name of the class and when data is found with that name it calls the delegate argument. It also takes that
    /// type of class and generates default data based off it.
    /// </typeparam>
    /// <param name="methodToCallWhenFoundData">The method you wish to be invoked when the data you are looking for is created.</param>
    void AddListener<type>(VoidDelegate methodToCallWhenFoundData) where type : T;

    void SearchForData();
}
