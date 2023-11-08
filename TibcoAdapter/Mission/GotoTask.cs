using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class GotoTask : MissionTask
    {
        public GotoTask(int taskIndex, string goal, string port) : base(taskIndex, goal, port)
        {
        }
    }
}
