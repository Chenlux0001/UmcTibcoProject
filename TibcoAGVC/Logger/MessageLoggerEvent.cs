using JxSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAGVC
{
    public enum MessageLevel
    {
        Info,
        Warn,
        Error,
    }

    public class MessageLoggerEvent : JxEventBase
    {
        public MessageLoggerEvent(string message, MessageLevel messageLevel) : base(null)
        {
            Message = message;
            Timestamp = DateTime.Now;
            MessageLevel = messageLevel;
        }

        public DateTime Timestamp { get; }

        public string Message { get; }

        public MessageLevel MessageLevel { get; }
    }
}
