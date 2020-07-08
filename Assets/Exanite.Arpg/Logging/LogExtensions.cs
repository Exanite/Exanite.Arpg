using System;

namespace Exanite.Arpg.Logging
{
    /// <summary>
    /// Contains extensions to the <see cref="ILog"/> interface so that implementers of the interface will not have to implement the following members
    /// </summary>
    public static class LogExtensions
    {
        private static readonly object[] NoPropertyValues = new object[0];

        /// <summary>
        /// Create a logger that marks LogEntries as being from the specified source
        /// </summary>
        public static ILog ForContext<TSource>(this ILog log)
        {
            return log.ForContext(typeof(TSource));
        }

        /// <summary>
        /// Create a logger that marks LogEntries as being from the specified source
        /// </summary>
        public static ILog ForContext(this ILog log, Type source)
        {
            return log.ForContext(Constants.SourceContextPropertyName, source.FullName);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Debug(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Debug, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Debug<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Debug, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Debug<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Debug, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Debug<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Debug, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Debug(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Debug, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Debug(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Debug<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Debug<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Debug<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Debug"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Debug(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Error(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Error, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Error<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Error, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Error<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Error, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Error<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Error, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Error(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Error, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Error(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Error, exception, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Error<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Error, exception, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Error<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Error<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Error"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Error(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Error, exception, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Fatal(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Fatal, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Fatal<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Fatal, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Fatal<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Fatal, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Fatal<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Fatal, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Fatal(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Fatal, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Fatal(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Fatal<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Fatal<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Fatal<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Fatal"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Fatal(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Information(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Information, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Information<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Information, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Information<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Information, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Information<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Information, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Information(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Information, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Information(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Information, exception, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Information<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Information, exception, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Information<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Information<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Information"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Information(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Information, exception, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Verbose(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Verbose, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Verbose<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Verbose, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Verbose<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Verbose, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Verbose<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Verbose, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Verbose(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Verbose, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Verbose(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Verbose<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Verbose<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Verbose<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Verbose"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Verbose(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Warning(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Warning, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Warning<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Warning, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Warning<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Warning, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Warning<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Warning, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Warning(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Warning, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Warning(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Warning<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate, propertyValue);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Warning<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Warning<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the <see cref="LogLevel.Warning"/> level and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Warning(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate, propertyValues);
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/>
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Write(this ILog log, LogLevel level, string messageTemplate)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, NoPropertyValues);
            }
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/>
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Write<T>(this ILog log, LogLevel level, string messageTemplate, T propertyValue)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, new object[] { propertyValue });
            }
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/>
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Write<T0, T1>(this ILog log, LogLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, new object[] { propertyValue0, propertyValue1 });
            }
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/>
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Write<T0, T1, T2>(this ILog log, LogLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, new object[] { propertyValue0, propertyValue1, propertyValue2 });
            }
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/>
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Write(this ILog log, LogLevel level, string messageTemplate, params object[] propertyValues)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, propertyValues);
            }
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/> and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        public static void Write(this ILog log, LogLevel level, Exception exception, string messageTemplate)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, exception, messageTemplate, NoPropertyValues);
            }
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/> and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue">Object positionally formatted into the message template</param>
        public static void Write<T>(this ILog log, LogLevel level, Exception exception, string messageTemplate, T propertyValue)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, exception, messageTemplate, new object[] { propertyValue });
            }
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/> and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        public static void Write<T0, T1>(this ILog log, LogLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, exception, messageTemplate, new object[] { propertyValue0, propertyValue1 });
            }
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/> and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValue0">Object positionally formatted into the message template</param>
        /// <param name="propertyValue1">Object positionally formatted into the message template</param>
        /// <param name="propertyValue2">Object positionally formatted into the message template</param>
        public static void Write<T0, T1, T2>(this ILog log, LogLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, exception, messageTemplate, new object[] { propertyValue0, propertyValue1, propertyValue2 });
            }
        }

        /// <summary>
        /// Write a <see cref="LogEntry"/> with the specified <see cref="LogLevel"/> and associated exception
        /// </summary>
        /// <param name="log">Log to write to</param>
        /// <param name="level">The level of the entry</param>
        /// <param name="exception">Exception related to the entry</param>
        /// <param name="messageTemplate">Message template describing the entry</param>
        /// <param name="propertyValues">Objects positionally formatted into the message template</param>
        public static void Write(this ILog log, LogLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (log.IsEnabled(level) && messageTemplate != null)
            {
                log.Log(new LogEntry(level, exception, messageTemplate, propertyValues));
            }
        }
    }
}
