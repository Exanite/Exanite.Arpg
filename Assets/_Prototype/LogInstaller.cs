using System;
using System.IO;
using Serilog;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using Serilog.Formatting.Json;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;
using ILogger = Serilog.ILogger;
using Logger = Serilog.Core.Logger;

namespace Prototype
{
    public class LogInstaller : MonoInstaller
    {
        [SerializeField, HideInInspector] private bool logToUnityConsole = true;
        [SerializeField, HideInInspector] private bool includeTimeStampInUnityConsole = false;
        [SerializeField, HideInInspector] private string timestampFormat = "[{Timestamp:HH:mm:ss}]";
        [SerializeField, HideInInspector] private string format = "[{Level}] [{SourceContext}]: {Message:lj}{NewLine}{Exception}";
        [SerializeField, HideInInspector] private LogEventLevel minimumLevel = LogEventLevel.Information;

        [ShowInInspector, DisableInPlayMode]
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

        [ShowInInspector, DisableInPlayMode]
        public bool IncludeTimeStampInUnityConsole
        {
            get
            {
                return includeTimeStampInUnityConsole;
            }
            set
            {
                includeTimeStampInUnityConsole = value;
            }
        }

        [ShowInInspector, DisableInPlayMode]
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

        [ShowInInspector, DisableInPlayMode]
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

        [ShowInInspector, DisableInPlayMode]
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

        public override void InstallBindings()
        {
            Container.Bind(typeof(ILogger), typeof(IDisposable)).To<Logger>().FromMethod(CreateLogger).AsSingle().NonLazy();

            Container.Bind(typeof(UnityToSerilogLogHandler), typeof(IDisposable)).To<UnityToSerilogLogHandler>().AsSingle().NonLazy();
        }

        public Logger CreateLogger(InjectContext ctx)
        {
            string path = Path.GetFullPath(Path.Combine(Application.persistentDataPath, "Logs", $@"Log-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log"));

            ITextFormatter fileFormatter = new MessageTemplateTextFormatter($"{TimestampFormat} {Format}");
            ITextFormatter unityConsoleFormatter = new MessageTemplateTextFormatter($"{(IncludeTimeStampInUnityConsole ? TimestampFormat : string.Empty)} {Format}");

            Logger log = new LoggerConfiguration()
                .Enrich.WithProperty("SourceContext", "Default")
                .Enrich.With<ShortContextEnricher>()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .MinimumLevel.Is(MinimumLevel)
                .WriteTo.File(fileFormatter, path)
                .WriteTo.File(new JsonFormatter(), $"{path}.json")
                .WriteTo.Sink(LogToUnityConsole ? new UnityConsoleSink(Debug.unityLogger.logHandler, unityConsoleFormatter) : null)
                .CreateLogger();

            var logContext = log.ForContext<LogInstaller>();

            logContext.Information("Initializing Logger");
            logContext.Information("Logging events to {Path}", path);

            return log;
        }
    }
}
