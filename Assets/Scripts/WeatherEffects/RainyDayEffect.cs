using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainyDayEffect : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject rainPrefab;

    void Awake()
    {
        // Add event listener
        EventManager.AddRainyDayListener(ActivateEffects);
    }

    void ActivateEffects()
    {
        Instantiate<GameObject>(rainPrefab, transform.position, Quaternion.identity);
        SetDimLight.LightEffect(0.85f, this);
    }
}
