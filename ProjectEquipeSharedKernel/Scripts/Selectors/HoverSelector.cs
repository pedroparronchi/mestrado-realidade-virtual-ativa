using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

using TMPro;


//Classe abstrata para detectar quando o mouse passou por cima de um objeto
public abstract class HoverSelector : BaseSelector
{
    protected void OnEnable()
    {
        InteractiveItem.OnOver += HandleOver;
    }


    protected void OnDisable()
    {
        InteractiveItem.OnOver -= HandleOver;
    }
    
    //QUANDO O BOTAO É ATIVADO, ESSA FUNCAO EH CHAMADA. AQUI VOCE DEVE PROGRAMAR O SEU CODIGO PARA O SEU BOTAO...
    protected abstract void HandleOver();
}

