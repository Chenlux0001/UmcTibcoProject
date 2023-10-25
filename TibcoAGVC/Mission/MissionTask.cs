using JxSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAGVC
{
    public class MissionTask : JxBindingBase
    {
        public MissionTask(int taskIndex, string goal, string port)
        {
            Goal = goal;
            Port = port;
            TaskIndex = taskIndex;
        }

        public int TaskIndex { get; }

        public string Goal { get; }

        public string Port { get; }

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
