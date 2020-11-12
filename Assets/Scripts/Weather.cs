using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weather
{

    public string city;
    public float temperature;
    public Weather3D.eWeatherState conditionId;
    public string conditionName;

    public Weather(string city, float temperature, Weather3D.eWeatherState conditionId, string conditionName)
    {
        this.temperature = temperature;
        this.conditionId = conditionId;
        this.city = city;
        this.conditionName = conditionName;
    }
}