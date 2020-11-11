using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDayEffect : MonoBehaviour
{
    private Animation myAnimationComponent;
    public AnimationClip anim;

    void Start()
    {
        
        myAnimationComponent = this.GetComponent<Animation>();
        myAnimationComponent.clip = anim;

        // add listeners to the event manager
        EventManager.AddClearDayListener(ActivateEffects);

    }

    void ActivateEffects()
    {

        AudioManager.Play(AudioClipName.Clear, this);
        myAnimationComponent.Play();
        SetLightEffect.LightEffect(1.1f, this);
    }
}
