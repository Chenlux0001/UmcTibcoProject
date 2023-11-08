using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TibcoAdapter;

namespace TibcoAGVC
{
    public class AgvTaskExecutor
    {
        public AgvTaskExecutor(Mission mission)
        {
            Mission = mission;
        }

        public Mission Mission { get; }
    }
}
