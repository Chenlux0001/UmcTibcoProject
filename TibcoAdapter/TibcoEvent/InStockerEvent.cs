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

        public override string CarrierId => throw new NotImplementedException();

        public override string Goal => throw new NotImplementedException();

        public override string Port => throw new NotImplementedException();
    }
}
