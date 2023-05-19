using System;
using System.Text;

namespace Build1.PostMVC.Unity.App.Modules.Logging.Impl
{
    internal abstract class LogBase : ILog, ILogDebug, ILogWarn, ILogError
    {
        private string   _prefix;
        private LogLevel _level;

        protected readonly ILogController _logController;

        protected LogBase(string prefix, LogLevel mode, ILogController logController)
        {
            _prefix = prefix;
            _level = mode;
            _logController = logController;
        }

        /*
         * Debug.
         */

        public abstract void Debug(string message);
        public abstract void Debug(Exception exception);
        public abstract void Debug(Func<string> callback);
        public abstract void Debug<T1>(Func<T1, string> callback, T1 param01);
        public abstract void Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        public abstract void Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        public abstract void Debug(Action<ILogDebug> callback);
        public abstract void Debug<T1>(Action<ILogDebug, T1> callback, T1 param01);
        public abstract void Debug<T1, T2>(Action<ILogDebug, T1, T2> callback, T1 param01, T2 param02);
        public abstract void Debug<T1, T2, T3>(Action<ILogDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Warn.
         */

        public abstract void Warn(string message);
        public abstract void Warn(Exception exception);
        public abstract void Warn(Func<string> callback);
        public abstract void Warn<T1>(Func<T1, string> callback, T1 param01);
        public abstract void Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        public abstract void Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        public abstract void Warn(Action<ILogWarn> callback);
        public abstract void Warn<T1>(Action<ILogWarn, T1> callback, T1 param01);
        public abstract void Warn<T1, T2>(Action<ILogWarn, T1, T2> callback, T1 param01, T2 param02);
        public abstract void Warn<T1, T2, T3>(Action<ILogWarn, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Error.
         */

        public abstract void Error(string message);
        public abstract void Error(Exception exception);
        public abstract void Error(Func<string> callback);
        public abstract void Error<T1>(Func<T1, string> callback, T1 param01);
        public abstract void Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        public abstract void Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        public abstract void Error(Action<ILogError> callback);
        public abstract void Error<T1>(Action<ILogError, T1> callback, T1 param01);
        public abstract void Error<T1, T2>(Action<ILogError, T1, T2> callback, T1 param01, T2 param02);
        public abstract void Error<T1, T2, T3>(Action<ILogError, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Public.
         */

        public void SetPrefix(string prefix)
        {
            _prefix = prefix;
        }

        public void SetLevel(LogLevel level)
        {
            _level = level;
        }

        public void Disable()
        {
            SetLevel(LogLevel.None);
        }

        /*
         * Protected.
         */

        protected bool   CheckLevel(LogLevel level)           { return (_level > LogLevel.None && level >= _level) || Logging.ForceAll; }
        protected string FormatMessage(string message)        { return _prefix != null ? $"{_prefix}: {message}\n" : message.ToString(); }
        protected string FormatException(Exception exception) { return FormatException(_prefix, exception); }

        /*
         * Static.
         */

        public static string FormatException(string prefix, Exception exception)
        {
            if (exception.InnerException == null)
            {
                return prefix != null
                           ? $"{prefix}: {exception.GetType().Name}: {exception.Message}\n"
                           : $"{exception.GetType().Name}: {exception.Message}\n";
            }

            var builder = prefix != null 
                              ? new StringBuilder($"{prefix}: {FormatExceptionNoInner(exception)}")
                              : new StringBuilder(FormatExceptionNoInner(exception));

            var innerException = exception.InnerException;
            while (innerException != null)
            {
                builder.AppendLine("");

                foreach (var line in FormatExceptionNoInner(innerException).Split('\n'))
                    builder.AppendLine($"        {line}");

                innerException = innerException.InnerException;
            }

            return builder.ToString();
        }

        private static string FormatExceptionNoInner(Exception exception)
        {
            return $"{exception.GetType().Name}: {exception.Message} at \n{exception.StackTrace}\n";
        }
    }
}