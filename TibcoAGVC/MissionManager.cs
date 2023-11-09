using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibcoAdapter;

namespace TibcoAGVC
{
    public class MissionManager
    {
        private readonly object syncRoot = new object();

        private readonly List<Mission> missionList;

        public MissionManager()
        {
            missionList = new List<Mission>();
        }

        public void AddMission(Mission mission)
        {
            lock (syncRoot)
            {
                missionList.Add(mission);
            }
        }

        public void RemoveMission(Mission mission)
        {
            lock (syncRoot)
            {
                missionList.Remove(mission);
            }
        }

        public Mission GetMission(string missionId)
        {
            lock (syncRoot)
            {
                return missionList.FirstOrDefault(x => x.MissionId == missionId);
            }
        }
    }
}
