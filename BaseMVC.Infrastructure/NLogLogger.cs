using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NLog;

namespace BaseMVC.Infrastructure
{
    public class NLogLogger : IInternalLogger
    {
        private static readonly NLog.Logger _logger = LogManager.GetCurrentClassLogger();

        #region IInternalLogger Members

        #region Properties

        public bool IsDebugEnabled { get { return _logger.IsDebugEnabled; } }

        public bool IsErrorEnabled { get { return _logger.IsErrorEnabled; } }

        public bool IsFatalEnabled { get { return _logger.IsFatalEnabled; } }

        public bool IsInfoEnabled { get { return _logger.IsInfoEnabled; } }

        public bool IsWarnEnabled { get { return _logger.IsWarnEnabled; } }

        #endregion

        #region IInternalLogger Methods

        public void Debug(object message, Exception exception)
        {
            if (IsDebugEnabled)
                _logger.DebugException(message.ToString(), exception);
        }

        public void Debug(object message)
        {
            if (IsDebugEnabled)
                _logger.Debug(message.ToString());
        }

        public void DebugFormat(string format, params object[] args)
        {
            if (IsDebugEnabled)
                _logger.Debug(String.Format(format, args));
        }

        public void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
                _logger.ErrorException(message.ToString(), exception);
        }

        public void Error(object message)
        {
            if (IsErrorEnabled)
                _logger.Error(message.ToString());
        }

        public void ErrorFormat(string format, params object[] args)
        {
            if (IsErrorEnabled)
                _logger.Error(String.Format(format, args));
        }

        public void Fatal(object message, Exception exception)
        {
            if (IsFatalEnabled)
                _logger.FatalException(message.ToString(), exception);
        }

        public void Fatal(object message)
        {
            if (IsFatalEnabled)
                _logger.Fatal(message.ToString());
        }

        public void Info(object message, Exception exception)
        {
            if (IsInfoEnabled)
                _logger.InfoException(message.ToString(), exception);
        }

        public void Info(object message)
        {
            if (IsInfoEnabled)
                _logger.Info(message.ToString());
        }

        public void InfoFormat(string format, params object[] args)
        {
            if (IsInfoEnabled)
                _logger.Info(String.Format(format, args));
        }

        public void Warn(object message, Exception exception)
        {
            if (IsWarnEnabled)
                _logger.WarnException(message.ToString(), exception);
        }

        public void Warn(object message)
        {
            if (IsWarnEnabled)
                _logger.Warn(message.ToString());
        }

        public void WarnFormat(string format, params object[] args)
        {
            if (IsWarnEnabled)
                _logger.Warn(String.Format(format, args));
        }

        #endregion

        #endregion

        #region Private methods

        #endregion
    }
}
