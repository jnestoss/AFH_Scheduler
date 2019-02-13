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

        //this could be changed from a loop and instead store the previously selected row
        private Boolean[] _selected;
        public Boolean[] Selected {
            get { return _selected;  }
            set {
                for(int i = 0; i < Selected.Length; i++)
                {
                    Selected[i] = false;
                }
            }
        }

        private int? _selectedRow;
        public int? SelectedRow {
            get { return _selectedRow; }
            set {
                if (value != _selectedRow && value < Selected.Length && value > -1)
                {
                    _selectedRow = value;
                }
            }
        }

        public ObservableProviders()
        {
            _providers = new ObservableCollection<ScheduleModel>();
            _selected = new bool[12];
        }

        /**
        public static void ClearSelected()
        {
            foreach (ScheduleModel Provider in Providers)
            {
                Providers.IsSelected = false;
            }
        }
        **/
    }
}
