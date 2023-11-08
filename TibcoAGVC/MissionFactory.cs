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

        private void JobPrepareEventToPrepareTransfer()
        {
            var jobPrepareEvents = tibcoEventManager.OfType<JobPrepareEvent>().ToList();
            foreach (var jobPrepareEvent in jobPrepareEvents)
            {
                PrepareTransfer prepareTransfer = jobPrepareEvent.ToPrepareTransfer();
                if (prepareTransfer != null)
                {
                    if (!prepareTransferDictionary.TryGetValue(prepareTransfer.CarrierId, out PrepareTransfer existPrepareTransfer))
                    {
                        prepareTransferDictionary.TryAdd(prepareTransfer.CarrierId, prepareTransfer);
                    }
                }

                tibcoEventManager.RemoveEvent(jobPrepareEvent);
            }
        }

        private void SetLoadPortEventReadyFlagOnTransfer()
        {
            var loadPortEvents = tibcoEventManager.OfType<LoadPortEvent>().ToList();
            foreach (var loadPortEvent in loadPortEvents)
            {
                if (loadPortEvent.IsReadyToLoad)
                {
                    var transfer = prepareTransferDictionary.Values.ToList().Where(x => x.DropTask.Goal == loadPortEvent.Eqp && x.DropTask.Port == loadPortEvent.Port).FirstOrDefault();
                    if (transfer != null)
                        transfer.IsDropReady = true;
                }
                else if (loadPortEvent.IsReadyToUnload)
                {
                    var transfer = prepareTransferDictionary.Values.ToList().Where(x => x.PickTask.Goal == loadPortEvent.Eqp && x.PickTask.Port == loadPortEvent.Port).FirstOrDefault();
                    if (transfer != null)
                        transfer.IsPickReady = true;
                }
                else if (loadPortEvent.IsLoadComplete)
                {
                    // TODO:
                }
                else if (loadPortEvent.IsUnloadComplete)
                {
                    // TODO:
                }

                tibcoEventManager.RemoveEvent(loadPortEvent);
            }
        }

        private void SetInStockerFlagOnTransfer()
        {
            var inStockerEvents = tibcoEventManager.OfType<InStockerEvent>().ToList();
            foreach (var inStockerEvent in inStockerEvents)
            {
                var transfer = prepareTransferDictionary.Values.ToList().Where(x => x.DropTask.Goal == inStockerEvent.Stocker && x.DropTask.Port == inStockerEvent.Port).FirstOrDefault();
                if (transfer != null)
                    transfer.IsDropReady = true;

                tibcoEventManager.RemoveEvent(inStockerEvent);
            }
        }

        private void SetOutStockerFlagOnTransfer()
        {
            var outStockerEvents = tibcoEventManager.OfType<OutStockerEvent>().ToList();
            foreach (var outStockerEvent in outStockerEvents)
            {
                var transfer = prepareTransferDictionary.Values.ToList().Where(x => x.PickTask.Goal == outStockerEvent.Stocker && x.PickTask.Port == outStockerEvent.Port).FirstOrDefault();
                if (transfer != null)
                    transfer.IsPickReady = true;

                tibcoEventManager.RemoveEvent(outStockerEvent);
            }
        }

        public Mission CreateMission()
        {
            // (1) JobPrepareEvent To PrepareTransfer
            JobPrepareEventToPrepareTransfer();

            // (2) Set LoadPort ReayToLoad & ReayToUnload Flag
            SetLoadPortEventReadyFlagOnTransfer();

            // (3) Set InStocker Flag
            SetInStockerFlagOnTransfer();

            // (4) Set OutStocker Flag
            SetOutStockerFlagOnTransfer();

            var readyTransfers = prepareTransferDictionary.Values.ToList().Where(x => x.IsPickReady == true && x.IsDropReady == true).ToList();

            // TODO: Create Mission

            return null;
        }
    }
}
