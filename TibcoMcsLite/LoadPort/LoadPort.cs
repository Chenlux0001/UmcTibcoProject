using JxSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoMcsLite
{
    public class LoadPort : JxBindingBase
    {
        private LoadPortState state;
        private LoadPortStatus status;
        private LoadPortAccessMode accessMode;

        public LoadPortState State
        {
            get
            {
                return state;
            }
            set
            {
                SetPropertyValue(ref state, value);
            }
        }

        public LoadPortAccessMode AccessMode
        {
            get
            {
                return accessMode;
            }
            set
            {
                SetPropertyValue(ref accessMode, value);
            }
        }

        public LoadPortStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                SetPropertyValue(ref status, value);
            }
        }

    }
}
