using System;

namespace Exanite.Arpg.Logging
{
    /// <summary>
    /// A log entry
    /// </summary>
    public class LogEntry
    {
        private readonly LogLevel level;
        private readonly Exception exception;
        private readonly string messageTemplate;
        private readonly object[] propertyValues;

        /// <summary>
        /// Creates a new <see cref="LogEntry"/>
        /// </summary>
        public LogEntry(LogLevel level, Exception exception, string messageTemplate, object[] propertyValues)
        {
            this.level = level;
            this.exception = exception;
            this.messageTemplate = messageTemplate;
            this.propertyValues = propertyValues;
        }

        /// <summary>
        /// Level of this <see cref="LogEntry"/>
        /// </summary>
        public LogLevel Level
        {
            get
            {
                return level;
            }
        }

        /// <summary>
        /// <see cref="Exception"/> associated with this <see cref="LogEntry"/>
        /// </summary>
        public Exception Exception
        {
            get
            {
                return exception;
            }
        }

        /// <summary>
        /// Message template describing this <see cref="LogEntry"/>
        /// </summary>
        public string MessageTemplate
        {
            get
            {
                return messageTemplate;
            }
        }

        /// <summary>
        /// Objects positionally formatted into the message template
        /// </summary>
        public object[] PropertyValues
        {
            get
            {
                return propertyValues;
            }
        }
    }
}