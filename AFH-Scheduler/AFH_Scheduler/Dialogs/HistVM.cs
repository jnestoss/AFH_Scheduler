using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AFH_Scheduler.Data;

namespace AFH_Scheduler.Dialogs
{
    public class HistVM : ObservableObject, IPageViewModel
    {
        public string Name => "History Page";

        public HistVM()
        {
            
        }
    }
}