using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainDayEffect : MonoBehaviour
{
    public GameObject rainPrefab;

    void Start()
    {
        // add listeners to the event manager
        EventManager.AddRainDayListener(ActivateEffects);
        
    }

    void ActivateEffects()
    {

        //Debug.Log("EFECTO LLUVIA");
        Instantiate<GameObject>(rainPrefab, transform.position, Quaternion.identity);
        SetLightEffect.LightEffect(0.85f, this);
    }
}
