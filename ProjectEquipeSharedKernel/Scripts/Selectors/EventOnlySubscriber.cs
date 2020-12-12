using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

using System.Reflection;
using UnityEngine.EventSystems;

using System.ComponentModel;
using UnityEngine.Events;

//Usada para ativar e desativar um objeto a partir do clique nele
public class EventOnlySubscriber : MonoBehaviour
{
    public UnityEngine.Component componentToSubscribe;

    public string methodName;

    public bool gameObjectAsParameter = true;

    public EventTrigger.TriggerEvent OnEventTriggerCallback;
       
    void Start()
    {
        var type = componentToSubscribe.GetType();
        if ( type.GetEvent(methodName) != null)
        {
            EventInfo eventInfo = type.GetEvent(methodName, BindingFlags.Public | BindingFlags.Instance);
            Type objectEventHandlerType = eventInfo.EventHandlerType;
            string eventReceiverName = gameObjectAsParameter ? "OnEventTriggeredWithGo" : "OnEventTriggered";
            MethodInfo mi = this.GetType().GetMethod(eventReceiverName, BindingFlags.Public | BindingFlags.Instance);
            Delegate del = Delegate.CreateDelegate(objectEventHandlerType, this, mi, false);
            eventInfo.AddEventHandler(componentToSubscribe, del);
            if(this.gameObject.name == "PanelViewPlus Proc1 Etapas 17")
                Debug.LogWarning("cadastrando no evento " + methodName);
        }
    }

    public void OnEventTriggeredWithGo(GameObject go)
    {
        //BaseEventData eventData = new BaseEventData(EventSystem.current);
        //eventData.selectedObject = this.gameObject;
        //OnEventTriggerCallback.Invoke(eventData);
        OnEventTriggerCallback.Invoke(null);
    }

    public void OnEventTriggered()
    {
        OnEventTriggerCallback.Invoke(null);
    }


}
