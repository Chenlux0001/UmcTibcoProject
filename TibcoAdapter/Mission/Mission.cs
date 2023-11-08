using JxSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class Mission : JxBindingBase
    {
        public Mission(string missionId, List<MissionTask> tasks, Agv assignAgv)
        {
            AssignAgv = assignAgv;
            MissionId = missionId;
            MissionTasks = tasks.ToList();
        }

        public string MissionId { get; }

        public Agv AssignAgv { get; }

        public List<MissionTask> MissionTasks { get; }

        private MissionStatus status;

        public MissionStatus Status
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
