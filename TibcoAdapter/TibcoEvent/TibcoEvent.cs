using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public abstract class TibcoEvent
    {
        public TibcoEvent(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
