using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoMcsLite
{
    public enum StockerStatus
    {
        InStocker,
        InTransit,
        InFab,
        InEqp,
        EqpOnInOutPort
    }
}
