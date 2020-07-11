using System;
using DarkRift;

namespace Exanite.Arpg.Logging.DarkRift
{
    /// <summary>
    /// Extensions for Dark Rift's <see cref="LogType"/>
    /// </summary>
    public static class DarkRiftLogTypeExtensions
    {
        /// <summary>
        /// Converts a <see cref="LogLevel"/> to a <see cref="LogType"/>
        /// </summary>
        public static LogType ToLogType(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Verbose: return LogType.Trace;
                case LogLevel.Debug: return LogType.Trace;
                case LogLevel.Information: return LogType.Info;
                case LogLevel.Warning: return LogType.Warning;
                case LogLevel.Error: return LogType.Error;
                case LogLevel.Fatal: return LogType.Fatal;
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
                case LogType.Trace: return LogLevel.Debug;
                case LogType.Info: return LogLevel.Information;
                case LogType.Warning: return LogLevel.Warning;
                case LogType.Error: return LogLevel.Error;
                case LogType.Fatal: return LogLevel.Fatal;
                default: throw new NotSupportedException($"{level} is not a supported {typeof(LogType).Name}");
            }
        }
    }
}