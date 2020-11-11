using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// An event manager
public static class EventManager
{
    // ClearDay support
    static List<WeatherConditions> clearDayInvokers = new List<WeatherConditions>();
    static List<UnityAction> clearDayListeners =
        new List<UnityAction>();

    // RainDay support
    static List<WeatherConditions> rainDayInvokers = new List<WeatherConditions>();
    static List<UnityAction> rainDayListeners =
        new List<UnityAction>();



    // Adds the given script as a points added invoker
    public static void AddClearDayInvoker(WeatherConditions invoker)
    {
        // add invoker to list and add all listeners to invoker
        clearDayInvokers.Add(invoker);
        foreach (UnityAction listener in clearDayListeners)
        {
            invoker.AddClearDayListener(listener);
        }
    }

    // Adds the given method as a points added listener
    public static void AddClearDayListener(UnityAction listener)
    {
        // add listener to list and to all invokers
        clearDayListeners.Add(listener);
        foreach (WeatherConditions invoker in clearDayInvokers)
        {
            invoker.AddClearDayListener(listener);
        }
    }



    // Adds the given script as a points added invoker
    public static void AddRainDayInvoker(WeatherConditions invoker)
    {
        // add invoker to list and add all listeners to invoker
        rainDayInvokers.Add(invoker);
        foreach (UnityAction listener in rainDayListeners)
        {
            invoker.AddRainDayListener(listener);
        }
    }



    // Adds the given method as a points added listener
    public static void AddRainDayListener(UnityAction listener)
    {
        // add listener to list and to all invokers
        rainDayListeners.Add(listener);
        foreach (WeatherConditions invoker in rainDayInvokers)
        {
            invoker.AddRainDayListener(listener);
        }
    }
}
