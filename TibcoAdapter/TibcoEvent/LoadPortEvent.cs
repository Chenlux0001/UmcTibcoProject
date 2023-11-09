using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public enum LoadPortCommandType
    {
        ReadyToLoad,
        ReadyToUnload,
        LoadComplete,
        UnloadComplete
    }

    public class LoadPortEvent : TibcoTransferEvent
    {
        public LoadPortEvent(string message) : base(TibcoEventType.LoadPort, message)
        {
            // TODO: Set Properties
        }

        public LoadPortCommandType CommandType { get; }

        public override string CarrierId => throw new NotImplementedException();

        public override string Goal => throw new NotImplementedException();

        public override string Port => throw new NotImplementedException();
    }
}
