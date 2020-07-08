namespace Exanite.Arpg.Logging
{
    public interface ILog
    {
        ILog ForContext(string property, object value);

        bool IsEnabled(LogLevel level);

        void Log(LogEntry entry);
    } 
}
