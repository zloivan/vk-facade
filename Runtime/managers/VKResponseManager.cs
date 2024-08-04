using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using vk_facade.Runtime.data;
using vk_facade.Runtime.helpers;

namespace vk_facade.Runtime.managers
{
    public class VKResponseManager
    {
        private readonly Dictionary<string, IVKPromise> _promises = new Dictionary<string, IVKPromise>();
        private readonly ILogger _logger = new VKBridgeLogger();

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

        public async UniTask<T> CallVkMethodAsync<T>(string methodName, VKParams parameters = default) where T : VKData
        {
            var vkPromise = new VKPromise<T>();
            _promises[methodName] = vkPromise;

            if (!Application.isEditor)
            {
                UnityVKBridge_SendMessage(methodName, parameters?.GetParams());
            }
            else
            {
                //Editor simulation, since in editor methods will never return value
                UniTask.Delay(300);
                var defaultInstance = Activator.CreateInstance<T>();
                vkPromise.CompletionSource.TrySetResult(defaultInstance);
            }

            _logger.Log("RESPONSE_MANAGER", $"VKBridge.send<T>({methodName}, {parameters?.GetParams()}) called");

            return await vkPromise.CompletionSource.Task;
        }

        public void HandlePromiseResponse(string jsonResponse)
        {
            _logger.Log("RESPONSE_MANAGER", $"GOT RESPONSE FROM PROMISE JSON: {jsonResponse}");
            VKPromiseResponse response;
            try
            {
                response = JsonConvert.DeserializeObject<VKPromiseResponse>(jsonResponse);
                _logger.Log("RESPONSE_MANAGER", $"Deserialized by newton RESPONSE FROM PROMISE data: {response.data}");
                _logger.Log("RESPONSE_MANAGER",
                    $"Deserialized by newton RESPONSE FROM PROMISE error: {response.error}");
                _logger.Log("RESPONSE_MANAGER",
                    $"Deserialized by newton RESPONSE FROM PROMISE error: {response.method}");
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError("RESPONSE_MANAGER", $"JsonReaderException: {ex.Message}");
                throw;
            }

            if (!_promises.TryGetValue(response.method, out var promise))
                return;

            promise.SetResult(response.data?.ToString());
            _promises.Remove(response.method);
        }

        public void HandleErrorResponse(string jsonResponse)
        {
            _logger.Log("RESPONSE_MANAGER", $"GOT RESPONSE FROM PROMISE JSON: {jsonResponse}");
            VKPromiseResponse response;
            try
            {
                response = JsonConvert.DeserializeObject<VKPromiseResponse>(jsonResponse);
                _logger.Log("RESPONSE_MANAGER", $"Deserialized by newton RESPONSE FROM PROMISE data: {response.data}");
                _logger.Log("RESPONSE_MANAGER",
                    $"Deserialized by newton RESPONSE FROM PROMISE error: {response.error}");
                _logger.Log("RESPONSE_MANAGER",
                    $"Deserialized by newton RESPONSE FROM PROMISE error: {response.method}");
            }
            catch (JsonReaderException exception)
            {
                _logger.LogError("RESPONSE_MANAGER", $"JsonReaderException: {exception.Message}");
                throw;
            }

            if (!_promises.TryGetValue(response.method, out var promise))
                return;

            promise.SetException(response.error?.ToString());
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
                _logger.Log("RESPONSE_MANAGER",$"VKBridge.alert({message}) called");
            }
        }
    }
}