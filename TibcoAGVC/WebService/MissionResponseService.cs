using GB.MobileRobot.MissionManagement.MissionResponseService;
using GB.MobileRobot.MissionManagement.MissionResponseService.Dtos;
using GB.MobileRobot.MissionManagement.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAGVC
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MissionResponseService : IMissionResponseService
    {
        private readonly MainViewModel mainViewModel;

        private readonly AgvTaskManager agvTaskManager;
        private readonly MissionServiceProxy missionServiceProxy;

        public MissionResponseService(MissionServiceProxy missionServiceProxy, AgvTaskManager agvTaskManager, MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
            this.agvTaskManager = agvTaskManager;
            this.missionServiceProxy = missionServiceProxy;
        }

        public void InvokeLoadRequest(string goal, string port, int agvId)
        {

        }

        public void InvokeUnloadRequest(string goal, string port, int agvId)
        {

        }

        public void MissionChangeRequest(string missionId, int taskIndex)
        {

        }

        public void NotifyAbortMission(string missionId, AbortMissionResponseCodeDto abortMissionResponseCodeDto)
        {

        }

        public void NotifyAgvModeChanged(int agvId, AgvModeDto mode)
        {

        }

        public void NotifyAgvOperationStateChanged(int agvId, AgvOperationStateDto state)
        {

        }

        public void NotifyAgvTaskStarted(string missionId, int taskIndex, int agvId)
        {
            LoggerEventDispatcher.Info($"MissionResponseService | NotifyAgvTaskStarted | MissionId: {missionId} , TaskIndex: {taskIndex} , Agv: {agvId}");
        }

        public void NotifyAgvTaskCompleted(string missionId, int taskIndex, int agvId)
        {
            LoggerEventDispatcher.Info($"MissionResponseService | NotifyAgvTaskCompleted | MissionId: {missionId} , TaskIndex: {taskIndex} , Agv: {agvId}");
        }

        public void NotifyChangeMission(string missionId, ChangeMissionResponseCodeDto changeMissionResponseCodeDto)
        {

        }

        public void NotifyConnected()
        {

        }

        public void NotifyDock(string missionId, DockResponseCodeDto dockResponseCodeDto)
        {

        }

        public void NotifyErrorOccurred(string missionId, int taskIndex, int agvId, ErrorCodeDto errorDto)
        {
            LoggerEventDispatcher.Error($"MissionResponseService | NotifyErrorOccurred | MissionId: {missionId} , TaskIndex: {taskIndex} , Agv: {agvId} , ErrorCode: {errorDto}");
        }

        public void NotifyInvokeMissionTasks(string missionId, ValidationCodeDto validationCodeDto)
        {

        }

        public void NotifyMaintainMode(int agvId, bool isReservedByOperator)
        {

        }

        public void NotifyMissionAcceptance(bool online)
        {

        }

        public void NotifyMissionAssignment(string missionId, int agvId)
        {

        }

        public void NotifyMissionCompleted(string missionId, bool successflag)
        {

        }

        public void NotifyMissionTaskStarted(string missionId, int taskIndex)
        {
            LoggerEventDispatcher.Info($"MissionResponseService | NotifyMissionTaskStarted | MissionId: {missionId} , TaskIndex: {taskIndex}");
        }

        public void NotifyMissionTaskCompleted(string missionId, int taskIndex)
        {
            LoggerEventDispatcher.Info($"MissionResponseService | NotifyMissionTaskCompleted | MissionId: {missionId} , TaskIndex: {taskIndex}");
        }

        public void NotifyModifyMissionAssignmentMode(MissionAssignmentModeDto mode)
        {

        }

        public void NotifyServiceModeChanged(ServiceModeDto mode)
        {

        }

        public void ResponseHeartBeat()
        {
            missionServiceProxy.MrmsWebIsReachable = true;

            this.mainViewModel.MrmsWebIsReachable = missionServiceProxy.MrmsWebIsReachable;
        }

        public void SendSmartTag(string missionId, int taskIndex, RobotPodPositionDto robotPodPosition, string tag)
        {

        }
    }
}
