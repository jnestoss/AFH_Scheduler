using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.HelperClasses
{
    interface ICloseable
    {
        event EventHandler<EventArgs> RequestClose;
    }
}
