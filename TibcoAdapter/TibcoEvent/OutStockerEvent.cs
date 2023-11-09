using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class OutStockerEvent : TibcoEvent
    {
        public OutStockerEvent(string message) : base(TibcoEventType.OutStocker, message)
        {

        }

        public override string CarrierId => throw new NotImplementedException();

        public override string Goal => throw new NotImplementedException();

        public override string Port => throw new NotImplementedException();
    }
}
