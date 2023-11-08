using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class InStockerEvent : TibcoEvent
    {
        public InStockerEvent(string message) : base(TibcoEventType.InStocker, message)
        {
            // TODO: Set Properties
        }

        public string Stocker { get; }

        public string Port { get; }
    }
}
