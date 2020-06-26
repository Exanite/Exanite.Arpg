using System;
using Serilog.Events;
using UnityEngine;
using ILogger = Serilog.ILogger;

namespace Exanite.Arpg.Logging
{
    /// <summary>
    /// Intercepts Unity's Debug.Log messages and sends them to a <see cref="Serilog.ILogger"/>
    /// </summary>
    public class UnityToSerilogLogHandler : ILogHandler, IDisposable
    {
        private bool isActivated = false;
        private bool hasDisposed = false;

        private ILogHandler inner;

        private readonly ILogger log;

        public bool IsActivated
        {
            get
            {
                return isActivated;
            }

            set
            {
                isActivated = value;
            }
        }

        /// <summary>
        /// Creates a new <see cref="UnityToSerilogLogHandler"/>
        /// </summary>
        /// <param name="log"><see cref="Serilog.ILogger"/> to log to</param>
        public UnityToSerilogLogHandler(ILogger log)
        {
            // Not ForContext<UnityToSerilogLogHandler> because this is so log messages from Unity have their context set properly
            this.log = log.ForContext("SourceContext", "Unity");
        }

        /// <summary>
        /// Logs an exception
        /// </summary>
        public void LogException(Exception exception, UnityEngine.Object context)
        {
            log.Error(exception, "Unhandled exception");
        }

        /// <summary>
        /// Logs a message
        /// </summary>
#pragma warning disable Serilog004 // Constant MessageTemplate verifier
        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogEventLevel level = ConvertToLogEventLevel(logType);

            log.Write(level, message);
        }
#pragma warning restore Serilog004 // Constant MessageTemplate verifier

        /// <summary>
        /// Starts the interception of Unity Debug.Log messages<para/>
        /// </summary>
        public void Activate()
        {
            if (!IsActivated)
            {
                inner = Debug.unityLogger.logHandler;
                Debug.unityLogger.logHandler = this;

                IsActivated = true;

                log.ForContext<UnityToSerilogLogHandler>().Debug("Starting interception of Unity Debug.Log messages");
            }
        }

        /// <summary>
        /// Stops the interception of Unity Debug.Log messages<para/>
        /// Note: This is also called when this <see cref="UnityToSerilogLogHandler"/> is disposed or goes out of scope
        /// </summary>
        public void Deactivate()
        {
            if (IsActivated)
            {
                Debug.unityLogger.logHandler = inner;
                inner = null;

                IsActivated = false;

                log.ForContext<UnityToSerilogLogHandler>().Debug("Stopping interception of Unity Debug.Log messages");
            }
        }

        /// <summary>
        /// Stops the interception of Unity's log messages
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            if (!hasDisposed)
            {
                if (dispose)
                {
                    Deactivate();
                }

                hasDisposed = true;
            }
        }

        private LogEventLevel ConvertToLogEventLevel(LogType logType)
        {
            switch (logType)
            {
                case LogType.Error: return LogEventLevel.Error;
                case LogType.Assert: return LogEventLevel.Warning;
                case LogType.Warning: return LogEventLevel.Warning;
                case LogType.Log: return LogEventLevel.Information;
                case LogType.Exception: return LogEventLevel.Error;
                default: return LogEventLevel.Information;
            }
        }
    }
}
