using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VKBridgeSDK.Runtime.components;
using VKBridgeSDK.Runtime.data;
using VKBridgeSDK.Runtime.managers;
using Object = UnityEngine.Object;

namespace VKBridgeSDK.Runtime
{
    public static class VkBridgeFacade
    {
        private static VKResponseManager _vkResponseManager;
        private static VKEventManager _eventManager;
        private static GameObject _messageReceiverObject;
        private static VKMessageReceiver _vkMessageReceiver;

        public const bool IsDebug = true;

        public static void Initialize()
        {
            _vkResponseManager = new VKResponseManager();
            _eventManager = new VKEventManager();

            _messageReceiverObject = new GameObject("VKMessageReceiver");
            _vkMessageReceiver = _messageReceiverObject.AddComponent<VKMessageReceiver>();
            _vkMessageReceiver.Initialize(_vkResponseManager, _eventManager);

            Object.DontDestroyOnLoad(_messageReceiverObject);

            if (IsDebug)
            {
                SpawnDebugMenu();
            }
        }

        private static void SpawnDebugMenu()
        {
            var go = new GameObject("VKDebugMenu");
            go.AddComponent<VKBridgeDebugMenu>();
            Object.DontDestroyOnLoad(go);
        }

        public static async UniTask<VKPromise> GetUserInfo()
        {
            return await _vkResponseManager.CallVkMethodAsync("VKWebAppGetUserInfo");
        }

        public static async UniTask<VKPromise> CallAPIMethod(string methodName, string parameters)
        {
            return await _vkResponseManager.CallVkMethodAsync("VKWebAppCallAPIMethod", new { method = methodName, @params = parameters });
        }

        public static async UniTask<bool> VkBridgeInit()
        {
            var vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppInit");
            return vkPromise._vkPromiseData.result;
        }

        public static async UniTask<bool> ShowLeaderBoard(int result = -1)
        {
            VKPromise vkPromise;
            if (result == -1)
            {
                 vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowLeaderBoardBox");
            }
            else
            {
                vkPromise = await _vkResponseManager.CallVkMethodAsync("VKWebAppShowLeaderBoardBox", new {user_result = result}); 
            }
             
            return vkPromise._vkPromiseData.result;
        }

        public static void ShowAlert(string message)
        {
            _vkResponseManager.ShowAlert(message);
        }

        public static void AddEventListener(VKBridgeEventType eventType, Action<VKPromiseData> listener)
        {
            _eventManager.AddEventListener(eventType, listener);
        }

        public static void RemoveEventListener(VKBridgeEventType eventType, Action<VKPromiseData> listener)
        {
            _eventManager.RemoveEventListener(eventType, listener);
        }

        public static void Reset()
        {
            if (_messageReceiverObject != null)
            {
                Object.Destroy(_messageReceiverObject);
            }
            _vkResponseManager = null;
            _eventManager = null;
            _messageReceiverObject = null;
            _vkMessageReceiver = null;
        }
    }
}