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

    // Coroutine that send a GET request to the Weather API
    public async UniTask<WeatherInfo> GetWeather()
    {
        UnityWebRequest req = await UnityWebRequest.Get(String.Format("http://api.openweathermap.org/data/2.5/weather?id={0}&APPID={1}&units=metric", CityId, API_KEY)).SendWebRequest(); 
        byte[] result = req.downloadHandler.data;
        string weatherJSON = System.Text.Encoding.Default.GetString(result);
        Debug.Log(weatherJSON);
        WeatherInfo info = JsonUtility.FromJson<WeatherInfo>(weatherJSON);
        return info;
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
