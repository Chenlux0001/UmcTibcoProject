using JxSystem.Core;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TibcoAGVC
{
    public class LoggerEventDispatcher
    {
        private static readonly Logger APP_LOGGER = LogManager.GetLogger("AppLogger");

        private static JxEventDispatcher eventDispatcher = new JxEventDispatcher(100);

        public static JxEventSubscription Subscribe(Predicate<JxEventBase> selector, Action<JxEventBase> callback)
        {
            return eventDispatcher.Subscribe(selector, callback);
        }

        public static void Unsubscribe(JxEventSubscription eventSubscription)
        {
            eventDispatcher.Unsubscribe(eventSubscription);
        }

        public static void Info(string message)
        {
            APP_LOGGER.Info(message);

            eventDispatcher.RaiseEvent(new MessageLoggerEvent(message, MessageLevel.Info));
        }

        public static void Warn(string message)
        {
            APP_LOGGER.Warn(message);

            eventDispatcher.RaiseEvent(new MessageLoggerEvent(message, MessageLevel.Warn));
        }

        public static void Error(string message)
        {
            APP_LOGGER.Error(message);

            eventDispatcher.RaiseEvent(new MessageLoggerEvent(message, MessageLevel.Error));
        }

        public static void Start()
        {
            eventDispatcher.Start();
        }

        public static void Stop()
        {
            eventDispatcher.Stop();
        }
    }
}
