using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MeteorologicalConditions : MonoBehaviour
{
    static Vector3 spawnCoordinates = new Vector3(90, 360, 476);
    
    public bool sonido = false;

    static Vector3 spawnCoordinates2 = new Vector3(90, 375, 490);
    //static Vector3 spawnCoordinates3 = new Vector3(300, 600, 637);
    //static Vector3 spawnCoordinates4 = new Vector3(160, 600, 637);
    //static Vector3 spawnCoordinates5 = new Vector3(100, 600, 637);

    static int weatherCondition;

    ClearDay clearDay;
    RainDay rainDay;

    // Start is called before the first frame update

    void Start()
    {

        rainDay = new RainDay();
        EventManager.AddRainDayInvoker(this);

        clearDay = new ClearDay();
        EventManager.AddClearDayInvoker(this);

        
    }

    // Update is called once per frame
    void Update()
    {
        DetectDistance();
    }

    static public MeteorologicalConditions SpawnCondition(int ndx)
    {
        GameObject wGO; // Weather Game Object
        weatherCondition = ndx;
        switch (weatherCondition)
        {
            case 0:
                wGO = Instantiate<GameObject>(Weather.WeatherSO.GetWeatherPrefab(ndx), spawnCoordinates, Quaternion.Euler(-15, -70, 48));
                break;
            case 1:
                Debug.Log("LLUVIA");
                wGO = Instantiate<GameObject>(Weather.WeatherSO.GetWeatherPrefab(ndx), spawnCoordinates2, Quaternion.Euler(0, 0, 5));
                break;
            default:
                wGO = Instantiate<GameObject>(Weather.WeatherSO.GetWeatherPrefab(ndx), spawnCoordinates, Quaternion.Euler(-12, -70f, 48));
                break;
        }

        MeteorologicalConditions ast = wGO.GetComponent<MeteorologicalConditions>();
        return ast;
    }

    void DetectDistance()
    {
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);


        //Debug.Log(distance);

        //195
        switch (weatherCondition)
        {
            case 0:
                if ((distance <= 220) && (!sonido))
                {
                    Debug.Log(distance);
                    Debug.Log("SOOLLL");
                    sonido = true;
                    clearDay.Invoke();
                }
                    break;
            case 1:
                if ((distance <= 240) && (!sonido))
                {
                    Debug.Log(distance);
                    Debug.Log("LLUVIAA");
                    sonido = true;
                    rainDay.Invoke();
                }
                break;
            default:
                if ((distance <= 195) && (!sonido))
                {
                    sonido = true;
                    clearDay.Invoke();
                }
                break;
        }

        /*if ((distance <= 230) && (!sonido))
        {
            Debug.Log(distance);
            Debug.Log("SONIDO!!!!!!!!!!!!!!!!!!!");
            sonido = true;
            rainDay.Invoke();
            //AudioManager.Play(AudioClipName.Clear);
        }*/
    }

    // Adds the given listener for the ClearDay event
    public void AddClearDayListener(UnityAction listener)
    {
        clearDay.AddListener(listener);
    }

    // Adds the given listener for the RainDay event
    public void AddRainDayListener(UnityAction listener)
    {
        rainDay.AddListener(listener);
    }

}
