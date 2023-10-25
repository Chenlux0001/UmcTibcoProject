using JxSystem.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TibcoAGVC
{
    public class MainViewModel : JxBindingBase
    {
        #region MRMS Web IsReachable

        private bool mrmsWebIsReachable;
        public bool MrmsWebIsReachable
        {
            get
            {
                return mrmsWebIsReachable;
            }
            set
            {
                SetPropertyValue(ref mrmsWebIsReachable, value);
            }
        }

        #endregion
    }
}
