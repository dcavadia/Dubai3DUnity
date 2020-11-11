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
    private Weather WeatherMainScript;


    private void Awake()
    {
        WeatherMainScript = gameObject.GetComponent<Weather>();
    }


    public IEnumerator GetWeather(Action<WeatherInfo> onSuccess)
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
            onSuccess(info);
        }
    }

    public void SetWeatherStatus(WeatherInfo weatherObj)
    {
        string weatherObjTemp = Mathf.RoundToInt(weatherObj.main.temp) + "°C";


        switch (weatherObj.weather[0].main)
        {
            case "Clear":
                WeatherMainScript.SpawnWeatherPrefabAPI(Weather.eWeatherState.clear, weatherObj.name, weatherObj.weather[0].main, weatherObjTemp);
                break;
            case "Rain":
                WeatherMainScript.SpawnWeatherPrefabAPI(Weather.eWeatherState.rain, weatherObj.name, weatherObj.weather[0].main, weatherObjTemp);
                break;
            default://Default: Clear
                WeatherMainScript.SpawnWeatherPrefabAPI(Weather.eWeatherState.clear, weatherObj.name, weatherObj.weather[0].main, weatherObjTemp);
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
