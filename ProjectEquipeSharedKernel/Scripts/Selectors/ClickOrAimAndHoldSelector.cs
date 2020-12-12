using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

using TMPro;

//Classe abstrata para detectar cliques em objetos
public abstract class ClickOrAimAndHoldSelector : ClickSelector, IAimOver, ITimeCounter
{
    [SerializeField] float timeToHold = 0.001f; //o tempo necessario para segurar o botao para concluir a ação (0 seria instantaneo)

    [SerializeField] bool cumulative = false;
    float count;

    private Image imgSelection;
    protected bool gazeIsOnNow = false;

    public float Count { get => count; set => count = value; }
    public Image ImgSelection { get => imgSelection; set => imgSelection = value; }
    public float TimeToHold { get => timeToHold; set => timeToHold = value; }

    protected virtual void Start()
    {
        ImgSelection = ReferenceManagerIndependent.Instance.SelectionRadialSlider;
    }

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

        TurnOffImageSelection();
    }

    protected virtual void Update()
    {
        ProcessUpdate();
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
        ProcessClick();
    }

    //QUANDO O BOTAO É ATIVADO, ESSA FUNCAO EH CHAMADA. AQUI VOCE DEVE PROGRAMAR O SEU CODIGO PARA O SEU BOTAO...
    override public void HandleUp()
    {
        HandleOut();
    }

    virtual public void HandleOut()
    {
        gazeIsOnNow = false;
        ProcessStop();
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

    public void ProcessClick()
    {
        count = Time.deltaTime + (cumulative ? count : 0);
    }

    public void ProcessStop()
    {
        if(!cumulative)
            count = 0;
        TurnOffImageSelection();
    }

    public void ProcessUpdate()
    {
        if (count > 0)
        {
            imgSelection.gameObject.SetActive(true);
            imgSelection.fillAmount = count / timeToHold;
        }

        if (count >= timeToHold)
        {
            count = 0;
            Finished(this.gameObject);
        }

        if (!gazeIsOnNow)
            return;

        // Setup the radial to have no fill at the start and hide if necessary.
        if (count > 0)
        {
            count += Time.deltaTime;
            imgSelection.gameObject.SetActive(true);
            imgSelection.fillAmount = count / timeToHold;
        }        
    }

    public void TurnOffImageSelection()
    {
        if (imgSelection && imgSelection.gameObject)
        {
            imgSelection.gameObject.SetActive(false);
            imgSelection.fillAmount = 0;
        }
    }

    public abstract float ReturnNormalizedTime();
}

