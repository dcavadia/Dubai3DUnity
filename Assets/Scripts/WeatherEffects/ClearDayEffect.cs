using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDayEffect : MonoBehaviour
{
    [Header("Set in Inspector")]
    public AnimationClip anim;
    private Animation myAnimationComponent;

    void Awake()
    { 
        myAnimationComponent = this.GetComponent<Animation>();
        myAnimationComponent.clip = anim;

        // Add event listener
        EventManager.AddClearDayListener(ActivateEffects);
    }

    void ActivateEffects()
    {
        AudioManager.Play(AudioClipName.Clear, this);
        myAnimationComponent.Play();
        SetDimLight.LightEffect(1.1f, this);
    }
}
