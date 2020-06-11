﻿using System;
using System.IO;
using Exanite.Arpg.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;
using UnityEngine;
using Zenject;
using ILogger = Serilog.ILogger;
using Logger = Serilog.Core.Logger;

namespace Exanite.Arpg.Installers
{
    /// <summary>
    /// Installs a <see cref="Logger"/> to the Extenject DI container
    /// </summary>
    public class LogInstaller : MonoInstaller
    {
        [SerializeField] private bool logToFileInEditor = false;
        [SerializeField] private bool logToUnityConsole = true;
        [SerializeField] private bool includeTimestampInUnityConsole = false;
        [SerializeField] private string timestampFormat = "[{Timestamp:HH:mm:ss}]";
        [SerializeField] private string format = "[{Level}] [{ShortContext}]: {Message:lj}{NewLine}{Exception}";
        [SerializeField] private LogEventLevel minimumLevel = LogEventLevel.Information;

        /// <summary>
        /// Should the <see cref="Logger"/> log to file while in the Unity Editor?
        /// </summary>
        public bool LogToFileInEditor
        {
            get
            {
                return logToFileInEditor;
            }

            set
            {
                logToFileInEditor = value;
            }
        }

        /// <summary>
        /// Should the <see cref="Logger"/> log to the Unity Console?
        /// </summary>
        public bool LogToUnityConsole
        {
            get
            {
                return logToUnityConsole;
            }

            set
            {
                logToUnityConsole = value;
            }
        }

        /// <summary>
        /// Should the <see cref="Logger"/> include timestamps when logging to the Unity Console?<para/>
        /// Usually this should be off because Unity already provides timestamps for logged events
        /// </summary>
        public bool IncludeTimestampInUnityConsole
        {
            get
            {
                return includeTimestampInUnityConsole;
            }
            set
            {
                includeTimestampInUnityConsole = value;
            }
        }

        /// <summary>
        /// MessageTemplate format for timestamps
        /// </summary>
        public string TimestampFormat
        {
            get
            {
                return timestampFormat;
            }
            set
            {
                timestampFormat = value;
            }
        }

        /// <summary>
        /// MessageTemplate format for the logged event
        /// </summary>
        public string Format
        {
            get
            {
                return format;
            }
            set
            {
                format = value;
            }
        }

        /// <summary>
        /// Minimum level required for <see cref="LogEvent"/>s to be logged
        /// </summary>
        public LogEventLevel MinimumLevel
        {
            get
            {
                return minimumLevel;
            }

            set
            {
                minimumLevel = value;
            }
        }

        /// <summary>
        /// Installs the <see cref="Logger"/> to the Extenject DI container
        /// </summary>
        public override void InstallBindings()
        {
            Container.Bind(typeof(ILogger), typeof(IDisposable)).To<Logger>().FromMethod(CreateLogger).AsSingle().NonLazy();

            Container.Bind(typeof(UnityToSerilogLogHandler), typeof(IDisposable)).To<UnityToSerilogLogHandler>().AsSingle().NonLazy();
        }

        /// <summary>
        /// Creates a new <see cref="Logger"/> based off the provided settings
        /// </summary>
        private Logger CreateLogger(InjectContext ctx)
        {
            var config = new LoggerConfiguration()
                .Enrich.WithProperty("SourceContext", "Default")
                .Enrich.With<ShortContextEnricher>()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .MinimumLevel.Is(MinimumLevel);

            if (Application.isEditor && LogToFileInEditor)
            {
                string path = Path.GetFullPath(Path.Combine(Application.persistentDataPath, "Logs", $@"Log-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log"));

                WriteToFile(config, path);
            }

            if (LogToUnityConsole)
            {
                WriteToUnityConsole(config);
            }

            var log = config.CreateLogger();

            return log;
        }

        private void WriteToFile(LoggerConfiguration config, string path)
        {
            ITextFormatter fileFormatter = new MessageTemplateTextFormatter(string.Join(" ", TimestampFormat, Format));

            config.WriteTo.File(fileFormatter, path)
                .WriteTo.File(new JsonFormatter(), $"{path}.json");
        }

        private void WriteToUnityConsole(LoggerConfiguration config)
        {
            ITextFormatter unityConsoleFormatter = new MessageTemplateTextFormatter(string.Join((IncludeTimestampInUnityConsole ? TimestampFormat : null), Format));

            config.WriteTo.Sink(new UnityConsoleSink(Debug.unityLogger.logHandler, unityConsoleFormatter));
        }

    }
}
