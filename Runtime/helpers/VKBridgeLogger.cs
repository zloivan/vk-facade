using System;
using System.Runtime.InteropServices;
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

        [DllImport("__Internal")]
        private static extern void UnityVKBridge_SetLogging(int isLogingEnabled);

        private static readonly bool _logsEnabled;

        static VKBridgeLogger()
        {
            _logsEnabled = false;
#if WEBGL_VK_DEBUG
_logsEnabled = true;
#endif
            
        }
        
        internal static void SetLogging()
        {
            UnityVKBridge_SetLogging(_logsEnabled? 1 : 0);
        }

        public void Log(object message, Object context = null)
        {
            if(_logsEnabled)
                Debug.Log($"{PREFIX}{message}", context);
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