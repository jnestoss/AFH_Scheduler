using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AFH_Scheduler.Helper_Classes;

namespace AFH_Scheduler.Data
{
    public class ObservableProviders : ObservableObject
    {
        private ObservableCollection<ScheduleModel> _providers;
        public ObservableCollection<ScheduleModel> Providers {
            get { return _providers; }
            set {
                if (value != _providers)
                {
                    _providers = value;
                    OnPropertyChanged("Providers");
                }
            }
        }

    

        public ObservableProviders()
        {
            _providers = new ObservableCollection<ScheduleModel>();
        }

        public static void ClearSelected()
        {
            foreach (ScheduleModel Provider in Providers)
            {
                Providers.IsSelected = false;
            }
        }
    }
}
