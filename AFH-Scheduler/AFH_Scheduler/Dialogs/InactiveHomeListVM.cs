using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Dialogs.Confirmation;
using AFH_Scheduler.Dialogs.Rescheduling;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs
{
    public class InactiveHomeListVM : ObservableObject
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();

        private List<string> _updateHomeSchedules;
        public List<string> UpdateHomeSchedules
        {
            get { return _updateHomeSchedules; }
            set
            {
                if (value != _updateHomeSchedules)
                {
                    _updateHomeSchedules = value;
                }
            }
        }

        private static ObservableCollection<HomeModel> _inActiveHomes;
        public ObservableCollection<HomeModel> InActiveHomes
        {
            get { return _inActiveHomes; }
            set
            {
                if (value != _inActiveHomes)
                {
                    _inActiveHomes = value;
                    OnPropertyChanged("InActiveHomes");
                }
            }
        }

        private static ObservableCollection<HomeModel> _reActiveHomes;
        public ObservableCollection<HomeModel> ReActiveHomes
        {
            get { return _reActiveHomes; }
            set
            {
                if (value != _reActiveHomes)
                {
                    _reActiveHomes = value;
                    OnPropertyChanged("ReActiveHomes");
                }
            }
        }

        private RelayCommand _reactivateCommand;
        public ICommand ReactivateCommand
        {
            get
            {
                if (_reactivateCommand == null)
                    _reactivateCommand = new RelayCommand(ReactivateHomeAsync);
                return _reactivateCommand;
            }
        }

        private async void ReactivateHomeAsync(object obj)
        {
            HomeModel item = (HomeModel)obj;
            var vm = new DeleteVM("Are you sure you want to reactivate this home?", "Address:",
                item.Address);
            var rescheduleView = new DeleteProviderDialog(vm);

            var result = await DialogHost.Show(rescheduleView, "ReactivateDialog", ClosingEventHandlerAddProviders);

            if (result.Equals("Yes"))
            {
                vm.DeleteMessage = "Would you like to schedule an inspection date for this address?";

                var deleteResult2 = await DialogHost.Show(rescheduleView, "ReactivateDialog", ClosingEventHandlerAddProviders);
                if (deleteResult2.Equals("Yes"))
                {
                    var vm2 = new ReschedulingVM(item.NextInspection);
                    var rescheduleView2 = new SettingUpAnInspection(vm2);
                    var deleteResult3 = await DialogHost.Show(rescheduleView2, "ReactivateDialog", ClosingEventHandlerAddProviders);
                    if (deleteResult3.Equals("SUBMIT"))
                    {
                        UpdateHomeSchedules.Add(item.HomeID + "-" + vm2.DatePicked.ToShortDateString());
                    }
                }
                ReActiveHomes.Add(item);
                InActiveHomes.Remove(item);
            }

        }

        private void ClosingEventHandlerAddProviders(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                return;
            }
        }

        public InactiveHomeListVM(ObservableCollection<HomeModel> homesList)
        {
            InActiveHomes = new ObservableCollection<HomeModel>();
            ReActiveHomes = new ObservableCollection<HomeModel>();
            UpdateHomeSchedules = new List<string>();

            foreach (var home in homesList)
            {
                InActiveHomes.Add(home);
            }
        }
    }
}
