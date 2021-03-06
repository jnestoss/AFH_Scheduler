﻿using AFH_Scheduler.Dialogs.SettingSubWindows;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs
{
    public class SettingsVM : ObservableObject
    {
        private string _normalCurveValue;
        public string NormalCurve
        {
            get { return _normalCurveValue; }
            set
            {
                if (_normalCurveValue == value) return;
                _normalCurveValue = value;
                OnPropertyChanged("NormalCurve");
            }
        }

        private bool _dialogSettingBool;
        public bool DialogSettingBoolReturn
        {
            get { return _dialogSettingBool; }
            set
            {
                _dialogSettingBool = value;
                OnPropertyChanged("DialogSettingBoolReturn");
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

        private RelayCommand _normalCurveCommand;
        public ICommand NormalCurveCommand
        {
            get
            {
                if (_normalCurveCommand == null)
                    _normalCurveCommand = new RelayCommand(AdjustNormalCurve);
                return _normalCurveCommand;
            }
        }

        private async void AdjustNormalCurve(object obj)
        {
            var vm = new NormalCurveAdjusterVM(NormalCurve);
            var view = new NormalCurveAdjuster(vm);
            var result = await DialogHost.Show(view, "ProvidersDialog", ClosingEventHandlerProviders);
            if (DialogSettingBoolReturn)
            {
                File.WriteAllText(@"..\..\NormalCurve\NormalCurveValue.txt", String.Empty);
                File.WriteAllText(@"..\..\NormalCurve\NormalCurveValue.txt", vm.CurveNumber);
                NormalCurve = vm.CurveNumber;
            }
        }

        private RelayCommand _outcomeListCommand;
        public ICommand OutcomeListCommand
        {
            get
            {
                if (_outcomeListCommand == null)
                    _outcomeListCommand = new RelayCommand(ShowOutcomeCodes);
                return _outcomeListCommand;
            }
        }
        private async void ShowOutcomeCodes(object obj)
        {
            var vm = new OutcomeCodeListVM();
            var view = new OutcomeCodeListDialog(vm);
            var result = await DialogHost.Show(view, "ProvidersDialog", ClosingEventHandlerProviders);
        }

        public void ClosingEventHandlerProviders(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogSettingBoolReturn = false;
                return;
            }
            DialogSettingBoolReturn = true;
        }

        public SettingsVM()
        {
            string text = File.ReadAllText(@"..\..\NormalCurve\NormalCurveValue.txt");
            double testCase;
            if (!Double.TryParse(text, out testCase))
            {
                File.WriteAllText(@"..\..\NormalCurve\NormalCurveValue.txt", String.Empty);
                File.WriteAllText(@"..\..\NormalCurve\NormalCurveValue.txt", "15.99");
                text = "15.99";
            }
            NormalCurve = text;
        }
    }
}
