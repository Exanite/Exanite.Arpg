using Serilog;
using Serilog.Events;

namespace Exanite.Arpg.Logging
{
    public class SerilogLogAdapter : ILog
    {
        private readonly ILogger inner;

        public SerilogLogAdapter(ILogger inner)
        {
            this.inner = inner;
        }   

        public ILog ForContext(string property, object value)
        {
            return new SerilogLogAdapter(inner.ForContext(property, value));
        }

        public bool IsEnabled(LogLevel level)
        {
            return inner.IsEnabled((LogEventLevel)level);
        }

        public void Log(LogEntry entry)
        {
            inner.Write((LogEventLevel)entry.Level, entry.Exception, entry.MessageTemplate, entry.PropertyValues);
        }
    }
}
