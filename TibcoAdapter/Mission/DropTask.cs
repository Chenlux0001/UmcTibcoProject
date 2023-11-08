using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class DropTask : MissionTask
    {
        public DropTask(int taskIndex, string goal, string port, string carrierId) : base(taskIndex, goal, port)
        {
            CarrierId = carrierId;
        }

        public string CarrierId { get; }
    }
}
