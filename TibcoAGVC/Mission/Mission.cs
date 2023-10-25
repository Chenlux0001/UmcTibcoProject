using JxSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAGVC
{
    public class Mission : JxBindingBase
    {
        public Mission(string missionId, List<MissionTask> tasks)
        {
            MissionId = missionId;
            Tasks = tasks.ToList();
        }

        public string MissionId { get; }

        public List<MissionTask> Tasks { get; }

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
