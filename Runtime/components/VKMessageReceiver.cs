using System;
using JetBrains.Annotations;
using UnityEngine;
using VKBridgeSDK.Runtime.data;
using VKBridgeSDK.Runtime.managers;

namespace VKBridgeSDK.Runtime.components
{
    public class VKMessageReceiver : MonoBehaviour
    {
        private VKResponseManager _vkResponseManager;
        private VKEventManager _eventManager;

        public void Initialize(VKResponseManager responseManager, VKEventManager eventManager)
        {
            _vkResponseManager = responseManager;
            _eventManager = eventManager;
        }

        [UsedImplicitly]
        public void ReceivePromise(string jsonData)
        {
            _vkResponseManager.HandlePromiseResponse(jsonData);
        }

        [UsedImplicitly]
        public void ReceiveError(string jsonData)
        {
            _vkResponseManager.HandleErrorResponse(jsonData);
        }

        [UsedImplicitly]
        public void ReceiveEvent(string jsonData)
        {
            var eventObject = JsonUtility.FromJson<VKEvent>(jsonData);
            if (Enum.TryParse(eventObject.detail.type, out VKBridgeEventType eventType))
            {
                _eventManager.TriggerEvent(eventType, eventObject.detail.data);
            }
        }

        [UsedImplicitly]
        public void OnFocus()
        {
            Debug.Log("Browser window gained focus");
            _eventManager.TriggerEvent(VKBridgeEventType.FocusChanged, new VKEventData { result = true });
        }

        [UsedImplicitly]
        public void OnBlur()
        {
            Debug.Log("Browser window lost focus");
            _eventManager.TriggerEvent(VKBridgeEventType.FocusChanged, new VKEventData { result = false });
        }
    }
}