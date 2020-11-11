using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather : MonoBehaviour
{
    // Private Singleton-style instance. Accessed by static property S later in script
    static private Weather _S;
    static private eWeatherState WEATHER_STATE = eWeatherState.none;

    [System.Flags]
    public enum eWeatherState
    {
        // Decimal      // Binary
        none = 0,       // 00000000
        clear = 1,      // 00000001
        rain = 2,     // 00000010
      //rainy = 4,         00000100
    }

    [Header("Set in Inspector")]
    [Tooltip("This sets the WeatherScriptableObject to be used throughout the app.")]
    public WeatherScriptableObject weatherSO;

    [SerializeField]
    [Tooltip("This private field shows the weather state in the Inspector")]
    protected eWeatherState weatherState;

    private void Awake()
    {
        S = this;
        weatherState = eWeatherState.none;
        WEATHER_STATE = weatherState;
    }


    void Start()
    {
        //MeteorologicalConditions cond = MeteorologicalConditions.SpawnCondition();
    }


    void Update()
    {
        
    }

    public static void SpawnWeatherPrefab(eWeatherState actualWeather)
    {
        Weather.WEATHER_STATE = actualWeather;
        WeatherStateChanged();

        //GameObject wGO = Instantiate<GameObject>(Weather.WeatherSO.GetWeatherPrefab());
    }

    static void WeatherStateChanged()
    {
        S.weatherState = Weather.WEATHER_STATE;
        //this.weatherState = Weather.WEATHER_STATE;
    }

    // ---------------- Static Section ---------------- //

    //This static private property provides some protection for the Singleton _S
    static private Weather S
    {
        get
        {
            if (_S == null)
            {
                Debug.LogError("Weather:S getter - Attempt to get value of S before it has been set.");
                return null;
            }
            return _S;
        }
        set
        {
            if (_S != null)
            {
                Debug.LogError("Weather:S setter - Attempt to set S when it has already been set.");
            }
            _S = value;
        }
    }

    static public WeatherScriptableObject WeatherSO
    {
        get
        {
            if (S != null)
            {
                return S.weatherSO;
            }
            return null;
        }
    }


}
