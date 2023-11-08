using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class LoadPortEvent : TibcoEvent
    {
        public LoadPortEvent(string message) : base(TibcoEventType.LoadPort, message)
        {
            // TODO: Set Properties
            // Eqp
            // Port
            // IsReadyToLoad
            // IsReadyToUnload
            // IsLoadComplete
            // IsUnloadComplete
        }

        public string Eqp { get; }

        public string Port { get; }

        public bool IsReadyToLoad { get; }

        public bool IsReadyToUnload { get; }

        public bool IsLoadComplete { get; }

        public bool IsUnloadComplete { get; }
    }
}
