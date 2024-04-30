using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// An interface for retrieving json strings and then converting it into a dictionary. Allows other classes do what they want with the Dicionary.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IJsonHandler<T> where T : class
{
    Task<string> RetrieveJson(Dictionary<string, Type> expectedTypes);
}
