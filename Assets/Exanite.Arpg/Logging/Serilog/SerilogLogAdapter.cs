using Serilog;

namespace Exanite.Arpg.Logging.Serilog
{
    /// <summary>
    /// Adapts Serilog's logger to <see cref="ILog"/>
    /// </summary>
    public class SerilogLogAdapter : ILog
    {
        private readonly ILogger inner;

        /// <summary>
        /// Create a new <see cref="SerilogLogAdapter"/>
        /// </summary>
        public SerilogLogAdapter(ILogger inner)
        {
            this.inner = inner;
        }

        /// <summary>
        /// Create a logger that enriches LogEntries with the specified property
        /// </summary>
        public ILog ForContext(string property, object value)
        {
            return new SerilogLogAdapter(inner.ForContext(property, value));
        }

        /// <summary>
        /// Determine if events of the specified level will be logged
        /// </summary>
        public bool IsEnabled(LogLevel level)
        {
            return inner.IsEnabled(level.ToLogEventLevel());
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/>
        /// </summary>
        public void Log(LogEntry entry)
        {
            inner.Write(entry.Level.ToLogEventLevel(), entry.Exception, entry.MessageTemplate, entry.PropertyValues);
        }
    }
}
