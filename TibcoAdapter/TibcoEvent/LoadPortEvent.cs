using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class LoadPortEvent : TibcoEvent
    {
        public LoadPortEvent(string message) : base(message)
        {
        }
    }
}
