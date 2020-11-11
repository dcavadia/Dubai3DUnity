using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLightEffect : MonoBehaviour
{
    static Light myLight;
    static float startTime;
    static float dimLightTime = 3f;
    public Light point_light;
    static float intensity1;

    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
    }

    static public void LightEffect(float intensity, MonoBehaviour justToStartCoroutine)
    {
        //myLight.intensity = Mathf.PingPong(Time.time, 8)
        //startTime = Time.deltaTime;
        /*while (myLight.intensity < intensity)
        {
            myLight.intensity = Mathf.Lerp(1f, intensity, Time.deltaTime * 1f);
        }*/
        Debug.Log("LUZ");
        //Debug.Log(Time.deltaTime);
        justToStartCoroutine.StartCoroutine(LerpLightRepeat(intensity));
        //myLight.intensity = intensity;

    }

    static IEnumerator LerpLightRepeat(float intensity)
    {
        while (true)
        {
            //Lerp to intensity1
            yield return LerpLight(myLight, intensity, 3f);
            //Lerp to intensity2
            //yield return LerpLight(point_light, intensity2, 2f);
        }
    }

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
