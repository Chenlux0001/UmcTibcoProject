using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class PrepareTransfer
    {
        private readonly List<TibcoEvent> tibcoEventList;

        public PrepareTransfer(string carrierId)
        {
            CarrierId = carrierId;

            CreateTime = DateTime.Now;

            tibcoEventList = new List<TibcoEvent>();
        }

        public string CarrierId { get; }

        public DateTime CreateTime { get; }

        public void AddEvent(TibcoEvent tibcoEvent)
        {
            tibcoEventList.Add(tibcoEvent);
        }

        public Mission ConvertToMission(Agv agv)
        {
            // TODO:

            return null;
        }
    }
}
