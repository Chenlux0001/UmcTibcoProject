using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public abstract class TibcoTransferEvent : TibcoEvent
    {
        public TibcoTransferEvent(TibcoEventType eventType, string message) : base(eventType, message)
        {
        }

        public abstract string CarrierId { get; }

        public abstract string Goal { get; }

        public abstract string Port { get; }
    }
}
