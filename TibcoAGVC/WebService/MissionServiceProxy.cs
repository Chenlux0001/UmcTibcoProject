using GB.Common.CommunicationAdapters.Wcf;
using JxSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibcoAdapter;
using TibcoAGVC.ServiceReference1;

namespace TibcoAGVC
{
    public class MissionServiceProxy : JxServiceBase
    {
        private readonly MainViewModel mainViewModel;

        private readonly object syncRoot = new object();

        private readonly AgvManager agvManager;

        private readonly string missionServiceUri;

        private readonly string mssionResponseServiceUri;

        private readonly MissionServiceClient missionServiceClient;

        public MissionServiceProxy(AgvManager agvManager, MainViewModel mainViewModel, string missionServiceUri, string mssionResponseServiceUri) : base(3000)
        {
            this.agvManager = agvManager;
            this.mainViewModel = mainViewModel;
            this.missionServiceUri = missionServiceUri;
            this.mssionResponseServiceUri = mssionResponseServiceUri;

            var binding = WcfBindings.GetBasicHttpBinding(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), 2000000);

            EndpointAddress address = new EndpointAddress(missionServiceUri);

            missionServiceClient = new MissionServiceClient(binding, address);
        }

        public override bool CanExecute()
        {
            return true;
        }

        public override JxExecutionResult CatchException(object sender, Exception exception)
        {
            LoggerEventDispatcher.Error($"MissionServiceProxy | CatchException | Exception: {exception}");

            return JxExecutionResult.Continue;
        }

        public bool ConnectToService()
        {
            try
            {
                lock (syncRoot)
                {
                    missionServiceClient.ConnectToService(mssionResponseServiceUri);

                    LoggerEventDispatcher.Info($"MissionServiceProxy | ConnectToService | Connect To Mission Service {missionServiceUri} Success");
                }

                return true;
            }
            catch
            {
                LoggerEventDispatcher.Error($"MissionServiceProxy | ConnectToService | Connect To Mission Service {missionServiceUri} Failed");
            }

            return false;
        }

        public bool MrmsWebIsReachable { get; set; }

        public void InvokeMissionTasks(string missionId, List<MissionTaskDto> missionTaskDtos)
        {
            missionServiceClient.InvokeMissionTasks(missionId, missionTaskDtos.ToArray());
        }

        public override JxExecutionResult Execute(IService service, CancellationToken cancellationToken)
        {
            try
            {
                if (this.mainViewModel != null)
                {
                    if (!this.MrmsWebIsReachable)
                    {
                        // ConnectToService
                        this.MrmsWebIsReachable = ConnectToService();
                        this.mainViewModel.MrmsWebIsReachable = this.MrmsWebIsReachable;

                        if (this.MrmsWebIsReachable)
                        {
                            var agvIds = missionServiceClient.GetAgvIds().ToList();
                            foreach (var agvId in agvIds)
                            {
                                if (agvManager.GetAgvById(agvId) == null)
                                    agvManager.AddAgv(new Agv(agvId));
                            }
                        }
                    }
                    else
                    {
                        // RequestHeartBeat
                        missionServiceClient.RequestHeartBeat();
                    }
                }
            }
            catch (Exception ex)
            {
                this.MrmsWebIsReachable = false;
                this.mainViewModel.MrmsWebIsReachable = this.MrmsWebIsReachable;

                LoggerEventDispatcher.Error($"MissionServiceProxy | Execute | Exception: {ex.Message}");

                Thread.Sleep(3000);
            }

            return JxExecutionResult.Continue;
        }
    }
}
