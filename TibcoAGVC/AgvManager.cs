using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibcoAdapter;

namespace TibcoAGVC
{
    public class AgvManager
    {
        private readonly List<Agv> agvList;

        public AgvManager()
        {
            agvList = new List<Agv>();
        } 

        public void AddAgv(Agv agv)
        {
            agvList.Add(agv);
        }

        public List<Agv> AllAgvs
        {
            get
            {
                return agvList.ToList();
            }
        }

        public Agv GetAgvById(int agvId)
        {
            return agvList.ToList().FirstOrDefault(x => x.Id == agvId);
        }
    }
}
