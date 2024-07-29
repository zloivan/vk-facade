using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace VKBridgeSDK.Runtime.data
{
    public interface IVKPromise
    {
        void SetResult(string jsonData);
        void SetException(string jsonExceptionData);
    }
    
    public class VKPromise<T> : IVKPromise where T : VKData
    {
        public UniTaskCompletionSource<T> CompletionSource { get; set; } = new UniTaskCompletionSource<T>();

        public void SetResult(string jsonData)
        {
            var data = JsonUtility.FromJson<T>(jsonData);
            CompletionSource.TrySetResult(data);
        }

        public void SetException(string jsonExceptionData)
        {
            var error = JsonUtility.FromJson<VKError>(jsonExceptionData);

            var data = error.data.error_data;
            var errorMessage = !string.IsNullOrEmpty(data.error_description) ? data.error_description :
                !string.IsNullOrEmpty(data.error_msg) ? data.error_msg : data.error_reason;

            CompletionSource.TrySetException(new Exception(errorMessage));
        }
    }
}