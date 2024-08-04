using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using vk_facade.Runtime.helpers;


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
            _logger.Log("PROMISE", $"SetResult called, json Data: {jsonData}");

            T data;
            try
            {
                data = JsonUtility.FromJson<T>(jsonData);
                if (!data.IsValid)
                {
                    throw new Exception("JsonUtility failed to deserialize data.");
                }

                _logger.Log("PROMISE", "Deserialization using JsonUtility was successful.");
            }
            catch (Exception ex)
            {
                _logger.Log("PROMISE",
                    $"JsonUtility deserialization failed: {ex.Message}. Falling back to Newtonsoft.Json.");
                try
                {
                    data = JsonConvert.DeserializeObject<T>(jsonData);
                    if (data.IsValid)
                    {
                        _logger.Log("PROMISE", "Deserialization using Newtonsoft.Json was successful.");
                    }
                    else
                    {
                        throw new JsonException();
                    }
                }
                catch (JsonException newtonEx)
                {
                    _logger.LogError("PROMISE", $"Newtonsoft.Json deserialization also failed: {newtonEx.Message}");
                    CompletionSource.TrySetException(
                        new Exception("Deserialization failed with both JsonUtility and Newtonsoft.Json."));
                    return;
                }
            }

            _logger.Log("PROMISE", $"Deserialized data: {data}");
            CompletionSource.TrySetResult(data);
        }

        public void SetException(string jsonExceptionData)
        {
            _logger.Log("PROMISE", $"SetException called, json Data: {jsonExceptionData} ");

            VKError error;
            try
            {
                error = JsonUtility.FromJson<VKError>(jsonExceptionData);

                if (!error.IsValid)
                {
                    throw new Exception("JsonUtility failed to deserialize error.");
                }

                _logger.Log("PROMISE", "Deserialization using JsonUtility was successful.");
            }
            catch (Exception e)
            {
                _logger.Log("PROMISE",
                    $"JsonUtility deserialization failed: {e.Message}. Falling back to Newtonsoft.Json.");

                try
                {
                    error = JsonConvert.DeserializeObject<VKError>(jsonExceptionData);
                    if (error.IsValid)
                    {
                        _logger.Log("PROMISE", "Deserialization using Newtonsoft.Json was successful.");
                    }
                    else
                    {
                        throw new JsonException();
                    }
                }
                catch (JsonException exception)
                {
                    _logger.LogError("PROMISE", $"Newtonsoft.Json deserialization also failed: {exception.Message}");
                    CompletionSource.TrySetException(
                        new Exception("Deserialization failed with both JsonUtility and Newtonsoft.Json."));
                    return;
                }
            }

            _logger.Log("PROMISE", $"Deserialized data: {jsonExceptionData}");

            var data = error.data.error_data;
            var errorMessage = !string.IsNullOrEmpty(data.error_description) ? data.error_description :
                !string.IsNullOrEmpty(data.error_msg) ? data.error_msg : data.error_reason;

            CompletionSource.TrySetException(new Exception(errorMessage));
        }
    }
}