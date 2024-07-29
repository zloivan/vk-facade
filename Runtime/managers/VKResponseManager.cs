using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VKBridgeSDK.Runtime.data;

namespace VKBridgeSDK.Runtime.managers
{
    public class VKResponseManager
    {
        private readonly Dictionary<string, IVKPromise> _promises = new Dictionary<string, IVKPromise>();

        [DllImport("__Internal")]
        private static extern void UnityVKBridge_SendMessage(string methodName, string parameters);

        [DllImport("__Internal")]
        private static extern void UnityVKBridge_Alert(string message);

        [DllImport("__Internal")]
        private static extern void UnityVKBridge_SetupFocusHandlers();

        public VKResponseManager()
        {
            SetupFocusHandlers();
        }

        private static void SetupFocusHandlers()
        {
            if (!Application.isEditor)
            {
                UnityVKBridge_SetupFocusHandlers();
            }
        }

        public async UniTask<T> CallVkMethodAsync<T>(string methodName, object parameters = null) where T : VKData
        {
            var vkPromise = new VKPromise<T>();
            _promises[methodName] = vkPromise;

            var paramsString = parameters != null
                ? Newtonsoft.Json.JsonConvert.SerializeObject(parameters)
                : string.Empty;

            if (!Application.isEditor)
            {
                UnityVKBridge_SendMessage(methodName, paramsString);
            }
            else
            {
                UniTask.Delay(300);
                var defaultInstance = Activator.CreateInstance<T>();
                vkPromise.CompletionSource.TrySetResult(defaultInstance);
                Debug.Log($"VKBridge.send<T>({methodName}, {paramsString}) called");
            }

            return await vkPromise.CompletionSource.Task;
        }

        public void HandlePromiseResponse(string jsonReponse)
        {
            var response = JsonUtility.FromJson<VKPromiseResponse>(jsonReponse);
            if (!_promises.TryGetValue(response.method, out var promise)) 
                return;
            
            promise.SetResult(response.data);
            _promises.Remove(response.method);
        }

        public void HandleErrorResponse(string jsonData)
        {
            var response = JsonUtility.FromJson<VKPromiseResponse>(jsonData);

            if (!_promises.TryGetValue(response.method, out var promise)) 
                return;
            
            promise.SetException(response.data);
            _promises.Remove(response.method);
        }

        public void ShowAlert(string message)
        {
            if (!Application.isEditor)
            {
                UnityVKBridge_Alert(message);
            }
            else
            {
                Debug.Log($"VKBridge.alert({message}) called");
            }
        }
    }
}