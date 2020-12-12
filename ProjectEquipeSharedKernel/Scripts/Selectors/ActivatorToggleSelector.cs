using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

using UnityEngine.UI;

//Usada para ativar e desativar um objeto a partir do clique nele
public class ActivatorToggleSelector : ClickOrAimSelector
{

    private Toggle toggle;

    public override void HandleClick()
    {

        // pegar toogle
        toggle = transform.GetComponent<Toggle>();
        if(!toggle.isOn)
        {
            toggle.isOn = !toggle.isOn;
        }
        // ativar toogle e desativor o parent no mesmo nível
        //Debug.Log(toggle.isOn);
    }

}

