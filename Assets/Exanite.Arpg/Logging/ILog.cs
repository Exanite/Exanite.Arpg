namespace Exanite.Arpg.Logging
{
    /// <summary>
    /// Common interface for Loggers
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Create a logger that enriches LogEntries with the specified property
        /// </summary>
        ILog ForContext(string property, object value);

        /// <summary>
        /// Determine if events of the specified level will be logged
        /// </summary>
        bool IsEnabled(LogLevel level);

        /// <summary>
        /// Write a <see cref="LogEntry"/>
        /// </summary>
        void Log(LogEntry entry);
    } 
}
