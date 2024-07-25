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


        public async UniTask<VKPromise> CallVkMethodAsync(string methodName, params object[] parameters)
        {
            var taskCompletionSource = new UniTaskCompletionSource<VKPromise>();
            _promiseTasks[methodName] = taskCompletionSource;

            var paramsString = parameters.Length > 0
                ? Newtonsoft.Json.JsonConvert.SerializeObject(parameters)
                : string.Empty;


            if (!Application.isEditor)
            {
                UnityVKBridge_SendMessage(methodName, paramsString);
            }
            else
            {
                UniTask.Delay(300);
                taskCompletionSource.TrySetResult(
                    new VKPromise
                    {
                        method = methodName,
                        data = new VKPromiseData
                        {
                            result = true
                        }
                    }
                );

                Debug.Log($"VKBridge.send({methodName}, {paramsString}) called");
            }


            return await taskCompletionSource.Task;
        }


        public void HandlePromiseResponse(string jsonData)
        {

            var response = JsonUtility.FromJson<VKPromise>(jsonData);

            Debug.Log("Response parsed for " + response.method);

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