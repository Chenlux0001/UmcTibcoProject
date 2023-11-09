using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class QueryJobPrepareEvent : TibcoEvent
    {
        public QueryJobPrepareEvent(string message) : base(TibcoEventType.QueryJobPrepare, message)
        {
        }
    }
}
