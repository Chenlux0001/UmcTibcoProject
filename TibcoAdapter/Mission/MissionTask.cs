using JxSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class MissionTask : JxBindingBase
    {
        public MissionTask(int taskIndex, string goal, string port, string carrierId)
        {
            Goal = goal;
            Port = port;
            CarrierId = carrierId;
            TaskIndex = taskIndex;
        }

        public int TaskIndex { get; }

        public string Goal { get; }

        public string Port { get; }

        public string CarrierId { get; }

        private MissionTaskStatus status;

        public MissionTaskStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                SetPropertyValue(ref status, value);
            }
        }
    }
}
