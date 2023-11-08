using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public abstract class TibcoEvent
    {
        public TibcoEvent(TibcoEventType eventType, string message)
        {
            Message = message;
            EventType = eventType;
        }

        public TibcoEventType EventType { get; }

        public string Message { get; }
    }
}
