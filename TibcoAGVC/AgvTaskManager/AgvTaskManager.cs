using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAGVC
{
    public class AgvTaskManager
    {
        private readonly ConcurrentDictionary<Agv, AgvTaskExecutor> agvTaskExecutorDictionary;

        public AgvTaskManager()
        {
            agvTaskExecutorDictionary = new ConcurrentDictionary<Agv, AgvTaskExecutor>();
        }

        public bool HasTask(Agv agv, out AgvTaskExecutor agvTaskExecutor)
        {
            return agvTaskExecutorDictionary.TryGetValue(agv, out agvTaskExecutor);
        }

    }
}
