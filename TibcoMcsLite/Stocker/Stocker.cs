using JxSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoMcsLite
{
    public class Stocker : JxBindingBase
    {
        private StockerStatus status;

        public StockerStatus Status
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
