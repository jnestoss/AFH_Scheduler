using AFH_Scheduler.Dialogs.SettingSubWindows;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs
{
    public class SettingsVM
    {
        private string _normalCurveValue;
        public string NormalCurve
        {
            get { return _normalCurveValue; }
            set
            {
                if (_normalCurveValue == value) return;
                _normalCurveValue = value;
            }
        }

        private RelayCommand _providerListCommand;
        public ICommand ProviderListCommand
        {
            get
            {
                if (_providerListCommand == null)
                    _providerListCommand = new RelayCommand(ShowProviderList);
                return _providerListCommand;
            }
        }

        private async void ShowProviderList(object obj)
        {
            var vm = new ProviderListVM();
            var view = new ProviderListDialog(vm);
            var result = await DialogHost.Show(view, "ProvidersDialog", ClosingEventHandlerProviders);
        }

        public static void ClosingEventHandlerProviders(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                return;
            }
        }

        public SettingsVM()
        {
            NormalCurve = "15.99";
        }
    }
}
