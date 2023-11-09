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
        }

        public override string CarrierId => throw new NotImplementedException();

        public override string Goal => throw new NotImplementedException();

        public override string Port => throw new NotImplementedException();
    }
}
