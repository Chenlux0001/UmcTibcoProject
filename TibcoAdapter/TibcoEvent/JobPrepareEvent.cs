using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class JobPrepareEvent : TibcoEvent
    {
        public JobPrepareEvent(string message) : base(TibcoEventType.JobPrepare, message)
        {
        }

        public PrepareTransfer ToPrepareTransfer()
        {
            // TODO:

            return null;
        }
    }
}
