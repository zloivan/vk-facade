using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using vk_facade.Runtime.helpers;
using ILogger = vk_facade.Runtime.helpers.ILogger;

namespace vk_facade.Runtime.data
{
    public interface IVKPromise
    {
        void SetResult(string jsonData);
        void SetException(string jsonExceptionData);
    }
    
    public class VKPromise<T> : IVKPromise where T : VKData
    {
        private readonly ILogger _logger = new VKBridgeLogger();
        public UniTaskCompletionSource<T> CompletionSource { get; set; } = new UniTaskCompletionSource<T>();

        public void SetResult(string jsonData)
        {
            _logger.Log($"SetResult called, json Data: {jsonData}");
            var data = JsonUtility.FromJson<T>(jsonData);

            _logger.Log($"Deserialized data: {data}");
            CompletionSource.TrySetResult(data);
        }

        public void SetException(string jsonExceptionData)
        {
            _logger.Log($"SetException called, json Data: {jsonExceptionData} ");
            
            var error = JsonUtility.FromJson<VKError>(jsonExceptionData);
            
            _logger.Log($"Deserialized data: {jsonExceptionData}");
            
            var data = error.data.error_data;
            var errorMessage = !string.IsNullOrEmpty(data.error_description) ? data.error_description :
                !string.IsNullOrEmpty(data.error_msg) ? data.error_msg : data.error_reason;

            CompletionSource.TrySetException(new Exception(errorMessage));
        }
    }
}