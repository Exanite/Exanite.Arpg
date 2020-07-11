using Serilog.Events;

namespace Exanite.Arpg.Logging.Serilog
{
    /// <summary>
    /// Extensions for Serilog's <see cref="LogEventLevel"/>
    /// </summary>
    public static class SerilogLogEventLevelExtensions
    {
        /// <summary>
        /// Converts a <see cref="LogLevel"/> to a <see cref="LogEventLevel"/>
        /// </summary>
        public static LogEventLevel ToLogEventLevel(this LogLevel level)
        {
            return (LogEventLevel)level;
        }

        /// <summary>
        /// Converts a <see cref="LogEventLevel"/> to a <see cref="LogLevel"/>
        /// </summary>
        public static LogLevel ToLogLevel(this LogEventLevel level)
        {
            return (LogLevel)level;
        }
    }
}