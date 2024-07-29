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

        public bool useold = true;

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

        [Obsolete]
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
                        data = new VKPromiseDataOLD
                        {
                            result = true
                        }
                    }
                );

                Debug.Log($"VKBridge.send({methodName}, {paramsString}) called");
            }


            return await taskCompletionSource.Task;
            
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

        private readonly Dictionary<string, IVKPromise> _promises = new Dictionary<string, IVKPromise>();

        public void HandlePromiseResponse(string jsonData)
        {
            if (useold)
            {
                HandlePromiseResponseOld(jsonData);
                
            }
            else
            {
                HandlePromiseResponseNew(jsonData);
            }
        }


        [Obsolete]
        public void HandlePromiseResponseOld(string jsonData)
        {
            var response = JsonUtility.FromJson<VKPromise>(jsonData);

            Debug.Log("Response parsed for " + response.method);

            if (!_promiseTasks.TryGetValue(response.method, out var tcs))
                return;

            tcs.TrySetResult(response);
            
            _promiseTasks.Remove(response.method);
        }
        public void HandlePromiseResponseNew(string jsonResponse)
        {
            var response = JsonUtility.FromJson<VKPromiseResponse>(jsonResponse);
            if (_promises.TryGetValue(response.method, out var promise))
            {
                promise.SetResult(response.data);
                _promises.Remove(response.method);
            }
        }

        public void HandleErrorResponseNew(string jsonData)
        {
            var response = JsonUtility.FromJson<VKPromiseResponse>(jsonData);
            
            if (_promises.TryGetValue(response.method, out var promise))
            {
                promise.SetException(response.data);
                _promises.Remove(response.method);
            }
        }

        [Obsolete]
        public void HandleErrorResponseOld(string jsonData)
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
        
        public void HandleErrorResponse(string jsonData)
        {
            if (useold)
            {
                HandleErrorResponseOld(jsonData);
            }
            else
            {
                HandleErrorResponseNew(jsonData);
            }
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