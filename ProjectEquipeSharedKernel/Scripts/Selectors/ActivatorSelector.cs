using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;


//Usada para ativar e desativar um objeto a partir do clique nele
public class ActivatorSelector : ClickOrAimSelector
{
    public GameObject target;
    [Tooltip("Use to dinamically find target")]
    public string targetName;
    [Tooltip("True to activate and false do deactivate target")]
    public bool willActivate;
    [Tooltip("True to enable undo")]
    public bool rollBack = false;
    bool timeToRollback = false;

    public float delayToProcess = 0;

    void Start()
    {
        if (target == null)
            target = GameObject.Find(targetName);
    }

    //QUANDO O BOTAO É ATIVADO, ESSA FUNCAO EH CHAMADA. AQUI VOCE DEVE PROGRAMAR O SEU CODIGO PARA O SEU BOTAO...
    async public override void HandleClick()
    {
        base.HandleClick();
        if (delayToProcess > 0)
            await new WaitForSeconds(delayToProcess);
        target.SetActive(rollBack ? (timeToRollback ? !willActivate : willActivate) : willActivate);
        timeToRollback = !timeToRollback;
    }
}

