using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

//Usada para tocar um som a partir do clique em um objeto
public class SoundClickSelector : ClickOrAimSelector
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField] SoundSelector soundSelector;

    // Start is called before the first frame update
    void Start()
    {
        if(!audioSource)
            audioSource = GetComponent<AudioSource>();
    }
    
    public override void HandleClick()
    {
        soundSelector.PlaySound(audioSource);
    }
}
