using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

public class LerpVisualElementOnVRInteractive : LerpVisualElement
{    

    VRRaycaster vrRaycaster;

    async override protected void Awake()
    {
        base.Awake();
        await new WaitForEndOfFrame();
        vrRaycaster = ReferenceManagerIndependent.Instance.VRRaycaster;        
    }

    async override protected void OnEnable()
    {
        base.OnEnable();
        await new WaitForSeconds(0.2f);
        vrRaycaster.OnOverSomething += OnOver;
        vrRaycaster.OnOutOfEverything += OnOut;
    }

    override protected void OnDisable()
    {
        base.OnDisable();        
        vrRaycaster.OnOverSomething -= OnOver;
        vrRaycaster.OnOutOfEverything -= OnOut;
    }

    void OnOver(VRInteractiveItem interactiveItem)
    {
        HandleOver();
    }

    void OnOut(VRInteractiveItem interactiveItem)
    {
        HandleOut();
    }

}
