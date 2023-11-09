using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public enum TibcoEventType
    {
        JobPrepare = 1,
        Stocker = 2,
        LoadPort = 3,
        QueryJobPrepare = 4,
        QueryStocker = 5,
        QueryLoadPort = 6,
    }
}
