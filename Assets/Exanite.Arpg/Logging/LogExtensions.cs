using System;

namespace Exanite.Arpg.Logging
{
    public static class LogExtensions
    {
        private static readonly object[] NoPropertyValues = new object[0];

        public static ILog ForContext<TSource>(this ILog log)
        {
            return log.ForContext(typeof(TSource));
        }

        public static ILog ForContext(this ILog log, Type source)
        {
            return log.ForContext(Constants.SourceContextPropertyName, source.FullName);
        }

        public static void Debug(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Debug, messageTemplate);
        }

        public static void Debug<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Debug, messageTemplate, propertyValue);
        }

        public static void Debug<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Debug, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Debug<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Debug, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Debug(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Debug, messageTemplate, propertyValues);
        }

        public static void Debug(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate);
        }

        public static void Debug<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate, propertyValue);
        }

        public static void Debug<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Debug<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Debug(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Debug, exception, messageTemplate, propertyValues);
        }

        public static void Error(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Error, messageTemplate);
        }

        public static void Error<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Error, messageTemplate, propertyValue);
        }

        public static void Error<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Error, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Error<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Error, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Error(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Error, messageTemplate, propertyValues);
        }

        public static void Error(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Error, exception, messageTemplate);
        }

        public static void Error<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Error, exception, messageTemplate, propertyValue);
        }

        public static void Error<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Error<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Error, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Error(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Error, exception, messageTemplate, propertyValues);
        }

        public static void Fatal(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Fatal, messageTemplate);
        }

        public static void Fatal<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Fatal, messageTemplate, propertyValue);
        }

        public static void Fatal<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Fatal, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Fatal<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Fatal, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Fatal(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Fatal, messageTemplate, propertyValues);
        }

        public static void Fatal(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate);
        }

        public static void Fatal<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate, propertyValue);
        }

        public static void Fatal<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Fatal<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Fatal(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Fatal, exception, messageTemplate, propertyValues);
        }

        public static void Information(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Information, messageTemplate);
        }

        public static void Information<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Information, messageTemplate, propertyValue);
        }

        public static void Information<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Information, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Information<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Information, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Information(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Information, messageTemplate, propertyValues);
        }

        public static void Information(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Information, exception, messageTemplate);
        }

        public static void Information<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Information, exception, messageTemplate, propertyValue);
        }

        public static void Information<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Information<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Information, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Information(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Information, exception, messageTemplate, propertyValues);
        }

        public static void Verbose(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Verbose, messageTemplate);
        }

        public static void Verbose<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Verbose, messageTemplate, propertyValue);
        }

        public static void Verbose<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Verbose, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Verbose<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Verbose, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Verbose(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Verbose, messageTemplate, propertyValues);
        }

        public static void Verbose(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate);
        }

        public static void Verbose<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate, propertyValue);
        }

        public static void Verbose<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Verbose<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Verbose(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Verbose, exception, messageTemplate, propertyValues);
        }

        public static void Warning(this ILog log, string messageTemplate)
        {
            log.Write(LogLevel.Warning, messageTemplate);
        }

        public static void Warning<T>(this ILog log, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Warning, messageTemplate, propertyValue);
        }

        public static void Warning<T0, T1>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Warning, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Warning<T0, T1, T2>(this ILog log, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Warning, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Warning(this ILog log, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Warning, messageTemplate, propertyValues);
        }

        public static void Warning(this ILog log, Exception exception, string messageTemplate)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate);
        }

        public static void Warning<T>(this ILog log, Exception exception, string messageTemplate, T propertyValue)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate, propertyValue);
        }

        public static void Warning<T0, T1>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1);
        }

        public static void Warning<T0, T1, T2>(this ILog log, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate, propertyValue0, propertyValue1, propertyValue2);
        }

        public static void Warning(this ILog log, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            log.Write(LogLevel.Warning, exception, messageTemplate, propertyValues);
        }

        public static void Write(this ILog log, LogLevel level, string messageTemplate)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, NoPropertyValues);
            }
        }

        public static void Write<T>(this ILog log, LogLevel level, string messageTemplate, T propertyValue)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, new object[] { propertyValue });
            }
        }

        public static void Write<T0, T1>(this ILog log, LogLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, new object[] { propertyValue0, propertyValue1 });
            }
        }

        public static void Write<T0, T1, T2>(this ILog log, LogLevel level, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, new object[] { propertyValue0, propertyValue1, propertyValue2 });
            }
        }

        public static void Write(this ILog log, LogLevel level, string messageTemplate, params object[] propertyValues)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, (Exception)null, messageTemplate, propertyValues);
            }
        }

        public static void Write(this ILog log, LogLevel level, Exception exception, string messageTemplate)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, exception, messageTemplate, NoPropertyValues);
            }
        }

        public static void Write<T>(this ILog log, LogLevel level, Exception exception, string messageTemplate, T propertyValue)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, exception, messageTemplate, new object[] { propertyValue });
            }
        }

        public static void Write<T0, T1>(this ILog log, LogLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, exception, messageTemplate, new object[] { propertyValue0, propertyValue1 });
            }
        }

        public static void Write<T0, T1, T2>(this ILog log, LogLevel level, Exception exception, string messageTemplate, T0 propertyValue0, T1 propertyValue1, T2 propertyValue2)
        {
            if (log.IsEnabled(level))
            {
                log.Write(level, exception, messageTemplate, new object[] { propertyValue0, propertyValue1, propertyValue2 });
            }
        }

        public static void Write(this ILog log, LogLevel level, Exception exception, string messageTemplate, params object[] propertyValues)
        {
            if (log.IsEnabled(level) && messageTemplate != null)
            {
                log.Log(new LogEntry(level, exception, messageTemplate, propertyValues));
            }
        }
    }
}
