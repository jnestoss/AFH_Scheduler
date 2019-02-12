using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class NormalCurveAdjusterVM : ObservableObject, IPageViewModel
    {
        private string _curveNumber;
        public string CurveNumber
        {
            get { return _curveNumber; }
            set
            {
                _curveNumber = value;
                OnPropertyChanged("CurveNumber");
            }
        }

        public NormalCurveAdjusterVM(string setValue)
        {
            CurveNumber = setValue;
        }

        public string Name
        {
            get
            {
                return "Adjust Normal Curve";
            }
        }

    }
}
