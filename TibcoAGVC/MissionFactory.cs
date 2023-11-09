using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibcoAdapter;

namespace TibcoAGVC
{
    public class MissionFactory
    {
        private readonly AgvManager agvManager;
        private readonly TibcoEventManager tibcoEventManager;

        private readonly ConcurrentDictionary<string, PrepareTransfer> prepareTransferDictionary;

        public MissionFactory(AgvManager agvManager, TibcoEventManager tibcoEventManager)
        {
            this.agvManager = agvManager;
            this.tibcoEventManager = tibcoEventManager;
            this.prepareTransferDictionary = new ConcurrentDictionary<string, PrepareTransfer>();
        }

        public Mission CreateMission()
        {
            #region Distribute TibcoEvent On PrepareTransfer By CarrierId

            var tibcoEvents = tibcoEventManager.AllEvents.ToList();
            foreach (var tibcoEvent in tibcoEvents)
            {
                if (!prepareTransferDictionary.ContainsKey(tibcoEvent.CarrierId))
                    prepareTransferDictionary.TryAdd(tibcoEvent.CarrierId, new PrepareTransfer(tibcoEvent.CarrierId));

                prepareTransferDictionary[tibcoEvent.CarrierId].AddEvent(tibcoEvent);

                tibcoEventManager.RemoveEvent(tibcoEvent);
            }

            #endregion

            #region Convert PrepareTransfer To Mission

            var prepareTransfers = prepareTransferDictionary.Values.ToList();
            foreach (var prepareTransfer in prepareTransfers)
            {
                if (DateTime.Now.Subtract(prepareTransfer.CreateTime).TotalHours >= 3)
                {
                    prepareTransferDictionary.TryRemove(prepareTransfer.CarrierId, out PrepareTransfer existPrepareTransfer);
                    continue;
                }

                var mission = prepareTransfer.ConvertToMission();
                if (mission != null)
                {
                    if (prepareTransferDictionary.TryRemove(prepareTransfer.CarrierId, out PrepareTransfer existPrepareTransfer))
                        return mission;
                }
            }

            #endregion

            return null;
        }
    }
}
