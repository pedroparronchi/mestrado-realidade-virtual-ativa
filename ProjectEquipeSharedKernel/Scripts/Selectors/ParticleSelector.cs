using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

//Usada para ativar e desativar uma particular a partir do clique em um objeto
public class ParticleSelector : ClickSelector
{
    private ParticleSystem particle;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        particle.Stop();
    }
    
    public override void HandleClick()
    {
        if (particle.isPlaying)
        {
            particle.Stop();
        }
        else {
            particle.Play();
        }
        
    }
}
