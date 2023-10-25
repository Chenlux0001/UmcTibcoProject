using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoMcsLite
{
    public enum LoadPortStatus
    {
        ReadyToUnload,
        ReadyToLoad,
        TransferBlocked,
        OutofService
    }
}
