using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TibcoAGVC
{
    public class AppLoggerViewModel
    {
        public AppLoggerViewModel()
        {
            LoggerEventCollection = new ObservableCollection<MessageLoggerEvent>();

            LoggerEventDispatcher.Subscribe(e => e is MessageLoggerEvent, e => ProcessLoggerEvent(e as MessageLoggerEvent));
        }

        public ObservableCollection<MessageLoggerEvent> LoggerEventCollection { get; }

        private bool ProcessLoggerEvent(MessageLoggerEvent messageLoggerEvent)
        {
            if (Application.Current == null)
                return false;

            Application.Current.Dispatcher.Invoke(DispatcherPriority.Send, new Action(() =>
            {
                LoggerEventCollection.Add(messageLoggerEvent);

                if (LoggerEventCollection.Count > 1000)
                    LoggerEventCollection.RemoveAt(0);
            }));

            return true;
        }
    }
}
