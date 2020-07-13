using System;
using UnityEngine;

namespace Exanite.Arpg.Logging.Unity
{
    /// <summary>
    /// Intercepts Unity's Debug.Log messages and sends them to an <see cref="ILog"/>
    /// </summary>
    public class UnityDebugLogIntercepter : ILogHandler, IDisposable
    {
        private bool isActivated = false;
        private bool hasDisposed = false;

        private ILogHandler inner;

        private readonly ILog log;

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
        /// Creates a new <see cref="UnityDebugLogIntercepter"/>
        /// </summary>
        /// <param name="log"><see cref="ILog"/> to log to</param>
        public UnityDebugLogIntercepter(ILog log)
        {
            // Not ForContext<UnityDebugLogIntercepter> because this is so log messages from Unity have their context set properly
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
        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogLevel level = logType.ToLogLevel();

            log.Write(level, message);
        }

        /// <summary>
        /// Starts the interception of Unity Debug.Log messages
        /// </summary>
        public void Activate()
        {
            if (!IsActivated)
            {
                inner = Debug.unityLogger.logHandler;
                Debug.unityLogger.logHandler = this;

                IsActivated = true;

                log.ForContext<UnityDebugLogIntercepter>().Debug("Starting interception of Unity Debug.Log messages");
            }
        }

        /// <summary>
        /// Stops the interception of Unity Debug.Log messages<para/>
        /// Note: This is also called when this <see cref="UnityDebugLogIntercepter"/> is disposed
        /// </summary>
        public void Deactivate()
        {
            if (IsActivated)
            {
                Debug.unityLogger.logHandler = inner;
                inner = null;

                IsActivated = false;

                log.ForContext<UnityDebugLogIntercepter>().Debug("Stopping interception of Unity Debug.Log messages");
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
    }
}
