using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

using System;

[RequireComponent(typeof(Rigidbody))]
public class GrabSelector : BaseSelector, IGetItemOnHand, IGrababble
{
    
    [Tooltip("Para melhorar legibilidade de formularios em papel")]
    [SerializeField] bool duplicateSizeInVR;
    const float extraSize = 2;
    HandController playerController;
       
    private void Start()
    {
        playerController = ReferenceManagerDependent.Instance.HandsController;
    }

    protected virtual void OnEnable()
    {
        RegisterGrabEventListener();
    }

    protected virtual void OnDisable()
    {
        UnregisterGrabEventListener();
    }

    public void HandleGrab()
    {
        Debug.Log("entrou");
        playerController.SetObjectOnHand(this.gameObject, true);
        DuplicateSizeInVR();
        base.Finished(this.gameObject);
    }

    public void RegisterGrabEventListener()
    {
        InteractiveItem.OnGrab += HandleGrab;
    }

    public void UnregisterGrabEventListener()
    {
        InteractiveItem.OnGrab -= HandleGrab;
    }

    public void DuplicateSizeInVR()
    {
         if(duplicateSizeInVR && 
            ReferenceManagerIndependent.Instance.PlatformManager.CurrentVRPlatform != VRPlataform.PC)
            this.transform.localScale *= extraSize; //para formularios em papel
    }    
}
