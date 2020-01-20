using System;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using UnityEngine;

namespace Prototype
{
    /// <summary>
    /// Writes <see cref="LogEvent"/>s to the Unity Console
    /// </summary>
    public class UnityConsoleSink : ILogEventSink
    {
        private const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}";

        private readonly ITextFormatter formatter;
        private readonly ILogHandler unityLogHandler;
        /// <summary>
        /// Cached empty array used for passing into <see cref="ILogHandler.LogFormat"/>
        /// </summary>
        private static readonly object[] empty = new object[0];

        /// <summary>
        /// Creates a new <see cref="UnityConsoleSink"/>
        /// </summary>
        /// <param name="unityLogHandler">Unity's default log handler, found at <see cref="Debug.unityLogger.logHandler"/></param>
        /// <param name="outputTemplate">
        /// A message template describing the format used to write to this sink<para/>
        /// The default is "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        /// </param>
        public UnityConsoleSink(ILogHandler unityLogHandler, string outputTemplate = DefaultOutputTemplate)
            : this(unityLogHandler, new MessageTemplateTextFormatter(outputTemplate)) { }

        /// <summary>
        /// Creates a new <see cref="UnityConsoleSink"/>
        /// </summary>
        /// <param name="unityLogHandler"></param>
        /// <param name="formatter">A <see cref="ITextFormatter"/> used to convert LogEvents into text for the Unity Console</param>
        public UnityConsoleSink(ILogHandler unityLogHandler, ITextFormatter formatter)
        {
            this.formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            this.unityLogHandler = unityLogHandler;
        }

        /// <summary>
        /// Writes a <see cref="LogEvent"/> to the Unity Console
        /// </summary>
        /// <param name="logEvent">The <see cref="LogEvent"/> to write</param>
        public void Emit(LogEvent logEvent)
        {
            using (StringWriter writer = new StringWriter())
            {
                formatter.Format(logEvent, writer);

                LogType type = ConvertToLogType(logEvent.Level);
                string message = writer.ToString();

                unityLogHandler.LogFormat(type, null, message, empty);
            }
        }

        private LogType ConvertToLogType(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose: return LogType.Log;
                case LogEventLevel.Debug: return LogType.Log;
                case LogEventLevel.Information: return LogType.Log;
                case LogEventLevel.Warning: return LogType.Warning;
                case LogEventLevel.Error: return LogType.Error;
                case LogEventLevel.Fatal: return LogType.Error;
                default: return LogType.Log;
            }
        }
    }
}
