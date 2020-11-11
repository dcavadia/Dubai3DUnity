using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    static bool initialized = false;
    static AudioSource audioSource;
    static Dictionary<AudioClipName, AudioClip> audioClips =
        new Dictionary<AudioClipName, AudioClip>();

    public static bool Initialized
    {
        get { return initialized; }
    }

    public static void Initialize(AudioSource source)
    {
        initialized = true;
        audioSource = source;
        audioSource.volume = 0f;
        audioClips.Add(AudioClipName.Clear,
            Resources.Load<AudioClip>("Clear"));

    }

    // Plays the audio clip with the given name
    public static void Play(AudioClipName name, MonoBehaviour justToStartCoroutine)
    {
        audioSource.PlayOneShot(audioClips[name]);
        justToStartCoroutine.StartCoroutine(StartFade(audioSource, 3f, 1f));
    }

    public static void Stop()
    {
        audioSource.Stop();
    }

    // Makes audio volumen fade
    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}

public enum AudioClipName
{
    Clear
    //Rain,
    //Snow,
}