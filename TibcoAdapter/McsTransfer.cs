using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class McsTransfer
    {
        public string TransferId { get; set; }

        public string CarrierId { get; set; }

        public McsPickTask PickTask { get; set; }

        public McsDropTask DropTask { get; set; }
    }
}
