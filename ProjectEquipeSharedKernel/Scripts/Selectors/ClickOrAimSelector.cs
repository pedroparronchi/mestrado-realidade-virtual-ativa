using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

using TMPro;

//Classe abstrata para detectar cliques em objetos
public abstract class ClickOrAimSelector : ClickSelector, IAimOver
{

    protected bool gazeIsOnNow = false;

    override protected void OnEnable()
    {
        base.OnEnable();

        gazeIsOnNow = false;

        RegisterAimEventListener();
    }

    override protected void OnDisable()
    {
        base.OnDisable();

        UnregisterAimEventListener();
        gazeIsOnNow = false;
    }

    virtual public void HandleOver()
    {
        if (referenceManagerIndependent.PlatformManager.CurrentVRControlScheme == VRControlScheme.Laser)
            return;
        HandleClick();
    }

    //QUANDO O BOTAO É ATIVADO, ESSA FUNCAO EH CHAMADA. AQUI VOCE DEVE PROGRAMAR O SEU CODIGO PARA O SEU BOTAO...
    override public void HandleClick()
    {
        gazeIsOnNow = true;
    }

    //QUANDO O BOTAO É ATIVADO, ESSA FUNCAO EH CHAMADA. AQUI VOCE DEVE PROGRAMAR O SEU CODIGO PARA O SEU BOTAO...
    override public void HandleUp()
    {
        gazeIsOnNow = false;
    }

    virtual public void HandleOut()
    {
        gazeIsOnNow = false;
    }
    
    public void RegisterAimEventListener()
    {
        InteractiveItem.OnOver += HandleOver;
        InteractiveItem.OnOut += HandleOut;
    }

    public void UnregisterAimEventListener()
    {
        InteractiveItem.OnOver -= HandleOver;
        InteractiveItem.OnOut -= HandleOut;
    }
}

