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
        private readonly Dictionary<string, UniTaskCompletionSource<VKPromise>> _promiseTasks =
            new Dictionary<string, UniTaskCompletionSource<VKPromise>>();

        [DllImport("__Internal")]
        private static extern void UnityVKBridge_SendMessage(string methodName, string parameters);

        [DllImport("__Internal")]
        private static extern void UnityVKBridge_Subscribe();

        [DllImport("__Internal")]
        private static extern void UnityVKBridge_Alert(string message);

        [DllImport("__Internal")]
        private static extern void UnityVKBridge_SetupFocusHandlers();

        public VKResponseManager()
        {
            UnityVKBridge_SetupFocusHandlers();
            UnityVKBridge_Subscribe();
        }

        public async UniTask<VKPromise> CallVkMethodAsync(string methodName, params object[] parameters)
        {
            var taskCompletionSource = new UniTaskCompletionSource<VKPromise>();
            _promiseTasks[methodName] = taskCompletionSource;

            var paramsString = parameters.Length > 0 ? JsonUtility.ToJson(parameters) : string.Empty;
            UnityVKBridge_SendMessage(methodName, paramsString);

            return await taskCompletionSource.Task;
        }

        public void HandlePromiseResponse(string jsonData)
        {
            var response = JsonUtility.FromJson<VKPromise>(jsonData);

            if (!_promiseTasks.TryGetValue(response.method, out var tcs))
                return;

            tcs.TrySetResult(response);
            _promiseTasks.Remove(response.method);
        }

        public void HandleErrorResponse(string jsonData)
        {
            var response = JsonUtility.FromJson<VkPromiseError>(jsonData);
            if (!_promiseTasks.TryGetValue(response.method, out var tcs))
                return;

            var data = response.error.data.error_data;
            var errorMessage = !string.IsNullOrEmpty(data.error_description) ? data.error_description :
                !string.IsNullOrEmpty(data.error_msg) ? data.error_msg : data.error_reason;
        
            tcs.TrySetException(new Exception(errorMessage));
            _promiseTasks.Remove(response.method);
        }

        public void ShowAlert(string message)
        {
            UnityVKBridge_Alert(message);
        }
    }
}

