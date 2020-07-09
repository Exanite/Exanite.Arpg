using System;
using DarkRift;
using DarkRift.Server;
using Exanite.Arpg.Logging;
using Zenject;

public class DarkRiftLogWriter : LogWriter
{
    private ILog log;

    public DarkRiftLogWriter(LogWriterLoadData logWriterLoadData) : base(logWriterLoadData) { }

    public override Version Version
    {
        get
        {
            return new Version(1, 0, 0);
        }
    }

    [Inject]
    public void Inject(ILog log)
    {
        this.log = log.ForContext("SourceContext", "DarkRift");
    }

    public override void WriteEvent(WriteEventArgs args)
    {
        log.Write(ConvertToLogLevel(args.LogType), args.Exception, args.Message);
    }

    public LogLevel ConvertToLogLevel(LogType type)
    {
        switch (type)
        {
            case LogType.Trace:
            {
                return LogLevel.Debug;
            }
            case LogType.Info:
            {
                return LogLevel.Information;
            }
            case LogType.Warning:
            {
                return LogLevel.Warning;
            }
            case LogType.Error:
            {
                return LogLevel.Error;
            }
            case LogType.Fatal:
            {
                return LogLevel.Fatal;
            }
            default:
            {
                throw new NotSupportedException($"{type} is not a supported {typeof(LogType).Name}");
            }
        }
    }
}
