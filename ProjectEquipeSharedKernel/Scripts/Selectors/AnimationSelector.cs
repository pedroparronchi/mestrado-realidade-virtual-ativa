using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

//
//Usada para ativar e desativar uma animacao a partir do clique em um objeto
[RequireComponent(typeof(Animation))]
public class AnimationSelector : ClickOrAimSelector
{
    private Animation anim;
    string animationName;
    bool isOn = false;

    public bool IsOn(){ return isOn; }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        animationName = anim.clip.name;
        foreach (AnimationState state in anim)
        {
            animationName = state.name;
        }
    }

    public override void HandleClick()
    {
        base.HandleClick();
        if(isOn)
        {
            isOn = !isOn;
            anim[animationName].speed = -1;
            anim[animationName].time = anim.isPlaying ? anim[animationName].time : anim[animationName].length;
            anim.Play(animationName);
        }
        else
        {
            isOn = !isOn;
            anim[animationName].speed = 1;
            anim.Play();
        }
    }
}
