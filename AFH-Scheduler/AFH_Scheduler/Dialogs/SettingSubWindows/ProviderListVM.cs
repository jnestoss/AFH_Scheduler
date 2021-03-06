﻿using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Dialogs.Confirmation;
using AFH_Scheduler.Dialogs.Errors;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs.SettingSubWindows
{
    public class ProviderListVM : ObservableObject
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();
        private ObservableCollection<ProvidersModel> _providersList;
        public ObservableCollection<ProvidersModel> ProvidersList
        {
            get { return _providersList; }
            set
            {
                if (value != _providersList)
                {
                    _providersList = value;
                    OnPropertyChanged("Providers");
                }
            }
        }

        private bool _dialogBool;
        public bool DialogBoolReturn
        {
            get { return _dialogBool; }
            set
            {
                _dialogBool = value;
                OnPropertyChanged("DialogBoolReturn");
            }
        }

        #region Delete Provider Command
        private RelayCommand _providerDeleteCommand;
        public ICommand ProviderDeleteCommand
        {
            get
            {
                if (_providerDeleteCommand == null)
                    _providerDeleteCommand = new RelayCommand(DeleteProvider);
                return _providerDeleteCommand;
            }
        }
        private async void DeleteProvider(object obj)
        {
            var provider = (ProvidersModel)obj;
            var vm = new DeleteVM("Are you sure you want to remove this provider from the database?", "Provider:",
                provider.ProviderName);
            var deleteView = new DeleteProviderDialog(vm);

            var deleteResult = await DialogHost.Show(deleteView, "AddProviderDialog", ClosingEventHandlerAddProviders);

            if (deleteResult.Equals("Yes"))
            {
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    var id = Convert.ToInt64(provider.ProviderID);
                    var list = db.Provider_Homes.Where(r => r.FK_Provider_ID == id).ToList();
                    if (list.Count > 0)
                    {

                        var transferOrDeleteVM = new TransferDeleteVM(id, provider.ProviderName);
                        var transferView = new TransferOrDeleteHomes(transferOrDeleteVM);
                        var remainingHomeOption = await DialogHost.Show(transferView, "AddProviderDialog", ClosingEventHandlerAddProviders);

                        if (remainingHomeOption.Equals("Cancel"))
                        {
                            return;
                        }
                        if (remainingHomeOption.Equals("TRANSFER"))
                        {
                            foreach (var house in transferOrDeleteVM.RemovedHomes)
                            {
                                try
                                {
                                    Provider_Homes deletingHome = db.Provider_Homes.First(r => r.PHome_ID == house.HomeID);
                                                                         Scheduled_Inspections deletingSchedule = db.Scheduled_Inspections.First(r => r.FK_PHome_ID == house.HomeID
                                        && r.SInspections_Date == house.NextInspection);

                                        var homesHistory = db.Home_History.Where(r => r.FK_PHome_ID == house.HomeID).ToList();

                                        db.Provider_Homes.Remove(deletingHome);
                                        db.SaveChanges();

                                        db.Scheduled_Inspections.Remove(deletingSchedule);
                                        db.SaveChanges();
                                        foreach (var historyItem in homesHistory)
                                        {
                                            db.Home_History.Remove(historyItem);
                                            db.SaveChanges();
                                        }
                                }
                                catch (InvalidOperationException e)
                                {

                                }
                            }

                            foreach (var chowHome in transferOrDeleteVM.ChowedHomes)
                            {
                                try
                                {
                                    Provider_Homes tranferingHome = db.Provider_Homes.First(r => r.PHome_ID == chowHome.HomeID);
                                    tranferingHome.FK_Provider_ID = chowHome.ProviderID;
                                    db.SaveChanges();
                                }
                                catch (InvalidOperationException e)
                                {

                                }
                            }
                        }
                        
                    }
                      Provider deletingProv = db.Providers.First(r => r.Provider_ID == id);
                      db.Providers.Remove(deletingProv);
                      db.SaveChanges();
                      ProvidersList.Clear();
                      FillProviderTable();                    
                }
            }
        }
        #endregion

        #region Add Provider Command
        private RelayCommand _providerListCommand;
        public ICommand ProviderListCommand
        {
            get
            {
                if (_providerListCommand == null)
                    _providerListCommand = new RelayCommand(AddProvider);
                return _providerListCommand;
            }
        }

        private async void AddProvider(object obj)
        {
            var vm = new NewProviderCreatorVM();
            var view = new NewProviderCreator(vm);
            var result = await DialogHost.Show(view, "AddProviderDialog", ClosingEventHandlerAddProviders);
            if (DialogBoolReturn)
            {
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    db.Providers.Add(new Provider
                    {
                        Provider_ID = Convert.ToInt64(vm.NewProviderAdded.ProviderID),
                        Provider_Name = vm.NewProviderAdded.ProviderName
                    });
                    db.SaveChanges();

                }

                ProvidersList.Add(
                          new ProvidersModel
                          (
                              vm.NewProviderAdded.ProviderID,
                              vm.NewProviderAdded.ProviderName
                          )
                      );
            }
        }
        #endregion

        #region Edit Provider
        private RelayCommand _providerEditCommand;
        public ICommand ProviderEditCommand
        {
            get
            {
                if (_providerEditCommand == null)
                    _providerEditCommand = new RelayCommand(EditProvider);
                return _providerEditCommand;
            }
        }
        private async void EditProvider(object obj)
        {
            var provider = (ProvidersModel)obj;
            var vm = new EditProviderVM(provider.ProviderName);
            var view = new EditProvider(vm);
            var result = await DialogHost.Show(view, "AddProviderDialog", ClosingEventHandlerAddProviders);
            if (DialogBoolReturn)
            {
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    var id = Convert.ToInt64(provider.ProviderID);
                    var dbProvider = db.Providers.Where(r => r.Provider_ID == id).First();

                    dbProvider.Provider_Name = vm.EditableProviderName;
                    db.SaveChanges();
                }
                foreach (var prov in ProvidersList)
                {
                    if (prov.Equals(provider))
                    {
                        prov.ProviderName = vm.EditableProviderName;
                        break;
                    }
                }
            }
        }
        #endregion

        private void ClosingEventHandlerAddProviders(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogBoolReturn = false;
                return;
            }

            DialogBoolReturn = true;
        }

        public ProviderListVM()
        {
            _providersList = new ObservableCollection<ProvidersModel>();

            FillProviderTable();
        }

        public void FillProviderTable()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();
                foreach (var item in provs)
                {
                    ProvidersList.Add(
                            new ProvidersModel
                            (
                                item.Provider_ID.ToString(),
                                item.Provider_Name
                            )
                        );
                }
            }
        }
    }
}
