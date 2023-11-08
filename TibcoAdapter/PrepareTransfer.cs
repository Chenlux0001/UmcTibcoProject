using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class PrepareTransfer
    {
        public PrepareTransfer(string carrierId, PickTask pickTask, DropTask dropTask)
        {
            PickTask = pickTask;
            DropTask = dropTask;
            CarrierId = carrierId;
        }

        public string CarrierId { get; }

        public PickTask PickTask { get; }

        public DropTask DropTask { get; }

        public bool IsPickReady { get; set; }

        public bool IsDropReady { get; set; }
    }
}
