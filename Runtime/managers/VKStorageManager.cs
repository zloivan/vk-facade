using System;
using System.Collections.Generic;
using System.Globalization;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using vk_facade.Runtime.helpers;

namespace vk_facade.Runtime.managers
{
    public class VKStorageManager
    {
        public event Action OnLoadComplete;
        public event Action OnSaveComplete;

        private readonly Dictionary<string, string> _loadedStorage = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _toSend = new Dictionary<string, string>();
        private readonly ILogger _logger = new VKBridgeLogger();


        internal async UniTask Load()
        {
            if (Application.isEditor)
            {
                return;
            }

            try
            {
                _logger.Log("Loading from VK...");
                var keyNames = await VkBridgeFacade.StorageGetKeys(100, 0);

                if (keyNames?.keys == null || keyNames.keys.Length == 0)
                {
                    _logger.LogWarning("VK_STORAGE_MANAGER", "No keys found in VK storage.");
                    return;
                }

                var storageContainer = await VkBridgeFacade.StorageGet(keyNames.keys);

                if (storageContainer?.keys == null || storageContainer.keys.Count == 0)
                {
                    _logger.LogWarning("VK_STORAGE_MANAGER", "No data found for keys in VK storage.");
                    return;
                }

                foreach (var keyValue in storageContainer.keys)
                {
                    _loadedStorage[keyValue.key] = keyValue.value;
                }

                _logger.Log("Load from VK complete!");
                OnLoadComplete?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("VK_STORAGE_MANAGER", $"Error loading from VK: {ex.Message}");
            }
        }

        public async UniTask Save()
        {
            if (Application.isEditor)
            {
                PlayerPrefs.Save();
                return;
            }

            try
            {
                _logger.Log("Saving to VK...");
                if (_toSend.Count == 0)
                {
                    _logger.LogWarning("VK_STORAGE_MANAGER", "No data to save to VK.");
                    return;
                }

                // Make copy to prevent concurrency issues
                var storageCopy = new Dictionary<string, string>(_toSend);

                // Clear send list
                _toSend.Clear();

                foreach (var keyValue in storageCopy)
                {
                    await VkBridgeFacade.StorageSet(keyValue.Key, keyValue.Value);
                }

                _logger.Log("Save to VK complete!");
                OnSaveComplete?.Invoke();
            }
            catch (Exception ex)
            {
                _logger.LogError("VK_STORAGE_MANAGER", $"Error saving to VK: {ex.Message}");
            }
        }

        [PublicAPI]
        public void SetString(string key, string value = "")
        {
            if (Application.isEditor)
            {
                PlayerPrefs.SetString(key, value);
                return;
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogWarning("VK_STORAGE_MANAGER", "Key cannot be null or whitespace.");
                return;
            }

            _logger.Log($"SetKeyValue called: [{key}, {value}]");
            _loadedStorage[key] = value;

            if (!_toSend.TryGetValue(key, out var valueToSend) || value != valueToSend)
            {
                _toSend[key] = value;
            }
        }


        [PublicAPI]
        public void SetFloat(string key, float value)
        {
            if (Application.isEditor)
            {
                PlayerPrefs.SetFloat(key, value);
                return;
            }
            
            SetString(key, value.ToString(CultureInfo.InvariantCulture));
        }

        [PublicAPI]
        public void SetInt(string key, int value)
        {
            if (Application.isEditor)
            {
                PlayerPrefs.SetInt(key, value);
                return;
            }
            
            SetString(key, value.ToString());
        }

        [PublicAPI]
        public string GetString(string key, string defaultValue)
        {
            if (Application.isEditor) return PlayerPrefs.GetString(key, defaultValue);
            
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogWarning("VK_STORAGE_MANAGER", "Key cannot be null or whitespace.");
                return defaultValue;
            }

            if (_loadedStorage.TryGetValue(key, out var value))
            {
                _logger.Log($"GetString result: [{key}, {value}]");
                return value;
            }

            _loadedStorage[key] = defaultValue;
            _logger.Log($"GetString using default value: [{key}, {defaultValue}]");
            return defaultValue;
        }

        [PublicAPI]
        public string GetString(string key) => GetString(key, "");

        [PublicAPI]
        public int GetInt(string key, int defaultValue)
        {
            if (Application.isEditor) return PlayerPrefs.GetInt(key, defaultValue);
            
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogWarning("VK_STORAGE_MANAGER", "Key cannot be null or whitespace.");
                return defaultValue;
            }

            if (_loadedStorage.TryGetValue(key, out var value) && int.TryParse(value, out var intValue))
            {
                _logger.Log($"GetInt result: [{key}, {intValue}]");
                return intValue;
            }

            _logger.LogWarning("VK_STORAGE_MANAGER",
                $"Invalid or missing value under key: {key}, using default value: {defaultValue}");
            _loadedStorage[key] = defaultValue.ToString();
            return defaultValue;
        }

        [PublicAPI]
        public int GetInt(string key) => GetInt(key, 0);

        /// <summary>
        /// Get float value in CultureInfo.InvariantCulture
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [PublicAPI]
        public float GetFloat(string key, float defaultValue)
        {
            
            
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogWarning("VK_STORAGE_MANAGER", "Key cannot be null or whitespace.");
                return defaultValue;
            }

            if (_loadedStorage.TryGetValue(key, out var value) && float.TryParse(value, NumberStyles.Float,
                    CultureInfo.InvariantCulture, out var floatValue))
            {
                _logger.Log($"GetFloat result: [{key}, {floatValue}]");
                return floatValue;
            }

            _logger.LogWarning("VK_STORAGE_MANAGER",
                $"Invalid or missing value under key: {key}, using default value: {defaultValue}");
            _loadedStorage[key] = defaultValue.ToString(CultureInfo.InvariantCulture);
            return defaultValue;
        }

        [PublicAPI]
        public float GetFloat(string key) => GetFloat(key, 0f);

        [PublicAPI]
        public void DeleteAll()
        {
            if (Application.isEditor)
            {
                 PlayerPrefs.DeleteAll();
                 return;
            }
            
            _logger.Log("Deleting all keys from storage...");
            foreach (var key in _loadedStorage.Keys)
            {
                _toSend[key] = string.Empty;
            }

            _loadedStorage.Clear();
            _logger.Log("All keys set to be deleted from VK storage.");
        }

        [PublicAPI]
        public void DeleteKey(string key)
        {
            if (Application.isEditor)
            {
                PlayerPrefs.DeleteKey(key);
                return;
            }
            
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogWarning("VK_STORAGE_MANAGER", "Key cannot be null or whitespace.");
                return;
            }

            if (_loadedStorage.ContainsKey(key))
            {
                _loadedStorage.Remove(key);
                _toSend[key] = string.Empty;
                _logger.Log($"Key {key} set to be deleted from VK storage.");
            }
            else
            {
                _logger.LogWarning("VK_STORAGE_MANAGER", $"Key {key} not found in loaded storage.");
            }
        }

        [PublicAPI]
        public bool HasKey(string key)
        {
            if (Application.isEditor) return PlayerPrefs.HasKey(key);
            
            
            if (string.IsNullOrWhiteSpace(key))
            {
                _logger.LogWarning("VK_STORAGE_MANAGER", "Key cannot be null or whitespace.");
                return false;
            }

            var exists = _loadedStorage.ContainsKey(key);
            _logger.Log($"HasKey check for {key}: {exists}");
            return exists;
        }
    }
}