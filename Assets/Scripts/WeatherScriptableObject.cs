using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/WeatherSO", fileName = "WeatherSO.asset")]
[System.Serializable]
public class WeatherScriptableObject : ScriptableObject
{
    static public WeatherScriptableObject S; // This Scriptable Object is an unprotected Singleton

    [Header("Set in Inspector")]
    public GameObject[] weatherPrefabs;

    public WeatherScriptableObject()
    {
        S = this; // Assign the Singleton as part of the constructor.
    }

    public GameObject GetWeatherPrefab(int ndx)
    {
        return weatherPrefabs[ndx];
    }

}
