using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public enum StockerCommandType
    {
        InStocker,
        OutStocker
    }

    public class StockerEvent : TibcoTransferEvent
    {
        public StockerEvent(string message) : base(TibcoEventType.Stocker, message)
        {
            // TODO: Set Properties
        }

        public StockerCommandType CommandType { get; }

        public override string CarrierId => throw new NotImplementedException();

        public override string Goal => throw new NotImplementedException();

        public override string Port => throw new NotImplementedException();
    }
}
