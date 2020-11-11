using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class WeatherAPI : MonoBehaviour
{

    private const string API_KEY = "1bd65cb959ab6e20f614c0ca46711fac";
    private const string CityId = "292223"; //Dubai

    private const float API_CHECK_MAXTIME = 5.0f * 60.0f; //5 minutes
    private float apiCheckCountdown = API_CHECK_MAXTIME;

    public Text locationUI;
    public Text conditionUI;
    public Text temperatureUI;


    IEnumerator GetWeather(Action<WeatherInfo> onSuccess)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}&units=metric", CityId, API_KEY)))
        {
            yield return req.Send();
            while (!req.isDone)
                yield return null;
            byte[] result = req.downloadHandler.data;
            string weatherJSON = System.Text.Encoding.Default.GetString(result);
            Debug.Log(weatherJSON);
            WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(weatherJSON);
            //Debug.Log(info);
            onSuccess(info);
        }
    }

    void Start()
    {
        StartCoroutine(GetWeather(SetWeatherStatus));
    }

    void Update()
    {
        apiCheckCountdown -= Time.deltaTime;
        if (apiCheckCountdown <= 0)
        {
            apiCheckCountdown = API_CHECK_MAXTIME;
            StartCoroutine(GetWeather(SetWeatherStatus));
        }
    }

    public void SetWeatherStatus(WeatherInfo weatherObj)
    {
        string weatherObjTemp = Mathf.RoundToInt(weatherObj.main.temp) + "°C";

        locationUI.text = weatherObj.name;
        conditionUI.text = weatherObj.weather[0].main;
        temperatureUI.text = weatherObjTemp;


        switch (weatherObj.weather[0].main)
        {
            case "Clear":
                Weather.SpawnWeatherPrefab(Weather.eWeatherState.clear);
                MeteorologicalConditions.SpawnCondition(1);
                break;
            case "Rain":
                Weather.SpawnWeatherPrefab(Weather.eWeatherState.rain);
                MeteorologicalConditions.SpawnCondition(1);
                break;
            default://Default: Clear
                Weather.SpawnWeatherPrefab(Weather.eWeatherState.clear);
                MeteorologicalConditions.SpawnCondition(1);
                break;
        }

    }

}

[Serializable]
public class WeatherMain
{
    public int id;
    public string main;
}
[Serializable]
public class WeatherInfo
{
    public int id;
    public string name;
    public main main;
    public List<WeatherMain> weather;
}
[Serializable]
public class main
{
    public float temp;
}
