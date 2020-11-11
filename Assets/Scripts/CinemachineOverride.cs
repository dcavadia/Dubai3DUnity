using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

// Makes cinemachine transition smoother
public class CinemachineOverride : MonoBehaviour
{

    private CinemachineFreeLook freeLookCam;
    private float duration;
    private float startTime;

    void Awake()
    {
        freeLookCam = GetComponent<CinemachineFreeLook>();
        startTime = Time.time;
        duration = 30.0f;
    }

    void Update()
    {
        float t = (Time.time - startTime) / duration;
        freeLookCam.m_YAxis.Value = Mathf.SmoothStep(freeLookCam.m_YAxis.Value, 0, t); //SmoothStep is similar to Lerp function but smoother at the beginning and at the end
    }
}
