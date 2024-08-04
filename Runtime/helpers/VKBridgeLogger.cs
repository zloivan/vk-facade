using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace vk_facade.Runtime.helpers
{
    internal class VKBridgeLogger : ILogger
    {
        [DllImport("__Internal")]
        private static extern void UnityVKBridge_SetLogging(int isLogingEnabled);

        internal VKBridgeLogger()
        {
            var logsEnabled = false;
#if WEBGL_VK_DEBUG
            logsEnabled = true;
#endif
            if (Application.isEditor == false)
            {
                UnityVKBridge_SetLogging(logsEnabled ? 1 : 0);
            }
        }
        
        void ILogger.LogException(Exception exception)
        {
            LogException(exception);
        }

        public ILogHandler logHandler { get; set; } = Debug.unityLogger.logHandler;
        public bool logEnabled { get; set; } = true;
        public LogType filterLogType { get; set; } = LogType.Log;

        private const string PREFIX = "VK_BRIDGE: ";

        public void Log(LogType logType, object message)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, null, PREFIX + "{0}", message);
            }
        }

        public void Log(LogType logType, object message, Object context)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, context, PREFIX + "{0}", message);
            }
        }

        public void Log(LogType logType, string tag, object message)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, null, PREFIX + "{0}: {1}", tag, message);
            }
        }

        public void Log(LogType logType, string tag, object message, Object context)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, context, PREFIX + "{0}: {1}", tag, message);
            }
        }

        public void Log(object message)
        {
            Log(LogType.Log, message);
        }

        public void Log(string tag, object message)
        {
            Log(LogType.Log, tag, message);
        }

        public void Log(string tag, object message, Object context)
        {
            Log(LogType.Log, tag, message, context);
        }

        public void LogWarning(string tag, object message)
        {
            logHandler.LogFormat(LogType.Warning, null, PREFIX + "Warning: {0}: {1}", tag, message);
        }

        public void LogWarning(string tag, object message, Object context)
        {
            logHandler.LogFormat(LogType.Warning, context, PREFIX + "Warning: {0}: {1}", tag, message);
        }

        public void LogError(string tag, object message)
        {
            logHandler.LogFormat(LogType.Error, null, PREFIX + "Error: {0}: {1}", tag, message);
        }

        public void LogError(string tag, object message, Object context)
        {
            logHandler.LogFormat(LogType.Error, context, PREFIX + "Error: {0}: {1}", tag, message);
        }

        public void LogFormat(LogType logType, Object context, string format, params object[] args)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, context, PREFIX + format, args);
            }
        }


        public void LogFormat(LogType logType, string format, params object[] args)
        {
            if (IsLogTypeAllowed(logType))
            {
                logHandler.LogFormat(logType, null, PREFIX + format, args);
            }
        }

        public void LogException(Exception exception)
        {
            logHandler.LogException(exception, null);
        }

        public void LogException(Exception exception, Object context)
        {
            logHandler.LogException(exception, context);
        }

        public bool IsLogTypeAllowed(LogType logType)
        {
#if WEBGL_VK_DEBUG
            if (logType == LogType.Log && logEnabled && filterLogType == LogType.Log)
            {
                return true;
            }
#endif
            return logType == LogType.Warning || logType == LogType.Error || logType == LogType.Exception;
        }
    }
}