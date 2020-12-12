using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

using TMPro;

//Classe base para fazer scripts para detectar clique ou mouse over em objetos
public abstract class BaseSelector : MonoBehaviour, IInteractive, IFinishable
{
    public event Action<GameObject> OnExecuted;

    protected ReferenceManagerIndependent referenceManagerIndependent;
    protected ReferenceManagerDependent referenceManagerDependent;

    [SerializeField] protected VRInteractiveItem m_InteractiveItem;
        
    public VRInteractiveItem InteractiveItem { get => m_InteractiveItem; set => m_InteractiveItem = value; }

    protected virtual void Awake()
    {
        if (InteractiveItem == null)
            InteractiveItem = GetComponentInChildren<VRInteractiveItem>();
        referenceManagerIndependent = ReferenceManagerIndependent.Instance;
        referenceManagerDependent = ReferenceManagerDependent.Instance;
    }

    protected virtual void Finished(GameObject go)
    {
        //Debug.Log("invoke");
        OnExecuted?.Invoke(go);
    }
}

