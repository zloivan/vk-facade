using System;
using JetBrains.Annotations;
using UnityEngine;
using vk_facade.Runtime.data;
using vk_facade.Runtime.helpers;
using vk_facade.Runtime.managers;

namespace vk_facade.Runtime.components
{
    public class VKMessageReceiver : MonoBehaviour
    {
        private VKResponseManager _vkResponseManager;
        private VKEventManager _eventManager;
        private readonly ILogger _logger = new VKBridgeLogger();

        public void Initialize(VKResponseManager responseManager, VKEventManager eventManager)
        {
            _logger.Log("MESSAGE_RECEIVER","Initialized VKMessageReceiver");
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
            _logger.Log("MESSAGE_RECEIVER",$"ReceiveEvent called with json:  {jsonData}");
            
            var eventObject = JsonUtility.FromJson<VKEvent>(jsonData);
            if (!Enum.TryParse(eventObject.detail.type, out VKBridgeEventType eventType)) 
                return;
            
            _logger.Log("MESSAGE_RECEIVER",$"Parsed {eventType} with {eventObject.detail.data}");
            _eventManager.TriggerEvent(eventType, eventObject.detail.data);
        }

        [UsedImplicitly]
        public void OnFocus()
        {
            Debug.Assert(_eventManager!= null, "VKEventManager can not be null!", this);
            
            _logger.Log("MESSAGE_RECEIVER","Browser window gained focus");
            _eventManager.TriggerEvent(VKBridgeEventType.FocusChanged, new VKEventData { result = true });
        }

        [UsedImplicitly]
        public void OnBlur()
        {
            Debug.Assert(_eventManager!= null, "VKEventManager can not be null!", this);
            
            _logger.Log("MESSAGE_RECEIVER","Browser window lost focus");
            _eventManager.TriggerEvent(VKBridgeEventType.FocusChanged, new VKEventData { result = false });
        }
    }
}