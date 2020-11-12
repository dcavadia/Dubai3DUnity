using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class Weather : MonoBehaviour
{
    //Private Singleton-style instance. Accessed by static property S later in script
    static private Weather _S;
    static private eWeatherState WEATHER_STATE = eWeatherState.none;

    [System.Flags]
    public enum eWeatherState
    {      
        none = 0,       
        clear = 1,      
        rain = 2,       
      //haze = 3,        
    }

    [Header("Set in Inspector")]
    [Tooltip("This sets the WeatherScriptableObject to be used throughout the app.")]
    public WeatherScriptableObject weatherSO;

    [SerializeField]
    [Tooltip("This private field shows the weather state in the Inspector")]
    protected eWeatherState weatherState;

    private WeatherAPI WeatherAPI;
    private const float API_CHECK_MAXTIME = 5.0f * 60.0f; //5 minutes
    private float apiCheckCountdown = API_CHECK_MAXTIME;
    private int CounterAPICalls;
    private Weather.eWeatherState currentWeather;

    // UI Text
    public Text locationUI;
    public Text conditionUI;
    public Text temperatureUI;

    // Supported UI condition
    private const string NONE_CONDITION_UI = "None";
    private const string CLEAR_CONDITION_UI = "Clear";
    private const string RAIN_CONDITION_UI = "Rain";

    // Supported prefabs tags
    private const string CLOUD_PREFAB_TAG = "Cloud";
    private const string RAIN_PREFAB_TAG = "Rain";

    private void Awake()
    {
        S = this;
        weatherState = eWeatherState.none;
        WEATHER_STATE = weatherState;
        WeatherAPI = gameObject.GetComponent<WeatherAPI>();
        CounterAPICalls = 1;
    }


    async void Start()
    {
        await UpdateWeather();
    }


    void Update()
    {

        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            CounterAPICalls++;
            apiCheckCountdown = API_CHECK_MAXTIME;
            UpdateWeather();
        }

        //Checking changes in weather state thru the inspector
        WeatherStateChanged();
    }

    private async UniTask UpdateWeather()
    {
        WeatherGet weatherInfoTask = await WeatherAPI.GetWeather();
        SetWeatherStatus(weatherInfoTask);
    }

    public void SetWeatherStatus(WeatherGet weatherObj)
    {
        string weatherObjTemp = Mathf.RoundToInt(weatherObj.temperature) + "°C";
        SpawnWeatherPrefabAPI(weatherObj.conditionId, weatherObj.city, weatherObj.conditionName, weatherObjTemp);
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
            DestroyWeather();
            WeatherSpawner(actualWeather);
        }

    }

    // Weather change by user in inspector
    private void SpawnWeatherPrefabManual(eWeatherState actualWeather, string weatherCondition)
    {
        //Set new condition
        conditionUI.text = weatherCondition;
        DestroyWeather();
        WeatherSpawner(actualWeather);
    }

    private void WeatherStateChanged()
    {

        string UIManualCondition;

        if (Weather.WEATHER_STATE != S.weatherState)
        {
            switch (S.weatherState)
            {
                case eWeatherState.none:
                    UIManualCondition = NONE_CONDITION_UI;
                    break;
                case eWeatherState.clear:
                    UIManualCondition = CLEAR_CONDITION_UI;
                    break;
                case eWeatherState.rain:
                    UIManualCondition = RAIN_CONDITION_UI;
                    break;
                default://Default: None
                    UIManualCondition = NONE_CONDITION_UI;
                    break;
            }
            SpawnWeatherPrefabManual(S.weatherState, UIManualCondition);


            Weather.WEATHER_STATE = S.weatherState;
        }
    }

    // Spawn prefab of given weather
    private void WeatherSpawner(eWeatherState actualWeather)
    {
        const int CLEAR_DAY_PREFAB = 0;
        const int RAINY_DAY_PREFAB = 1;
 
        switch (actualWeather)
        {
            case eWeatherState.none:
                //No current weather
                break;
            case eWeatherState.clear:
                WeatherConditions.SpawnCondition(CLEAR_DAY_PREFAB);
                break;
            case eWeatherState.rain:
                WeatherConditions.SpawnCondition(RAINY_DAY_PREFAB);
                break;
            default://Default: Clear
                WeatherConditions.SpawnCondition(CLEAR_DAY_PREFAB); // If WeatherAPI returns any conditions different from clear or rain
                break;
        }
    }

    // Destroy any weather model, sounds, light changes. (including particle effects).
    private void DestroyWeather()
    {
        Destroy(GameObject.FindWithTag(CLOUD_PREFAB_TAG));
        Destroy(GameObject.FindWithTag(RAIN_PREFAB_TAG));
        SetDimLight.LightEffect(1f, this);


        AudioManager.Stop();
    }

    // Getter of WEATHER_STATE
    // Since WEATHER_STATE is a enum, a mapped string is returned instead
    public string GetWeatherState()
    {
        switch (Weather.WEATHER_STATE)
        {
            case eWeatherState.none:
                return NONE_CONDITION_UI;
            case eWeatherState.clear:
                return CLEAR_CONDITION_UI;
            case eWeatherState.rain:
                return RAIN_CONDITION_UI;
            default://Default: Clear
                return conditionUI.text;
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
