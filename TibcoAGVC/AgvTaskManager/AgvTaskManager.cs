using JxSystem.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TibcoAdapter;
using TibcoAGVC.ServiceReference1;

namespace TibcoAGVC
{
    public class AgvTaskManager : JxServiceBase
    {
        private readonly AgvManager agvManager;
        private readonly MissionFactory missionFactory;
        private readonly MissionManager missionManager;
        private readonly TibcoEventManager tibcoEventManager;
        private readonly MissionServiceProxy missionServiceProxy;

        private readonly ConcurrentDictionary<Agv, AgvTaskExecutor> agvTaskExecutorDictionary;

        public AgvTaskManager(AgvManager agvManager, MissionManager missionManager, MissionServiceProxy missionServiceProxy, TibcoEventManager tibcoEventManager, MissionFactory missionFactory) : base(100)
        {
            this.agvManager = agvManager;
            this.missionManager = missionManager;
            this.missionFactory = missionFactory;
            this.tibcoEventManager = tibcoEventManager;
            this.missionServiceProxy = missionServiceProxy;
            this.agvTaskExecutorDictionary = new ConcurrentDictionary<Agv, AgvTaskExecutor>();

        }

        public override bool CanExecute()
        {
            return missionServiceProxy.MrmsWebIsReachable;
        }

        public override JxExecutionResult CatchException(object sender, Exception exception)
        {
            LoggerEventDispatcher.Info($"AgvTaskManager | CatchException | Exception: {exception}");

            return JxExecutionResult.Continue;
        }

        private MissionTaskDto ConvertToMissionTaskDto(MissionTask missionTask)
        {
            if (missionTask is PickTask pickTask)
            {
                PickTaskDto pickTaskDto = new PickTaskDto();
                pickTaskDto.TaskIndex = pickTask.TaskIndex;
                pickTaskDto.Goal = pickTask.Goal;
                pickTaskDto.PortId = pickTask.Port;
                pickTaskDto.CargoId = pickTask.CarrierId;
                pickTaskDto.ReadSmartTag = false;

                return pickTaskDto;
            }
            else if (missionTask is DropTask dropTask)
            {
                DropTaskDto dropTaskDto = new DropTaskDto();
                dropTaskDto.TaskIndex = dropTask.TaskIndex;
                dropTaskDto.Goal = dropTask.Goal;
                dropTaskDto.PortId = dropTask.Port;
                dropTaskDto.CargoId = dropTask.CarrierId;

                return dropTaskDto;
            }
            else if (missionTask is GotoTask gotoTask)
            {
                GotoTaskDto gotoTaskDto = new GotoTaskDto();
                gotoTaskDto.TaskIndex = gotoTask.TaskIndex;
                gotoTaskDto.Goal = gotoTask.Goal;
                gotoTaskDto.PortId = gotoTask.Port;

                return gotoTaskDto;
            }

            throw new NotImplementedException();
        }

        public void Remove(Agv agv)
        {
            agvTaskExecutorDictionary.TryRemove(agv, out AgvTaskExecutor existAgvTaskExecutor);
        }

        public bool HasTask(Agv agv, out AgvTaskExecutor agvTaskExecutor)
        {
            return agvTaskExecutorDictionary.TryGetValue(agv, out agvTaskExecutor);
        }

        public override JxExecutionResult Execute(IService service, CancellationToken cancellationToken)
        {
            var assignAgv = agvManager.AllAgvs.FirstOrDefault();
            if (assignAgv != null)
            {
                if (!HasTask(assignAgv, out AgvTaskExecutor agvTaskExecutor))
                {
                    var mission = missionFactory.CreateMission(assignAgv);
                    if (mission != null)
                    {
                        missionManager.AddMission(mission);

                        List<MissionTaskDto> missionTaskDtos = new List<MissionTaskDto>();
                        foreach (var missionTask in mission.MissionTasks)
                        {
                            missionTaskDtos.Add(ConvertToMissionTaskDto(missionTask));
                        }

                        var taskDetails = string.Join(",", mission.MissionTasks.Select(x => $"({x.GetType().Name} ({x.TaskIndex}) , CarrierId: {x.CarrierId} , Goal: {x.Goal} , Port: {x.Port})").ToList());
                        LoggerEventDispatcher.Info($"AgvTaskManager | Execute | InvokeMissionTasks: {mission.MissionId} , AgvId: {mission.AssignAgv.Id} , Tasks: {taskDetails}");

                        missionServiceProxy.InvokeMissionTasks(mission.MissionId, missionTaskDtos);

                        agvTaskExecutorDictionary.TryAdd(mission.AssignAgv, new AgvTaskExecutor(mission));
                    }
                }
            }

            return JxExecutionResult.Continue;
        }
    }
}
