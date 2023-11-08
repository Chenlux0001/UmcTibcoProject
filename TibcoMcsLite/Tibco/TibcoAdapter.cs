using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibcoAdapter;

namespace TibcoMcsLite
{
    public class TibcoAdapter : ITibcoAdapter
    {
        public event EventHandler<string> OnListenLoadPortEvent;
        public event EventHandler<string> OnListenStockerEvent;
        public event EventHandler<string> OnListenJobPrepareEvent;

        public string QueryJobPrepareEvent()
        {
            return ">>L EQPMLJOBPREP msgTag=replyBox.5969Dummy { class=ORDERED { class=ASSOC { class=A1 \"PARAMETER\" } { class=A1 \"VALUE\" } } { \"msgTag\" \"EQP - 01.FWsrv,replyBox.5969Dummy\" } { \"CMD\" \"EQPMLJOBPREP\" } { \"TID\" \"UAM_CuCmpSeasoner_23964\" } { \"USERID\" \"AM_PULL\" } { \"EQPID\" \"EQP - 01\" } { \"BATCHID\" \"AM_PULL_BN3815U_80079\" } { \"BATCHSIZE\" \"1\" } { \"SLOTINFOHEADER\" \"SLOTID: LOTID: COMPONENTID: WAFERID: LOTTYPE: TRACKINGUNIT: PPID: RETICLEID: PRODUCTID: PLANID: PLANTYPE: SUBPLANID: SUBPLANTYPE: STEPSEQ: STEPID: STEPTYPE: PPPARAMETER: ISMONITOR: PROCESSINGSTATE: PROCESSINGSTATUS: ISENGJIP: PRIORITY: STARTDATE: ANGLE\" } { \"COMMENT\" \"(Job Prepared by AutoSeasoner at 01 / 26 15:41:53)[SK6PM.29 - Ranking - NoNextLot]\" } { \"M - EQP - 01 - 063\" \"PLANID = MT - CUCMP - DUMMY.0 STEPDESC = DUMMY STAGENAME = \" } { \"BN3815U\" \"PORTID = EQP - 01 - 3 LOTIDS = M - EQP - 01 - 063 RFTAG = \" } { \"AM_PULL_BN3815U_80079\" \"BN3815U\" } }";
        }

        public string QueryLoadPortEvent()
        {
            string readToLoad = ">>L FwSrvExecuteRuleTxn msgTag=FWSRVdefault ruleName=EAPRTLEVENTREADYTOLOAD attributes={ { class=ASSOC { class=A1 \"PARAMETER\" } { class=A1 \"Value\" } } { \"CMD\" \"EAPEVENTREADYTOLOAD\" } { \"TID\" \"21681\" } { \"USERID\" \"AUTO\" } { \"CARRIERID\" \"\" } { \"LOTID\" \"\" } { \"EQPID\" \"EQP - 01\" } { \"PORTID\" \"EQP - 01 - 1\" } { \"COMMENT\" \"\" } }";
            //string loadComplete = ">>L FwSrvExecuteRuleTxn msgTag=FWSRVdefault ruleName=EAPRTLEVENTLOADCOMPLETE attributes={ { class=ASSOC { class=A1 \"PARAMETER\" } { class=A1 \"Value\" } } { \"CMD\" \"EAPEVENTLOADCOMPLETE\" } { \"TID\" \"21698\" } { \"USERID\" \"AUTO\" } { \"CARRIERID\" \"BP0318F\" } { \"LOTID\" \"SJ2RA.10\" } { \"EQPID\" \"EQP - 01\" } { \"PORTID\" \"EQP - 01 - 1\" } { \"COMMENT\" \"\" } }";

            //string readToUnLoad = ">>L FwSrvExecuteRuleTxn msgTag=FWSRVdefault ruleName=EAPRTLEVENTREADYTOUNLOAD attributes={ { class=ASSOC { class=A1 \"PARAMETER\" } { class=A1 \"Value\" } } { \"CMD\" \"EAPEVENTREADYTOUNLOAD\" } { \"TID\" \"21677\" } { \"USERID\" \"AUTO\" } { \"CARRIERID\" \"BP1778F\" } { \"LOTID\" \"SJ1YT.14\" } { \"EQPID\" \"EAP-01\" } { \"PORTID\" \"EQP-01-1\" } { \"BUFFER\" \"\" } { \"COMMENT\" \"\" } }";
            //string unloadComplete = ">>L FwSrvExecuteRuleTxn msgTag=FWSRVdefault ruleName=EAPRTLEVENTUNLOADCOMPLETE attributes={ { class=ASSOC { class=A1 \"PARAMETER\" } { class=A1 \"Value\" } } { \"CMD\" \"EAPEVENTUNLOADCOMPLETE\" } { \"TID\" \"21685\" } { \"USERID\" \"AUTO\" } { \"CARRIERID\" \"BP1778F\" } { \"LOTID\" \"SJ1YT.14\" } { \"EQPID\" \"EQP - 01\" } { \"PORTID\" \"EQP - 01 - 1\" } { \"COMMENT\" \"EQPID = EQP - 01 PORTID = EQP - 01 - 1\" } }";

            return readToLoad;
        }

        public string QueryStockerEvent()
        {
            return "OUTSTK TID=64758 msgTag=TRANSSRVdefault CARID=BP0318F TO=STK215 PORT=STK215_4FMO1";
        }

        #region Sample Functions

        public void SendLoadPortMessage(string tibcoMessage)
        {
            OnListenLoadPortEvent?.Invoke(this, tibcoMessage);
        }

        public void SendOutStockerMessage(string tibcoMessage)
        {
            OnListenStockerEvent?.Invoke(this, tibcoMessage);
        }

        public void SendJobPrepareMessage(string tibcoMessage)
        {
            OnListenJobPrepareEvent?.Invoke(this, tibcoMessage);
        }

        #endregion
    }
}
