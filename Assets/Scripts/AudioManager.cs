using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The audio manager
/// </summary>
public static class AudioManager
{
    static bool initialized = false;
    static AudioSource audioSource;
    static Dictionary<AudioClipName, AudioClip> audioClips =
        new Dictionary<AudioClipName, AudioClip>();

    /// <summary>
    /// Gets whether or not the audio manager has been initialized
    /// </summary>
    public static bool Initialized
    {
        get { return initialized; }
    }

    /// <summary>
    /// Initializes the audio manager
    /// </summary>
    /// <param name="source">audio source</param>
    public static void Initialize(AudioSource source)
    {
        initialized = true;
        audioSource = source;
        audioSource.volume = 0f;
        audioClips.Add(AudioClipName.Clear,
            Resources.Load<AudioClip>("Clear"));
        //audioClips.Add(AudioClipName.Rain,
        //    Resources.Load<AudioClip>("Rain"));
    }

    /// <summary>
    /// Plays the audio clip with the given name
    /// </summary>
    /// <param name="name">name of the audio clip to play</param>
    public static void Play(AudioClipName name, MonoBehaviour justToStartCoroutine)
    {
        audioSource.PlayOneShot(audioClips[name]);
        justToStartCoroutine.StartCoroutine(StartFade(audioSource, 3f, 1f));
        //audioSource.volume = Mathf.Lerp(audioSource.volume, 0f, 1f * Time.deltaTime);
    }

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