using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class WeatherAPI : MonoBehaviour
{

    private const string API_KEY = "1bd65cb959ab6e20f614c0ca46711fac";
    private const string CityId = "292223"; //Dubai
    private Weather WeatherMainScript;

    private void Awake()
    {
        WeatherMainScript = gameObject.GetComponent<Weather>();
    }

    // Async/await that send a GET request to the Weather API
    public async UniTask<WeatherGet> GetWeather()
    {
        UnityWebRequest req = await UnityWebRequest.Get(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}&units=metric", CityId, API_KEY)).SendWebRequest(); 
        byte[] result = req.downloadHandler.data;
        string weatherJSON = System.Text.Encoding.Default.GetString(result);
        Debug.Log(weatherJSON);
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(weatherJSON);

        return info.toWeatherGet();
    }

}

public class WeatherGet
{

    public string city;
    public float temperature;
    public Weather.eWeatherState conditionId;
    public string conditionName;

    public WeatherGet(string city, float temperature, Weather.eWeatherState conditionId, string conditionName)
    {
        this.temperature = temperature;
        this.conditionId = conditionId;
        this.city = city;
        this.conditionName = conditionName;
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


    // TODO: Add popup for user
    public WeatherGet toWeatherGet()
    {

        try
        {
            float actualTemperature = main.temp;
            Weather.eWeatherState actualConditionId;
            string actualConditionName = weather[0].main;
            string actualCity = name;
            switch (weather[0].main)
            {
                case "Clear":
                    actualConditionId = Weather.eWeatherState.clear;
                    break;
                case "Rain":
                    actualConditionId = Weather.eWeatherState.rain;
                    break;
                default://Default: Clear
                    actualConditionId = Weather.eWeatherState.clear;
                    break;
            }

            return new WeatherGet(actualCity, actualTemperature, actualConditionId, actualConditionName);

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return new WeatherGet("None", 0, Weather.eWeatherState.none, "None");
        }


    }

}
[Serializable]
public class main
{
    public float temp;
}
