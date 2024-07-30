using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FloorIsLava.VKBridgeSDK.helpers
{
    internal interface ILogger
    {
        public void Log(object message, Object context = null);
        public void LogError(object message, Object context = null);
        public void LogWarning(object message, Object context = null);
        public void LogException(Exception exception, Object context = null);
    }

    internal class VKBridgeLogger : ILogger
    {
        private const string PREFIX = "<b>VKBridgeSDK:</b>: ";

        public void Log(object message, Object context = null)
        {
#if WEBGL_VK_DEBUG
            Debug.Log($"{PREFIX}{message}", context);
#endif
        }

        public void LogError(object message, Object context = null)
        {
            Debug.LogError($"{PREFIX}{message}", context);
        }

        public void LogWarning(object message, Object context = null)
        {
            Debug.LogWarning($"{PREFIX}{message}", context);
        }

        public void LogException(Exception exception, Object context = null)
        {
            Debug.LogException(exception, context);
        }
    }
}