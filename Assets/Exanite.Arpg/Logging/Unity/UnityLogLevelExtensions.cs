using System;
using UnityEngine;

namespace Exanite.Arpg.Logging.Unity
{
    /// <summary>
    /// Extensions for Unity's <see cref="LogType"/>
    /// </summary>
    public static class UnityLogTypeExtensions
    {
        /// <summary>
        /// Converts a <see cref="LogLevel"/> to a <see cref="LogType"/>
        /// </summary>
        public static LogType ToLogType(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Verbose: return LogType.Log;
                case LogLevel.Debug: return LogType.Log;
                case LogLevel.Information: return LogType.Log;
                case LogLevel.Warning: return LogType.Warning;
                case LogLevel.Error: return LogType.Error;
                case LogLevel.Fatal: return LogType.Error;
                default: throw new NotSupportedException($"{level} is not a supported {typeof(LogLevel).Name}");
            }
        }

        /// <summary>
        /// Converts a <see cref="LogType"/> to a <see cref="LogLevel"/>
        /// </summary>
        public static LogLevel ToLogLevel(this LogType level)
        {
            switch (level)
            {
                case LogType.Error: return LogLevel.Error;
                case LogType.Assert: return LogLevel.Warning;
                case LogType.Warning: return LogLevel.Warning;
                case LogType.Log: return LogLevel.Information;
                case LogType.Exception: return LogLevel.Error;
                default: throw new NotSupportedException($"{level} is not a supported {typeof(LogType).Name}");
            }
        }
    }
}