using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

public static class VkBridgeFacade
{
    private static ResponseManager _responseManager;
    private static GameObject _messageReceiverObject;
    private static MessageReceiver _messageReceiver;
    private static readonly Dictionary<VKBridgeEventType, Action<string>> _eventListeners = new Dictionary<VKBridgeEventType, Action<string>>();

    static VkBridgeFacade()
    {
        Initialize();
    }

    private static void Initialize()
    {
        _responseManager = new ResponseManager();
        _messageReceiverObject = new GameObject("MessageReceiver");
        _messageReceiver = _messageReceiverObject.AddComponent<MessageReceiver>();
        MessageReceiver.responseManager = _responseManager;
        Object.DontDestroyOnLoad(_messageReceiverObject);
    }

    public static async UniTask<VKData> GetUserInfo()
    {
        return await _responseManager.CallVkMethodAsync("VKWebAppGetUserInfo");
    }

    public static async UniTask<VKData> CallAPIMethod(string methodName, string parameters)
    {
        return await _responseManager.CallVkMethodAsync("VKWebAppCallAPIMethod", new { method = methodName, @params = parameters });
    }

    public static void ShowAlert(string message)
    {
        _responseManager.ShowAlert(message);
    }
    
    public static void AddEventListener(VKBridgeEventType eventType, Action<string> listener)
    {
        if (!_eventListeners.ContainsKey(eventType))
        {
            _eventListeners[eventType] = listener;
        }
        else
        {
            _eventListeners[eventType] += listener;
        }
    }

    public static void RemoveEventListener(VKBridgeEventType eventType, Action<string> listener)
    {
        if (_eventListeners.ContainsKey(eventType))
        {
            _eventListeners[eventType] -= listener;
            if (_eventListeners[eventType] == null)
            {
                _eventListeners.Remove(eventType);
            }
        }
    }

    public static void TriggerEvent(VKBridgeEventType eventType, string data)
    {
        if (_eventListeners.ContainsKey(eventType))
        {
            _eventListeners[eventType]?.Invoke(data);
        }
    }

    public static void Reset()
    {
        if (_messageReceiverObject != null)
        {
            Object.Destroy(_messageReceiverObject);
        }
        _responseManager = null;
        _messageReceiverObject = null;
        _messageReceiver = null;
        _eventListeners.Clear();
    }
}