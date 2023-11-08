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
        private readonly MissionFactory missionFactory;
        private readonly TibcoEventManager tibcoEventManager;
        private readonly MissionServiceProxy missionServiceProxy;

        private readonly ConcurrentDictionary<Agv, AgvTaskExecutor> agvTaskExecutorDictionary;

        public AgvTaskManager(MissionServiceProxy missionServiceProxy, TibcoEventManager tibcoEventManager, MissionFactory missionFactory) : base(100)
        {
            this.missionFactory = missionFactory;
            this.tibcoEventManager = tibcoEventManager;
            this.missionServiceProxy = missionServiceProxy;
            this.agvTaskExecutorDictionary = new ConcurrentDictionary<Agv, AgvTaskExecutor>();

        }

        public override bool CanExecute()
        {
            return true;
            //return missionServiceProxy.MrmsWebIsReachable;
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

        public bool HasTask(Agv agv, out AgvTaskExecutor agvTaskExecutor)
        {
            return agvTaskExecutorDictionary.TryGetValue(agv, out agvTaskExecutor);
        }

        public override JxExecutionResult Execute(IService service, CancellationToken cancellationToken)
        {
            if (!agvTaskExecutorDictionary.Any())
            {
                var mission = missionFactory.CreateMission();
                if (mission != null && !HasTask(mission.AssignAgv, out AgvTaskExecutor agvTaskExecutor))
                {
                    List<MissionTaskDto> missionTaskDtos = new List<MissionTaskDto>();
                    foreach (var missionTask in mission.MissionTasks)
                    {
                        missionTaskDtos.Add(ConvertToMissionTaskDto(missionTask));
                    }

                    missionServiceProxy.InvokeMissionTasks(mission.MissionId, missionTaskDtos);

                    agvTaskExecutorDictionary.TryAdd(mission.AssignAgv, new AgvTaskExecutor(mission));
                }
            }

            return JxExecutionResult.Continue;
        }
    }
}
