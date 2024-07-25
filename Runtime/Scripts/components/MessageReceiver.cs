using System;
using UnityEngine;

public class MessageReceiver : MonoBehaviour
{
    public static ResponseManager responseManager;

    public void ReceivePromise(string jsonData)
    {
        responseManager.HandlePromiseResponse(jsonData);
    }

    public void ReceiveError(string jsonData)
    {
        responseManager.HandleErrorResponse(jsonData);
    }

    public void ReceiveEvent(string jsonData)
    {
        var eventObject = JsonUtility.FromJson<VKData>(jsonData);
        if (Enum.TryParse(eventObject.method, out VKBridgeEventType eventType))
        {
            VkBridgeFacade.TriggerEvent(eventType, eventObject.data);
        }
    }

    public void OnFocus()
    {
        Debug.Log("Browser window gained focus");
    }

    public void OnBlur()
    {
        Debug.Log("Browser window lost focus");
    }
}