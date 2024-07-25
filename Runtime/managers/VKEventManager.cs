using System;
using System.Collections.Generic;
using VKBridgeSDK.Runtime.data;

namespace VKBridgeSDK.Runtime.managers
{
    public class VKEventManager
    {
        private readonly Dictionary<VKBridgeEventType, Action<VKPromiseData>> _eventListeners = new Dictionary<VKBridgeEventType, Action<VKPromiseData>>();

        public void AddEventListener(VKBridgeEventType eventType, Action<VKPromiseData> listener)
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

        public void RemoveEventListener(VKBridgeEventType eventType, Action<VKPromiseData> listener)
        {
            if (!_eventListeners.ContainsKey(eventType)) 
                return;
        
            _eventListeners[eventType] -= listener;
            if (_eventListeners[eventType] == null)
            {
                _eventListeners.Remove(eventType);
            }
        }

        public void TriggerEvent(VKBridgeEventType eventType, VKPromiseData vkPromiseData)
        {
            if (_eventListeners.TryGetValue(eventType, out var listener)) listener?.Invoke(vkPromiseData);
        }
    }
}