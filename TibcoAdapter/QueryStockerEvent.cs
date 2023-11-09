using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class QueryStockerEvent : TibcoEvent
    {
        public QueryStockerEvent(string message) : base(TibcoEventType.QueryStocker, message)
        {
        }
    }
}
