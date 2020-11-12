using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weather : MonoBehaviour
{
    //Private Singleton-style instance. Accessed by static property S later in script
    static private Weather _S;
    static private eWeatherState WEATHER_STATE = eWeatherState.none;

    [System.Flags]
    public enum eWeatherState
    {
        // Decimal      // Binary
        none = 0,       // 00000000
        clear = 1,      // 00000001
        rain = 2,       // 00000010
      //rainy = 4,         00000100
    }

    [Header("Set in Inspector")]
    [Tooltip("This sets the WeatherScriptableObject to be used throughout the app.")]
    public WeatherScriptableObject weatherSO;

    [SerializeField]
    [Tooltip("This private field shows the weather state in the Inspector")]
    protected eWeatherState weatherState;

    private WeatherAPI APICall;
    private const float API_CHECK_MAXTIME = 5.0f * 60.0f; //5 minutes
    private float apiCheckCountdown = API_CHECK_MAXTIME;
    private int CounterAPICalls;

    // UI Text
    public Text locationUI;
    public Text conditionUI;
    public Text temperatureUI;


    private void Awake()
    {
        S = this;
        weatherState = eWeatherState.none;
        WEATHER_STATE = weatherState;
        APICall = gameObject.GetComponent<WeatherAPI>();
        CounterAPICalls = 1;
    }


    void Start()
    {
        StartCoroutine(APICall.GetWeather(APICall.SetWeatherStatus));
    }


    void Update()
    {

        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            CounterAPICalls++;
            apiCheckCountdown = API_CHECK_MAXTIME;
            StartCoroutine(APICall.GetWeather(APICall.SetWeatherStatus));
        }

        //Checking changes in weather state thru the inspector
        WeatherStateChanged();
    }


    // Weather change with API Call
    public void SpawnWeatherPrefabAPI(eWeatherState actualWeather, string location, string weatherCondition, string temperature)
    {

        //Temperature is constantly changing
        temperatureUI.text = temperature;

        //First API Call
        if (CounterAPICalls == 1)
        {
            Weather.WEATHER_STATE = actualWeather;
            locationUI.text = location;
            conditionUI.text = weatherCondition;
            S.weatherState = Weather.WEATHER_STATE;
            WeatherSpawner(actualWeather);
        }
        //New call with different weather
        else if (S.weatherState != actualWeather)
        {
            DestroyCloud();
            WeatherSpawner(actualWeather);
        }

    }

    // Weather change by user in inspector
    public void SpawnWeatherPrefabManual(eWeatherState actualWeather, string weatherCondition)
    {
        //Set new condition
        conditionUI.text = weatherCondition;
        DestroyCloud();
        WeatherSpawner(actualWeather);
    }

    public void WeatherStateChanged()
    {
        if (Weather.WEATHER_STATE != S.weatherState)
        {
            switch (S.weatherState)
            {
                case eWeatherState.none:
                    SpawnWeatherPrefabManual(eWeatherState.none, "None");
                    break;
                case eWeatherState.clear:
                    SpawnWeatherPrefabManual(eWeatherState.clear, "Clear");
                    break;
                case eWeatherState.rain:
                    SpawnWeatherPrefabManual(eWeatherState.rain, "Rain");
                    break;
                default://Default: None
                    SpawnWeatherPrefabManual(eWeatherState.none, "None");
                    break;
            }
            Weather.WEATHER_STATE = S.weatherState;
        }
    }

    public void WeatherSpawner(eWeatherState actualWeather)
    {
        switch (actualWeather)
        {
            case eWeatherState.none:
                //No current weather
                break;
            case eWeatherState.clear:
                WeatherConditions.SpawnCondition(0);
                break;
            case eWeatherState.rain:
                WeatherConditions.SpawnCondition(1);
                break;
            default://Default: Clear
                WeatherConditions.SpawnCondition(0); // If WeatherAPI returns any conditions different from clear or rain
                break;
        }
    }

    //Destroy any weather model, sounds, light changes. (including particle effects).
    public void DestroyCloud()
    {
        Destroy(GameObject.FindWithTag("Cloud"));
        Destroy(GameObject.FindWithTag("Rain"));
        SetDimLight.LightEffect(1f, this);
        AudioManager.Stop();
    }


    public string GetWeatherState()
    {
        switch (Weather.WEATHER_STATE)
        {
            case eWeatherState.none:
                return ("None");
            case eWeatherState.clear:
                return ("Clear");
            case eWeatherState.rain:
                return ("Rain");
            default://Default: Clear
                return (conditionUI.text);
        }
    }


    // ---------------- Static Section ---------------- //

    /// <summary>
    /// <para>This static private property provides some protection for the Singleton _S.</para>
    /// <para>get {} does return null, but throws an error first.</para>
    /// <para>set {} allows overwrite of _S by a 2nd instance, but throws an error first.</para>
    /// <para>Another advantage of using a property here is that it allows you to place
    /// a breakpoint in the set clause and then look at the call stack if you fear that 
    /// something random is setting your _S value.</para>
    /// </summary>
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
