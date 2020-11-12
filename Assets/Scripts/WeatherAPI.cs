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
    private Weather3D WeatherMainScript;

    private void Awake()
    {
        WeatherMainScript = gameObject.GetComponent<Weather3D>();
    }

    // Async/await that send a GET request to the Weather API
    public async UniTask<Weather> GetWeather()
    {
        UnityWebRequest req = await UnityWebRequest.Get(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}&units=metric", CityId, API_KEY)).SendWebRequest(); 
        byte[] result = req.downloadHandler.data;
        string weatherJSON = System.Text.Encoding.Default.GetString(result);
        Debug.Log(weatherJSON);
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(weatherJSON);

        return info.toWeather();
    }

}

// Represent OPENWEATHERMAP response body
// https://openweathermap.org/current 
[Serializable]
public class WeatherInfo
{
    public string name;
    public Main main;
    public List<WeatherMain> weather;


    // TODO: Add popup for user
    public Weather toWeather()
    {

        try
        {
            float actualTemperature = main.temp;
            Weather3D.eWeatherState actualConditionId;
            string actualConditionName = weather[0].main;
            string actualCity = name;
            switch (weather[0].main)
            {
                case "Clear":
                    actualConditionId = Weather3D.eWeatherState.clear;
                    break;
                case "Rain":
                    actualConditionId = Weather3D.eWeatherState.rain;
                    break;
                default://Default: Clear
                    actualConditionId = Weather3D.eWeatherState.clear;
                    break;
            }

            return new Weather(actualCity, actualTemperature, actualConditionId, actualConditionName);

        }
        catch (Exception e)
        {
            Debug.LogException(e);
            return new Weather("None", 0, Weather3D.eWeatherState.none, "None");
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
public class Main
{
    public float temp;
}

