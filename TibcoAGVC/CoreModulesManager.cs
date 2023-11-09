using GB.Common.CommunicationAdapters.Wcf;
using GB.MobileRobot.MissionManagement.MissionResponseService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAGVC
{
    public static class CoreModulesManager
    {
        private static AgvManager agvManager;
        private static MissionManager missionManager;
        private static AgvTaskManager agvTaskManager;
        private static TibcoEventManager tibcoEventManager;
        private static MissionServiceProxy missionServiceProxy;
        private static MissionResponseService missionResponseService;
        private static WcfServiceHost<IMissionResponseService> missionResponseServiceHost;

        public static McsLiteTcpClient McsLiteTcpClient { get; private set; }

        public static void Initialize(MainViewModel mainViewModel)
        {
            LoggerEventDispatcher.Start();

            tibcoEventManager = new TibcoEventManager();

            agvManager = new AgvManager();

            missionManager = new MissionManager();

            string mssionResponseServiceUri = @"http://127.0.0.1:40006/MissionResponseService";

            #region MissionServiceClient

            string missionServiceUri = @"http://127.0.0.1:40005/MissionService";
            missionServiceProxy = new MissionServiceProxy(agvManager, mainViewModel, missionServiceUri, mssionResponseServiceUri);
            missionServiceProxy.Start();

            #endregion

            #region MissionResponseService

            missionResponseService = new MissionResponseService(missionManager, missionServiceProxy, agvTaskManager, mainViewModel);
            var binding = WcfBindings.GetBasicHttpBinding(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), 2000000);
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            binding.Security.Mode = BasicHttpSecurityMode.None;
            binding.MaxBufferSize = 2000000;
            missionResponseServiceHost = new WcfServiceHost<IMissionResponseService>(missionResponseService, "MissionResponseService", true, binding);
            LoggerEventDispatcher.Info($"CoreModulesManager | Start MissionResponseService On {mssionResponseServiceUri}");
            missionResponseServiceHost.StartService(mssionResponseServiceUri);

            #endregion

            agvTaskManager = new AgvTaskManager(agvManager, missionManager, missionServiceProxy, tibcoEventManager, new MissionFactory(agvManager, tibcoEventManager));
            //agvTaskManager.Start();

            #region McsLiteTcpClient

            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000);
            McsLiteTcpClient = new McsLiteTcpClient(remoteEndPoint, tibcoEventManager);
            McsLiteTcpClient.Connect();

            #endregion
        }

        public static void Shutdown()
        {
            if (McsLiteTcpClient != null)
                McsLiteTcpClient.Disconnect();

            if (missionServiceProxy != null)
                missionServiceProxy.Stop();

            if (agvTaskManager != null)
                agvTaskManager.Stop();

            LoggerEventDispatcher.Stop();
        }
    }
}
