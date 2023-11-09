using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class QueryLoadPortEvent : TibcoEvent
    {
        public QueryLoadPortEvent(string message) : base(TibcoEventType.QueryLoadPort, message)
        {
        }
    }
}
