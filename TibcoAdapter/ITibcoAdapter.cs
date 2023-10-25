using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public interface ITibcoAdapter
    {
        #region EAP (LoadPort)

        event EventHandler<string> OnListenLoadPortEvent;

        string QueryLoadPortEvent();

        #endregion

        #region MCS (Stocker)

        event EventHandler<string> OnListenOutStockerEvent;

        string QueryOutStockerEvent();

        #endregion

        #region MES

        event EventHandler<string> OnListenJobPrepareEvent;

        string QueryJobPrepareEvent();

        #endregion
    }
}
