using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;


//Usada para trocar de skybox a partir do clique em um objeto
public class SkyboxSelector : ClickSelector
{
    public Material newSkybox;
    Material oldSkybox;
    bool isOn = false;

    void Start()
    {
        oldSkybox = RenderSettings.skybox;
    }

    public override void HandleClick()
    {
        isOn = !isOn;
        RenderSettings.skybox = isOn ? newSkybox : oldSkybox;
    }

    public void SetTargetName(string s)
    {
        
    }
}

