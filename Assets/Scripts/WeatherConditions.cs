using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeatherConditions : MonoBehaviour
{

    static Vector3 spawnCoordinatesClearDay = new Vector3(90, 360, 476); //Clear cloud prefab location
    static Vector3 spawnCoordinatesRainDay = new Vector3(90, 375, 490); //Rain cloud prefab location

    static int weatherCondition;

    // Events
    private ClearDay clearDay;
    private RainyDay rainyDay;

    [Header("Set Dinamically")]
    public bool sonido;


    void Awake()
    {
        // Add event invoker
        rainyDay = new RainyDay();
        EventManager.AddRainyDayInvoker(this);

        // Add event invoker
        clearDay = new ClearDay();
        EventManager.AddClearDayInvoker(this);

        sonido = false;
    }

    void Update()
    {

        DetectDistance();
    }

    static public WeatherConditions SpawnCondition(int ndx)
    {
        GameObject wGO; // Weather Game Object
        weatherCondition = ndx;
        switch (weatherCondition)
        {
            case 0:
                wGO = Instantiate<GameObject>(Weather3D.WeatherSO.GetWeatherPrefab(ndx), spawnCoordinatesClearDay, Quaternion.Euler(-15, -70, 48));
                break;
            case 1:
                wGO = Instantiate<GameObject>(Weather3D.WeatherSO.GetWeatherPrefab(ndx), spawnCoordinatesRainDay, Quaternion.Euler(0, 0, 5));
                break;
            default://Default: Clear
                wGO = Instantiate<GameObject>(Weather3D.WeatherSO.GetWeatherPrefab(ndx), spawnCoordinatesClearDay, Quaternion.Euler(-12, -70f, 48));
                break;
        }

        WeatherConditions ast = wGO.GetComponent<WeatherConditions>();
        return ast;
    }

    // Will detect the minimal distance between the camera and prefabs to activate animations and effects
    void DetectDistance()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);

        switch (weatherCondition)
        {
            case 0:
                if ((distance <= 220) && (!sonido))
                {
                    sonido = true;
                    clearDay.Invoke();
                }
                    break;
            case 1:
                if ((distance <= 240) && (!sonido))
                {
                    sonido = true;
                    rainyDay.Invoke();
                }
                break;
            default://Default: Clear
                if ((distance <= 220) && (!sonido))
                {
                    sonido = true;
                    clearDay.Invoke();
                }
                break;
        }
    }

    // Adds the given listener for the ClearDay event
    public void AddClearDayListener(UnityAction listener)
    {
        clearDay.AddListener(listener);
    }

    // Adds the given listener for the RainDay event
    public void AddRainyDayListener(UnityAction listener)
    {
        rainyDay.AddListener(listener);
    }

}
