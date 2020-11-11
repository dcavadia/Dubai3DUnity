using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherAudioSource : MonoBehaviour
{

    void Awake()
    {
        // Make sure we only have one of this game object
        if (!AudioManager.Initialized)
        {
            // Initialize audio manager and persist audio source across scenes
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            AudioManager.Initialize(audioSource);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Duplicate game object, so destroy
            Destroy(gameObject);
        }
    }
}
