﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAdapter
{
    public class OutStockerEvent : TibcoEvent
    {
        public OutStockerEvent(string message) : base(TibcoEventType.OutStocker, message)
        {
            // TODO: Set Properties
        }

        public string Stocker { get; }

        public string Port { get; }
    }
}
