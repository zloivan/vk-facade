using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using vk_facade.Runtime.data;
using vk_facade.Runtime.helpers;

namespace vk_facade.Runtime.managers
{
    public class VKEventManager
    {
        private readonly Dictionary<VKBridgeEventType, Action<VKEventData>> _eventListeners =
            new Dictionary<VKBridgeEventType, Action<VKEventData>>();

        private readonly ILogger _logger = new VKBridgeLogger();
        
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

        public void AddEventListener(VKBridgeEventType eventType, Action<VKEventData> listener)
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

        public void RemoveEventListener(VKBridgeEventType eventType, Action<VKEventData> listener)
        {
            if (!_eventListeners.ContainsKey(eventType))
                return;

            _eventListeners[eventType] -= listener;
            if (_eventListeners[eventType] == null)
            {
                _eventListeners.Remove(eventType);
            }
        }

        public void TriggerEvent(VKBridgeEventType eventType, VKEventData vkPromiseData)
        {
            _logger.Log("EVENT_MANAGER",$"TriggerEvent called for {eventType}: [{vkPromiseData}]...");
            
            if (_eventListeners.TryGetValue(eventType, out var listener))
                listener?.Invoke(vkPromiseData);
        }
    }
}