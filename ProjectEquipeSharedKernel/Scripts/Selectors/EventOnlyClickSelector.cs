using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;


//Usada para ativar e desativar um objeto a partir do clique nele
public class EventOnlyClickSelector : ClickOrAimSelector
{
    async public override void HandleClick()
    {
        Finished(this.gameObject);
    }
    
}

