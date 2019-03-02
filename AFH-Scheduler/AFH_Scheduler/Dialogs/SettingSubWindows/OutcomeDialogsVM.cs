using AFH_Scheduler.Data;
using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class OutcomeDialogsVM : ObservableObject, IPageViewModel
    {
        private OutcomeModel _outcomeData;
        public OutcomeModel OutcomeData
        {
            get { return _outcomeData; }
            set
            {
                _outcomeData = value;
                OnPropertyChanged("OutcomeData");
            }
        }

        public OutcomeDialogsVM(string codeword, string minTime, string maxTime)
        {
            OutcomeData = new OutcomeModel(codeword, minTime, maxTime);
        }

        public string Name
        {
            get
            {
                return "Outcome Code Data Window";
            }
        }
    }
}
