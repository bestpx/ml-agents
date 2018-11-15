using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommonUtil
{
    
    public interface ILogger
    {
        void Log(LogLevel level, string message);
    }

    public class Logger : ILogger
    {
        private LogLevel _level;

        public Logger(LogLevel level)
        {
            _level = level;
        }

        public void SetLogLevel(LogLevel level)
        {
            _level = level;
        }
        
        public void Log(LogLevel level, string message)
        {
            if (level < _level)
            {
                return;
            }

            if (level == LogLevel.Error)
            {
                Debug.LogError(message);
            }
            else if (level == LogLevel.Warning)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.Log(message);
            }
        }
    }


    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warning,
        Error,
    }
}