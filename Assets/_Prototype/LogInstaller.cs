using System;
using System.IO;
using Serilog;
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
        [SerializeField, HideInInspector] private bool includeTimeStampInConsole = false;
        [SerializeField, HideInInspector] private string timestampFormat = "[{Timestamp:HH:mm:ss}]";
        [SerializeField, HideInInspector] private string format = "[{Level}] [{SourceContext}]: {Message:lj}{NewLine}{Exception}";

        [ShowInInspector, DisableInPlayMode]
        public bool IncludeTimeStampInConsole
        {
            get
            {
                return includeTimeStampInConsole;
            }
            set
            {
                includeTimeStampInConsole = value;
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

        public override void InstallBindings()
        {
            Container.Bind(typeof(ILogger), typeof(IDisposable)).To<Logger>().FromMethod(CreateLogger).AsSingle().NonLazy();

            Container.Bind(typeof(UnityToSerilogLogHandler), typeof(IDisposable)).To<UnityToSerilogLogHandler>().AsSingle().NonLazy();
        }

        public Logger CreateLogger(InjectContext ctx)
        {
            string path = Path.GetFullPath(Path.Combine(Application.persistentDataPath, "Logs", $@"Log-{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.log"));

            ITextFormatter fileFormatter = new MessageTemplateTextFormatter($"{TimestampFormat} {Format}");
            ITextFormatter unityConsoleFormatter = new MessageTemplateTextFormatter($"{(IncludeTimeStampInConsole ? TimestampFormat : string.Empty)} {Format}");

            Logger log = new LoggerConfiguration()
                .Enrich.WithProperty("SourceContext", "Default")
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .MinimumLevel.Verbose()
                .WriteTo.File(fileFormatter, path)
                .WriteTo.File(new JsonFormatter(), $"{path}.json")
                .WriteTo.Sink(new UnityConsoleSink(Debug.unityLogger.logHandler, unityConsoleFormatter))
                .CreateLogger();

            var logContext = log.ForContext<LogInstaller>();

            logContext.Information("Initializing Logger");
            logContext.Information("Logging events to {Path}", path);

            return log;
        }
    }
}
