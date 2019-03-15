using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs.Help
{
    public class HelpBoxVM : ObservableObject, IPageViewModel
    {
        public HelpBoxVM()
        {
        }
        
        public string Name => "Help Page";
    }
}
