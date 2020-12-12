using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;


public abstract class LerpVisualElementOnAim : LerpVisualElement, IAimOver
{

    [Header("VRInteractiveItem reference")]
    [SerializeField] protected VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.

    public VRInteractiveItem InteractiveItem { get => m_InteractiveItem; set => m_InteractiveItem = value; }

    // Use this for initialization
    new protected void Awake () {
        if(m_InteractiveItem == null)
           m_InteractiveItem = GetComponent<VRInteractiveItem>();
        
    }

    override protected void OnEnable()
    {
        RegisterAimEventListener();
        base.OnEnable();
    }


    override protected void OnDisable()
    {
        UnregisterAimEventListener();
        base.OnDisable();
    }

    public void RegisterAimEventListener()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
    }

    public void UnregisterAimEventListener()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
    }
}
