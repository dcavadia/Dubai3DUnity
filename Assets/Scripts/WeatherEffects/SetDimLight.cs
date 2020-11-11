using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDimLight : MonoBehaviour
{
    private static Light myLight;
    private static float durationTime;

    void Awake()
    {
        myLight = GetComponent<Light>();
        durationTime = 3f;
    }

    static public void LightEffect(float intensity, MonoBehaviour justToStartCoroutine)
    {
        justToStartCoroutine.StartCoroutine(LerpLight(myLight, intensity, durationTime));

    }

    // Dim light with Lerp
    static IEnumerator LerpLight(Light targetLight, float toIntensity, float duration)
    {
        float currentIntensity = targetLight.intensity;
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            targetLight.intensity = Mathf.Lerp(currentIntensity, toIntensity, counter / duration);
            yield return null;
        }
    }

}
