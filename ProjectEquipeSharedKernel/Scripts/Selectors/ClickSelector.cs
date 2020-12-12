using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

using TMPro;

//Classe abstrata para detectar cliques em objetos
public abstract class ClickSelector : BaseSelector, IClickable
{
    protected virtual void OnEnable()
    {
        RegisterClickEventListener();
        
    }
    
    protected virtual void OnDisable()
    {
        UnregisterClickEventListener();

    }
    
    public void RegisterClickEventListener()
    {       
        InteractiveItem.OnClick += HandleClick;
        InteractiveItem.OnUp += HandleUp;        
    }

    public void UnregisterClickEventListener()
    {
        InteractiveItem.OnClick -= HandleClick;
        InteractiveItem.OnUp -= HandleUp;
    }

    abstract public void HandleClick();

    virtual public void HandleUp() { }
}

