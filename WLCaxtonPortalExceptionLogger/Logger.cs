//==========================================================================================
// Project              : WLCaxtonWebPortal
// File Name            : Logger.cs
// Program Description  : This class is used to log the errors.
// Programmed By        : Satyendra Gupta
// Programmed On        : 10-December-2012 
// Version              : 1.0.0
//==========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace WLCaxtonPortalExceptionLogger
{
    /// <summary>
    /// Log event types
    /// </summary>
    public enum LogEventType
    {
        Debug,
        Error,
        Fatal,
        Info,
        Warn
    }

    /// <summary>
    /// Our logger interface - by working to an interface we eliminate hard-wired dependencies
    /// on external logging frameworks
    /// </summary>
    public interface ILogger
    {
        void Log(Type sourceType, LogEventType eventType, string message, System.Exception exception);
        void Log(Type sourceType, LogEventType eventType, string message);
        void Log(string sourceType, LogEventType eventType, string message, System.Exception exception);
        void Log(string sourceType, LogEventType eventType, string message);
    }

    /// <summary>
    /// Logger factory - singleton class to construct and return a logger implementation
    /// </summary>
    public static class LoggerFactory
    {

        static ILogger _logger;

        public static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = new Log4NetLoggerWrapper();
                }
                return _logger;
            }
        }

    }

    /// <summary>
    /// Implements the ILogger interface for the Log4Net framework - internal constructor
    /// prevents direct construction outside of assembly/factory
    /// </summary>
    public class Log4NetLoggerWrapper : ILogger
    {

        internal Log4NetLoggerWrapper() { }

        public void Log(Type sourceType, LogEventType eventType, string message, Exception exception)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(sourceType);
            log(logger, eventType, message, exception);
        }

        public void Log(Type sourceType, LogEventType eventType, string message)
        {
            Log(sourceType, eventType, message, null);
        }

        public void Log(string sourceType, LogEventType eventType, string message, Exception exception)
        {
            log4net.ILog logger = log4net.LogManager.GetLogger(sourceType);
            log(logger, eventType, message, exception);

        }
        public void Log(string sourceType, LogEventType eventType, string message)
        {
            Log(sourceType, eventType, message, null);
        }

        private void log(log4net.ILog logger,LogEventType eventType, string message, Exception exception)
        {
            switch (eventType)
            {
                case LogEventType.Debug:
                    if (exception != null)
                    {
                        logger.Debug(message, exception);
                    }
                    else
                    {
                        logger.Debug(message);
                    }
                    break;
                case LogEventType.Error:
                    if (exception != null)
                    {
                        logger.Error(message, exception);
                    }
                    else
                    {
                        logger.Error(message);
                    }
                    break;
                case LogEventType.Fatal:
                    if (exception != null)
                    {
                        logger.Fatal(message, exception);
                    }
                    else
                    {
                        logger.Fatal(message);
                    }
                    break;
                case LogEventType.Info:
                    if (exception != null)
                    {
                        logger.Info(message, exception);
                    }
                    else
                    {
                        logger.Info(message);
                    }
                    break;
                case LogEventType.Warn:
                    if (exception != null)
                    {
                        logger.Warn(message, exception);
                    }
                    else
                    {
                        logger.Warn(message);
                    }
                    break;
            }
        }

    }
}


