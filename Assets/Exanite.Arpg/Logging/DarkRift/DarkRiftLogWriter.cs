using System;
using DarkRift.Server;
using Zenject;

namespace Exanite.Arpg.Logging.DarkRift
{
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
            log.Write(args.LogType.ToLogLevel(), args.Exception, args.Message);
        }
    } 
}
