using System;
using FloorIsLava.VKBridgeSDK.helpers;
using JetBrains.Annotations;
using UnityEngine;
using VKBridgeSDK.Runtime.data;
using VKBridgeSDK.Runtime.managers;
using ILogger = FloorIsLava.VKBridgeSDK.helpers.ILogger;


namespace VKBridgeSDK.Runtime.components
{
    public class VKMessageReceiver : MonoBehaviour
    {
        private VKResponseManager _vkResponseManager;
        private VKEventManager _eventManager;
        private ILogger _logger = new VKBridgeLogger();

        public void Initialize(VKResponseManager responseManager, VKEventManager eventManager)
        {
            _logger.Log("Initialized VKMessageReceiver");
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
            _logger.Log($"ReceiveEvent called with json:  {jsonData}");
            
            var eventObject = JsonUtility.FromJson<VKEvent>(jsonData);
            if (!Enum.TryParse(eventObject.detail.type, out VKBridgeEventType eventType)) 
                return;
            
            _logger.Log($"Parsed {eventType} with {eventObject.detail.data}");
            _eventManager.TriggerEvent(eventType, eventObject.detail.data);
        }

        [UsedImplicitly]
        public void OnFocus()
        {
            Debug.Assert(_eventManager!= null, "VKEventManager can not be null!", this);
            
            _logger.Log("Browser window gained focus");
            _eventManager.TriggerEvent(VKBridgeEventType.FocusChanged, new VKEventData { result = true });
        }

        [UsedImplicitly]
        public void OnBlur()
        {
            Debug.Assert(_eventManager!= null, "VKEventManager can not be null!", this);
            
            _logger.Log("Browser window lost focus");
            _eventManager.TriggerEvent(VKBridgeEventType.FocusChanged, new VKEventData { result = false });
        }
    }
}