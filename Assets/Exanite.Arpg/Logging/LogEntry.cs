using System;

namespace Exanite.Arpg.Logging
{
    public class LogEntry
    {
        public LogLevel Level;
        public Exception Exception;
        public string MessageTemplate;
        public object[] PropertyValues;

        public LogEntry(LogLevel level, Exception exception, string messageTemplate, object[] propertyValues)
        {
            Level = level;
            Exception = exception;
            MessageTemplate = messageTemplate;
            PropertyValues = propertyValues;
        }
    }
}