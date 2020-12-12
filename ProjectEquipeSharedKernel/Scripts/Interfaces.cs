using System;
using UnityEngine;
using UnityEngine.UI;
using VRStandardAssets.Utils;

interface IGrababble : IInteractive, IFinishable
{
    void RegisterGrabEventListener();
    void UnregisterGrabEventListener();

    void HandleGrab();
}

interface IClickable : IInteractive, IFinishable
{
    void RegisterClickEventListener();
    void UnregisterClickEventListener();

    void HandleClick();
    void HandleUp();
}

interface IAimOver : IInteractive, IFinishable
{
    void RegisterAimEventListener();
    void UnregisterAimEventListener();

    void HandleOver();
    void HandleOut();
}

interface IInteractive
{
    VRInteractiveItem InteractiveItem { get; set; }
}

interface IFinishable
{
    event Action<GameObject> OnExecuted;
}

interface IOpenable
{
    event Action<GameObject> OnOpen;

    bool GetIsOpen();

    void SetIsOpen(bool value);
}

interface IClosable
{
    event Action<GameObject> OnClose;

    bool GetIsOpen();

    void SetIsOpen(bool value);
}

interface IGetItemOnHand : IFinishable
{
    
}

interface ITimeCounter : IFinishable
{
    float Count { get; set; }

    Image ImgSelection { get; set; }

    float TimeToHold { get; set; }

    void ProcessClick();

    void ProcessStop();
    
    void ProcessUpdate();

    void TurnOffImageSelection();

}
