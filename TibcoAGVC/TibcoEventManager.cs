using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibcoAdapter;

namespace TibcoAGVC
{
    public class TibcoEventManager
    {
        private readonly List<TibcoEvent> tibcoEventList;

        public TibcoEventManager()
        {
            tibcoEventList = new List<TibcoEvent>();
        }

        public void AddEvent(TibcoEvent tibcoEvent)
        {
            if (tibcoEventList.Contains(tibcoEvent))
                return;

            tibcoEventList.Add(tibcoEvent);
        }

        public bool RemoveEvent(TibcoEvent tibcoEvent)
        {
            return tibcoEventList.Remove(tibcoEvent);
        }

        public List<TibcoEvent> AllEvents 
        { 
            get
            {
                return tibcoEventList.ToList();
            }
        }
    }
}
