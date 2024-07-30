using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Cysharp.Threading.Tasks;
using FloorIsLava.VKBridgeSDK.helpers;
using Newtonsoft.Json;
using UnityEngine;
using VKBridgeSDK.Runtime.data;
using ILogger = FloorIsLava.VKBridgeSDK.helpers.ILogger;

namespace VKBridgeSDK.Runtime.managers
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

        public async UniTask<T> CallVkMethodAsync<T>(string methodName, object parameters = null) where T : VKData
        {
            var vkPromise = new VKPromise<T>();
            _promises[methodName] = vkPromise;

            var paramsString = parameters != null
                ? JsonConvert.SerializeObject(parameters)
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
                _logger.Log($"VKBridge.send<T>({methodName}, {paramsString}) called");
            }

            return await vkPromise.CompletionSource.Task;
        }

        public void HandlePromiseResponse(string jsonResponse)
        {
            _logger.Log($"GOT RESPONSE FROM PROMISE JSON: {jsonResponse}");
            VKPromiseResponse response;
            try
            {
                response = JsonConvert.DeserializeObject<VKPromiseResponse>(jsonResponse);
                _logger.Log($"Deserialized by newton RESPONSE FROM PROMISE data: {response.data}");
                _logger.Log($"Deserialized by newton RESPONSE FROM PROMISE error: {response.error}");
                _logger.Log($"Deserialized by newton RESPONSE FROM PROMISE error: {response.method}");
            }
            catch  (JsonReaderException ex)
            {
                _logger.LogError($"JsonReaderException: {ex.Message}");
                throw;
            }

            if (!_promises.TryGetValue(response.method, out var promise))
                return;

            promise.SetResult(response.data?.ToString());
            _promises.Remove(response.method);
        }

        public void HandleErrorResponse(string jsonResponse)
        {
            
            _logger.Log($"GOT RESPONSE FROM PROMISE JSON: {jsonResponse}");
            VKPromiseResponse response;
            try
            {
                response = JsonConvert.DeserializeObject<VKPromiseResponse>(jsonResponse);
                _logger.Log($"Deserialized by newton RESPONSE FROM PROMISE data: {response.data}");
                _logger.Log($"Deserialized by newton RESPONSE FROM PROMISE error: {response.error}");
                _logger.Log($"Deserialized by newton RESPONSE FROM PROMISE error: {response.method}");
            }
            catch (JsonReaderException exception)
            {
                _logger.LogError($"JsonReaderException: {exception.Message}");
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
                _logger.Log($"VKBridge.alert({message}) called");
            }
        }
    }
}