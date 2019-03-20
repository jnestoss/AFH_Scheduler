using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AFH_Scheduler.Dialogs.Confirmation
{
    public class TransferDeleteVM : ObservableObject, IPageViewModel
    {
        private double _currentAverage;
        public double CurrentAverage {
            get { return _currentAverage; }
            set {
                if (_currentAverage == value) return;
                _currentAverage = value;
                OnPropertyChanged("NormalCurve");
            }
        }

        private double _desiredAverage;
        public double DesiredAverage {
            get => _desiredAverage;
            set {
                if (_desiredAverage == value) return;
                _desiredAverage = value;
                OnPropertyChanged("DesiredValue");
            }
        }

        private SchedulingAlgorithm alg = new SchedulingAlgorithm();
        private static ObservableCollection<HomeModel> _remainingHomes;
        public ObservableCollection<HomeModel> RemainingHomes
        {
            get { return _remainingHomes; }
            set
            {
                if (value != _remainingHomes)
                {
                    _remainingHomes = value;
                    OnPropertyChanged("RemainingHomes");
                }
            }
        }
        private static ObservableCollection<HomeModel> _chowedHomes;
        public ObservableCollection<HomeModel> ChowedHomes
        {
            get { return _chowedHomes; }
            set
            {
                if (value != _chowedHomes)
                {
                    _chowedHomes = value;
                    OnPropertyChanged("ChowedHomes");
                }
            }
        }

        private static ObservableCollection<HomeModel> _removedHomes;
        public ObservableCollection<HomeModel> RemovedHomes
        {
            get { return _removedHomes; }
            set
            {
                if (value != _removedHomes)
                {
                    _removedHomes = value;
                    OnPropertyChanged("RemovedHomes");
                }
            }
        }

        private static ObservableCollection<HomeModel> _deactiveHomes;
        public ObservableCollection<HomeModel> DeactiveHomes
        {
            get { return _deactiveHomes; }
            set
            {
                if (value != _deactiveHomes)
                {
                    _deactiveHomes = value;
                    OnPropertyChanged("DeactiveHomes");
                }
            }
        }

        private bool _allHomesCleared;
        public bool AllHomesCleared
        {
            get { return _allHomesCleared; }
            set
            {
                _allHomesCleared = value;
                OnPropertyChanged("AllHomesCleared");
            }
        }

        private string _provName;
        public string ProvName
        {
            get { return _provName; }
            set
            {
                _provName = value;
                OnPropertyChanged("ProvName");
            }
        }

        private RelayCommand _editLicenseCommand;
        public ICommand EditLicenseCommand
        {
            get
            {
                if (_editLicenseCommand == null)
                    _editLicenseCommand = new RelayCommand(EditHomeLicense);
                return _editLicenseCommand;
            }
        }

        private async void EditHomeLicense(object obj)
        {
            var selectItem = (HomeModel)obj;
            var view = new EditDialog {
                DataContext = new EditVM(selectItem, DesiredAverage, CurrentAverage)
            };

            var result = await DialogHost.Show(view, "TransferDialog", ClosingEventHandlerLicense);
            if (!result.Equals("CANCEL"))
            {
                var editVM = (EditVM)view.DataContext;
                var editedItem = editVM.SelectedSchedule;
                if (result.Equals("SUBMIT"))
                {
                    string editedProvider = editVM.TextSearch;
                    using (HomeInspectionEntities db = new HomeInspectionEntities())
                    {
                        Provider prov;
                        try
                        {
                            prov = db.Providers.FirstOrDefault(r => r.Provider_Name == editedProvider);
                            selectItem.ProviderID = prov.Provider_ID;
                            selectItem.ProviderName = prov.Provider_Name;
                        }
                        catch (Exception e)
                        {
                            selectItem.ProviderID = -1;
                            selectItem.ProviderName = "No Provider";
                        }
                        selectItem.HomeLicenseNum = editedItem.HomeLicenseNum;
                        selectItem.HomeName = editedItem.HomeName;
                        selectItem.Address = editedItem.Address;
                        selectItem.City = editedItem.City;
                        selectItem.ZIP = editedItem.ZIP;
                        selectItem.Phone = editedItem.Phone;
                        selectItem.NextInspection = editedItem.NextInspection;

                        ChowedHomes.Add(selectItem);
                    }
                }
                if (result.Equals("DELETE"))
                {
                    RemovedHomes.Add(selectItem);
                }

                if (result.Equals("DEAC"))
                {
                    DeactiveHomes.Add(selectItem);
                }

                RemainingHomes.Remove(selectItem);

                if (RemainingHomes.Count == 0)
                {
                    AllHomesCleared = true;
                }
            }
        }
        private void ClosingEventHandlerLicense(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                return;
            }

        }

        public TransferDeleteVM(long id, string name, double desiredAverage, double currentAverage)
        {
            DesiredAverage = desiredAverage;
            CurrentAverage = currentAverage;
            AllHomesCleared = false;
            _chowedHomes = new ObservableCollection<HomeModel>();
            _remainingHomes = new ObservableCollection<HomeModel>();
            _removedHomes = new ObservableCollection<HomeModel>();
            _deactiveHomes = new ObservableCollection<HomeModel>();

            ProvName = id + "-" + name;
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var totalHomes = db.Provider_Homes.Where(r => r.FK_Provider_ID == id).ToList();
                foreach (var house in totalHomes)
                {
                    string recentDate;
                    var recentInspec = alg.GrabbingRecentInspection(Convert.ToInt32(house.PHome_ID));
                    if (recentInspec == null)
                    {
                        recentDate = "";
                    }
                    else
                        recentDate = recentInspec.HHistory_Date;
                    var insp = db.Scheduled_Inspections.Where(r => r.FK_PHome_ID == house.PHome_ID).First().SInspections_Date;

                    bool isActive = true;
                    if (house.PHome_Active is null)
                        isActive = false;

                    RemainingHomes.Add(
                        new HomeModel
                        {
                            ProviderID = id,
                            HomeID = house.PHome_ID,
                            ProviderName = name,
                            HomeLicenseNum = Convert.ToInt32(house.PHome_LicenseNumber),
                            HomeName = house.PHome_Name,
                            Phone = house.PHome_Phonenumber,
                            Address = house.PHome_Address,
                            City = house.PHome_City,
                            ZIP = house.PHome_Zipcode,
                            RecentInspection = recentDate,
                            NextInspection = insp,
                            EighteenthMonthDate = alg.DropDateMonth(recentDate, Drop.EIGHTEEN_MONTH),
                            IsActive = isActive,
                            RcsRegionUnit = house.PHome_RCSUnit
                        }
                    );
                }
            }
        }

        public string Name
        {
            get
            {
                return "Transfer or Delete Homes?";
            }
        }
    }
}
