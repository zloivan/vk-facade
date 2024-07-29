using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using VKBridgeSDK.Runtime.data;

namespace VKBridgeSDK.Runtime.managers
{
    public class VKEventManager
    {
        private readonly Dictionary<VKBridgeEventType, Action<VKPromiseDataOLD>> _eventListeners =
            new Dictionary<VKBridgeEventType, Action<VKPromiseDataOLD>>();

        [DllImport("__Internal")]
        private static extern void UnityVKBridge_Subscribe();

        public VKEventManager()
        {
            SubscribeToVkBridgeEvents();
        }

        private static void SubscribeToVkBridgeEvents()
        {
            if (!Application.isEditor)
            {
                UnityVKBridge_Subscribe();
            }
        }

        public void AddEventListener(VKBridgeEventType eventType, Action<VKPromiseDataOLD> listener)
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

        public void RemoveEventListener(VKBridgeEventType eventType, Action<VKPromiseDataOLD> listener)
        {
            if (!_eventListeners.ContainsKey(eventType))
                return;

            _eventListeners[eventType] -= listener;
            if (_eventListeners[eventType] == null)
            {
                _eventListeners.Remove(eventType);
            }
        }

        public void TriggerEvent(VKBridgeEventType eventType, VKPromiseDataOLD vkPromiseData)
        {
            if (_eventListeners.TryGetValue(eventType, out var listener)) listener?.Invoke(vkPromiseData);
        }
    }
}