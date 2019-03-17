using System.Linq;
using System.Text;
using System;
using AFH_Scheduler.Helper_Classes;
using AFH_Scheduler.Database;
using System.Collections.ObjectModel;
using AFH_Scheduler.Data;
using System.Windows;
using AFH_Scheduler.Algorithm;
using System.Windows.Input;
using AFH_Scheduler.Dialogs;
using AFH_Scheduler.Dialogs.Errors;
using System.ComponentModel;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Microsoft.Office.Interop.Excel;
using AFH_Scheduler.Dialogs.Confirmation;
using System.Data.Entity;
using System.Collections.Generic;
using AFH_Scheduler.Database.LoginDB;
using System.IO;
using System.Windows.Data;
using System.Windows.Controls;
using AFH_Scheduler.Excel;
using AFH_Scheduler.HelperClasses;

namespace AFH_Scheduler
{
    public class DataVM : ObservableObject, IPageViewModel, INotifyPropertyChanged
    {
        #region variables

        public string Name {
            get {
                return "Schedules";
            }
        }

        private SchedulingAlgorithm alg = new SchedulingAlgorithm();

        private int _homeCount;
        public int HomeCount {
            get => _homeCount;
            set {
                if (_homeCount == value) return;
                _homeCount = value;
                OnPropertyChanged("HomeCount");
            }
        }

        private SnackbarMessageQueue _messageQueue;
        public SnackbarMessageQueue MessageQueue {
            get => _messageQueue;
            set {
                if (_messageQueue == value) return;
                _messageQueue = value;
                OnPropertyChanged("MessageQueue");
            }
        }

        private string _normalCurveValue;
        public string NormalCurve
        {
            get { return _normalCurveValue; }
            set
            {
                if (_normalCurveValue == value) return;
                _normalCurveValue = value;
                OnPropertyChanged("NormalCurve");
                CheckNormalCurveState();
            }
        }

        private double _desiredAverage;
        public double DesiredAverage {
            get => _desiredAverage;
            set {
                if (_desiredAverage == value) return;
                _desiredAverage = value;
                OnPropertyChanged("DesiredAverage");
            }
        }

        private string _normalCurveResultMsg;
        public string NormalCurveResultMsg
        {
            get { return _normalCurveResultMsg; }
            set
            {
                if (_normalCurveResultMsg == value) return;
                _normalCurveResultMsg = value;
                OnPropertyChanged("NormalCurveResultMsg");
            }
        }

        private System.Windows.Media.Brush _normalCurveState;
        public System.Windows.Media.Brush NormalCurveState
        {
            get { return _normalCurveState; }
            set
            {
                if (_normalCurveState == value) return;
                _normalCurveState = value;
                OnPropertyChanged("NormalCurveState");
            }
        }

        private User _usr;

        private string _snackBarContent;
        public string SnackBarContent {
            get => _snackBarContent;
            set {
                if (_snackBarContent == value) return;
                _snackBarContent = value;
                OnPropertyChanged("SnackBarContent");
            }
        }

        private static ObservableCollection<HomeModel> _selectedProviders;
        public ObservableCollection<HomeModel> SelectedProviders {
            get { return _selectedProviders; }
            set {
                if (value != _selectedProviders)
                {
                    _selectedProviders = value;
                    OnPropertyChanged("SelectedProviders");
                }
            }
        }

        private static ObservableCollection<HomeModel> _providers;
        public ObservableCollection<HomeModel> Providers {
            get { return _providers; }
            set {
                if (value != _providers)
                {
                    _providers = value;
                    OnPropertyChanged("Providers");
                }
            }
        }

        private static ObservableCollection<string> _providerNames;
        public ObservableCollection<string> ProviderNames {
            get { return _providerNames; }
            set {
                if (value != _providerNames)
                {
                    _providerNames = value;
                    OnPropertyChanged("ProviderNames");
                }
            }
        }

        private static ObservableCollection<string> _licenseNums;
        public ObservableCollection<string> LicenseNums {
            get { return _licenseNums; }
            set {
                if (value != _licenseNums)
                {
                    _licenseNums = value;
                    OnPropertyChanged("LicenseNums");
                }
            }
        }

        private static ObservableCollection<string> _homeNames;
        public ObservableCollection<string> HomeNames {
            get { return _homeNames; }
            set {
                if (value != _homeNames)
                {
                    _homeNames = value;
                    OnPropertyChanged("HomeNames");
                }
            }
        }

        private static ObservableCollection<string> _homeAddresses;
        public ObservableCollection<string> HomeAddresses {
            get { return _homeAddresses; }
            set {
                if (value != _homeAddresses)
                {
                    _homeAddresses = value;
                    OnPropertyChanged("HomeAddresses");
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

        /*public string _excelFilename;
        public string ExcelFileName
        {
            get { return _excelFilename; }
            set
            {
                _excelFilename = value;
                OnPropertyChanged("ExcelFileName");
            }
        }*/



        private HomeModel _selectedHome;
        public HomeModel SelectedHome {
            get => _selectedHome;
            set {
                if ( _selectedHome != value )
                {
                    _selectedHome = value;
                    OnPropertyChanged("SelectedHome");
                }
            }
        }


        public bool _dialogHostSuccess;
        public bool DialogHostSuccess
        {
            get { return _dialogHostSuccess; }
            set
            {
                _dialogHostSuccess = value;
                OnPropertyChanged("DialogHostSuccess");
            }
        }

        public bool _datePickerEnabled;
        public bool DatePickerEnabled
        {
            get { return _datePickerEnabled; }
            set
            {
                _datePickerEnabled = value;
                TextFieldEnabled = !_datePickerEnabled;
                OnPropertyChanged("DatePickerEnabled");
            }
        }

        public bool _textFieldEnabled;
        public bool TextFieldEnabled
        {
            get { return _textFieldEnabled; }
            set
            {
                _textFieldEnabled = value;
                OnPropertyChanged("TextFieldEnabled");
            }
        }

        public string _selectedFilter;
        public string SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;

                if(_selectedFilter == "Next Inspection Date")
                {
                    TextFieldEnabled = false;
                    DatePickerEnabled = true;
                } else
                {
                    TextFieldEnabled = false;
                    DatePickerEnabled = false;
                }

                switch(_selectedFilter)
                {
                    case "Provider Name":
                        UpdateSearchBoxSuggestions(ProviderNames);
                        break;
                    case "License Number":
                        UpdateSearchBoxSuggestions(LicenseNums);
                        break;
                    case "Home Name":
                        UpdateSearchBoxSuggestions(HomeNames);
                        break;
                    case "Address":
                        UpdateSearchBoxSuggestions(HomeAddresses);
                        break;
                    default:
                        UpdateSearchBoxSuggestions(ProviderNames);
                        break;
                }

                OnPropertyChanged("SelectedFilter");
            }
        }

        //public string _filterItem;
        //public string FilterItem
        //{
        //    get { return _filterItem; }
        //    set
        //    {
        //        _filterItem = value;
        //        OnPropertyChanged("FilterItem");
        //    }
        //}

        private DateTime _startDatePicked;
        public DateTime StartDatePicked
        {
            get { return _startDatePicked; }
            set
            {
                _startDatePicked = value;

                OnPropertyChanged("StartDatePicked");
            }
        }

        private DateTime _endDatePicked;
        public DateTime EndDatePicked
        {
            get { return _endDatePicked; }
            set
            {
                _endDatePicked = value;

                OnPropertyChanged("EndDatePicked");
            }
        }

        private IOpenMessageDialogService _messageService;
        public IOpenMessageDialogService MessageService
        {
            get
            {
                if (_messageService == null)
                    _messageService = new SchedulesOpenDialog();
                return _messageService;
            }
        }

        private  ICollectionView _comboBoxProviderItems;
        public ICollectionView ComboBoxProviderItems {
            get => _comboBoxProviderItems;
            set {
                _comboBoxProviderItems = value;
                OnPropertyChanged("ComboBoxProviderItems");
            }
        }

        private string _TextSearch;
        public string TextSearch {
            get {
                return _TextSearch;
            }
            set {
                if (_TextSearch != value)
                {
                    _TextSearch = value;
                    if (_TextSearch != "")
                    {
                        ComboBoxProviderItems.Refresh();
                    }
                    OnPropertyChanged("TextSearch");
                }
            }
        }

        #endregion

        #region commands
        private RelayCommand _inactiveListCommand;
        public ICommand InactiveListCommand
        {
            get
            {
                if (_inactiveListCommand == null)
                    _inactiveListCommand = new RelayCommand(InactiveListDisplayAsync);
                return _inactiveListCommand;
            }
        }

        private RelayCommand _filterTableCommand;
        public ICommand FilterTableCommand
        {
            get
            {
                if (_filterTableCommand == null)
                    _filterTableCommand = new RelayCommand(FilterTheTable);
                return _filterTableCommand;
            }
        }

        private RelayCommand _openSettingsDialog;
        public ICommand OpenSettingsDialogCommand {
            get {
                if (_openSettingsDialog == null) _openSettingsDialog = new RelayCommand(ExecuteSettingDialog);
                return _openSettingsDialog;
            }
        }

        private RelayCommand _openEditHistoryDialog;
        public ICommand OpenEditHistoryDialogCommand
        {
            get
            {
                if (_openEditHistoryDialog == null)
                    _openEditHistoryDialog = new RelayCommand(EditHistoryDialogOpen);
                return _openEditHistoryDialog;
            }
        }

        private RelayCommand _refreshTable;
        public ICommand RefreshTableCommand {
            get {
                if (_refreshTable == null)
                    _refreshTable = new RelayCommand(RefreshTable);
                return _refreshTable;
            }
        }

        private RelayCommand _importTableCommand;
        public ICommand ImportTableCommand {
            get {
                if (_importTableCommand == null)
                    _importTableCommand = new RelayCommand(ImportExcelTable);
                return _importTableCommand;
            }
        }

        private RelayCommand _exportTable;
        public ICommand ExportTableCommand {
            get {
                if (_exportTable == null)
                    _exportTable = new RelayCommand(ExportTable);
                return _exportTable;
            }
        }

        private RelayCommand _addNewHomeCommand;
        public ICommand AddNewHomeCommand {
            get {
                if (_addNewHomeCommand == null)
                    _addNewHomeCommand = new RelayCommand(CreateNewHomeAsync);
                return _addNewHomeCommand;
            }
        }

        //private RelayCommand _deleteHomeCommand;
        //public ICommand DeleteHomeCommand {
        //    get {
        //        if (_deleteHomeCommand == null)
        //            _deleteHomeCommand = new RelayCommand(DeleteHome);
        //        return _deleteHomeCommand;
        //    }
        //}

        public RelayCommand RunEditDialogCommand => new RelayCommand(ExecuteEditDialog);

        public RelayCommand RunHistoryDialogCommand => new RelayCommand(ExecuteHistoryDialog);

        public RelayCommand RunCompleteDialogCommand => new RelayCommand(ExecuteCompleteDialog);

        #endregion

        #region constructor
        public DataVM(User user)
        {
            MessageQueue = new SnackbarMessageQueue();
            Providers = new ObservableCollection<HomeModel>();
            SelectedProviders = new ObservableCollection<HomeModel>();
            InActiveHomes = new ObservableCollection<HomeModel>();
            TextFieldEnabled = true;
            StartDatePicked = DateTime.Today;
            EndDatePicked = DateTime.Today;
            _usr = user;

            TextSearch = "";

            ReadHomeData();
            FilterTheTable(null);

            HomeAddresses = new ObservableCollection<string>();
            HomeNames = new ObservableCollection<string>();
            LicenseNums = new ObservableCollection<string>();
            ProviderNames = new ObservableCollection<string>();

            foreach (var home in Providers)
            {
                HomeAddresses.Add(home.Address);
                HomeNames.Add(home.HomeName);
                LicenseNums.Add(home.HomeLicenseNum.ToString());
                ProviderNames.Add(home.ProviderName);
            }

            SelectedFilter = "Select Filter";

            //UpdateSearchBoxSuggestions(ProviderNames);
            //FilterByProviderName = true;

            string text = File.ReadAllText(@"..\..\NormalCurve\NormalCurveValue.txt");
            double testCase;
            if (!Double.TryParse(text, out testCase))
            {
                File.WriteAllText(@"..\..\NormalCurve\NormalCurveValue.txt", String.Empty);
                File.WriteAllText(@"..\..\NormalCurve\NormalCurveValue.txt", "15.99");
            }
            DesiredAverage = testCase;
            UpdateInspectionAverage();
        }

        #endregion

        #region excelstuff

        private async void ImportExcelTable(object obj)
        {
            var importData = new ImportDataPreviewVM();
            var view = new ImportDataPreview(importData);
            var result = await DialogHost.Show(view, "WindowDialogs", NewHomeClosingEventHandler);
            if (result.Equals("IMPORT"))
            {
                ExcelClass.ImportExcelTableNew(importData.ImportedHomes);

                foreach (var importedHome in importData.ImportedHomes)
                {
                    if (importedHome.IsActive)
                    {
                        Providers.Add(importedHome);
                    }
                    else
                    {
                        InActiveHomes.Add(importedHome);
                    }
                }
            }
        }

        private void ExportTable(object obj)
        {
            string fileName = MessageService.ExcelSaveDialog();
            if (fileName == null)
            {
                MessageQueue.Enqueue("File not saved");
                return;
            }

            ExcelClass.ExportTableNew(fileName, Providers);
        }
        #endregion

        #region Generators

        public void ReadHomeData()
        {
            Providers = new ObservableCollection<HomeModel>();
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var homes = db.Provider_Homes.ToList();

                foreach(Provider_Homes house in homes)
                {
                    Provider homeProvider = db.Providers.FirstOrDefault(x => x.Provider_ID == house.FK_Provider_ID);

                    var inspections = db.Scheduled_Inspections.FirstOrDefault(r => r.FK_PHome_ID == house.PHome_ID);

                    List<Home_History> allHomeHistory;
                    allHomeHistory = db.Home_History.Where(x => x.FK_PHome_ID == house.PHome_ID).ToList();
                    Home_History homeHistory = allHomeHistory.Last();

                    HomeModel newHome;
                    string provName;
                    long provID;
                    bool hasNoProv;
                    if (homeProvider is null)
                    {
                        provName = "No Provider";
                        provID = -1;
                        hasNoProv = true;
                    }
                    else
                    {
                        provName = homeProvider.Provider_Name;
                        provID = homeProvider.Provider_ID;
                        hasNoProv = false;
                    }

                    if (homeHistory.Inspection_Outcome.IOutcome_Code == "NEW")
                    {
                        newHome = new HomeModel
                        {
                            ProviderID = provID,
                            HomeID = house.PHome_ID,
                            ProviderName = provName,
                            HomeLicenseNum = Convert.ToInt64(house.PHome_LicenseNumber),
                            HomeName = house.PHome_Name,
                            Phone = house.PHome_Phonenumber,
                            Address = house.PHome_Address,
                            City = house.PHome_City,
                            ZIP = house.PHome_Zipcode,
                            RecentInspection = homeHistory.HHistory_Date,
                            NextInspection = inspections.SInspections_Date,
                            EighteenthMonthDate = inspections.SInspections_EighteenMonth,
                            SeventeenMonthDate = inspections.SInspections_SeventeenMonth,
                            ForecastedDate = inspections.SInspection_ForecastedDate,
                            HasNoProvider = hasNoProv,
                            IsActive = true,
                            RcsUnit = house.PHome_RCSUnit
                        };
                    }
                    else
                    {
                        newHome = new HomeModel
                        {
                            ProviderID = provID,
                            HomeID = house.PHome_ID,
                            ProviderName = provName,
                            HomeLicenseNum = Convert.ToInt64(house.PHome_LicenseNumber),
                            HomeName = house.PHome_Name,
                            Phone = house.PHome_Phonenumber,
                            Address = house.PHome_Address,
                            City = house.PHome_City,
                            ZIP = house.PHome_Zipcode,
                            RecentInspection = homeHistory.HHistory_Date,
                            NextInspection = inspections.SInspections_Date,
                            EighteenthMonthDate = inspections.SInspections_EighteenMonth,
                            SeventeenMonthDate = inspections.SInspections_SeventeenMonth,
                            ForecastedDate = inspections.SInspection_ForecastedDate,
                            HasNoProvider = hasNoProv,
                            IsActive = true,
                            RcsRegionUnit = house.PHome_RCSUnit,
                        };
                    }

                    Providers.Add(newHome);
                }               
            }
            HomeCount = Providers.Count;
            //FilterTheTable(null);
        }
        #endregion

        #region Dialogs

        private async void CreateNewHomeAsync(object obj)
        {
            var createdHome = new NewHomeDialogVM();
            var view = new NewHomeDialog(createdHome);
            var result = await DialogHost.Show(view, "WindowDialogs", NewHomeClosingEventHandler);

            if (DialogHostSuccess)
            {
                string recentDate;
                var home = createdHome.NewHomeCreated;

                var recentInspec = alg.GrabbingRecentInspection(Convert.ToInt32(home.HomeID));
                if (recentInspec == null)
                {
                    recentDate = DateTime.Today.ToString("MM/dd/yyyy");
                }
                else
                    recentDate = recentInspec.HHistory_Date;

                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    long newHomeID = GenerateNewIDs.GenerateHomeID();

                    long newHistID = GenerateNewIDs.GenerateHistoryID();

                    long newschedID = GenerateNewIDs.GenerateScheduleID();

                    Scheduled_Inspections dates = new Scheduled_Inspections
                    {
                        FK_PHome_ID = newHomeID,
                        SInspections_Date = home.NextInspection,
                        SInspections_EighteenMonth = alg.DropDateMonth(recentDate, Drop.EIGHTEEN_MONTH),
                        SInspections_SeventeenMonth = alg.DropDateMonth(recentDate, Drop.SEVENTEEN_MONTH),
                        SInspections_Id = newschedID,
                        SInspection_ForecastedDate = SchedulingAlgorithm.NextScheduledDate(db.Inspection_Outcome.First(), home.NextInspection)
                    };

                    db.Scheduled_Inspections.Add(dates);
                    db.SaveChanges();

                    List<Scheduled_Inspections> inspections = new List<Scheduled_Inspections>();
                    inspections.Add(dates);

                    if (createdHome.SelectedCode.IOutcome_Code == "NEW")
                    {
                        db.Home_History.Add(new Home_History
                        {
                            FK_PHome_ID = newHomeID,
                            FK_Outcome_Code = db.Inspection_Outcome.First(r => r.IOutcome_Code == "NEW").IOutcome_Code, //"NEW"
                            HHistory_Date = DateTime.Today.ToString("MM/dd/yyyy"),
                            HHistory_ID = newHistID
                        });
                    }
                    else
                    {
                        db.Home_History.Add(new Home_History
                        {
                            FK_PHome_ID = newHomeID,
                            FK_Outcome_Code = createdHome.SelectedCode.IOutcome_Code,
                            HHistory_Date = home.RecentInspection,
                            HHistory_ID = newHistID
                        });
                    }

                    db.SaveChanges();

                    db.Provider_Homes.Add(new Provider_Homes
                    {
                        FK_Provider_ID = home.ProviderID,
                        Home_History = db.Home_History.Where(r => r.FK_PHome_ID == home.HomeID).ToList(),
                        PHome_Address = home.Address,
                        PHome_City = home.City,
                        PHome_ID = newHomeID,
                        PHome_LicenseNumber = $"{home.HomeLicenseNum}",
                        PHome_Name = home.HomeName,
                        PHome_Phonenumber = home.Phone,
                        PHome_RCSUnit = home.RcsRegionUnit,
                        PHome_Zipcode = home.ZIP,
                        Provider = db.Providers.First(r => r.Provider_Name == createdHome.TextSearch)
                    });

                    db.SaveChanges();

                }

                RefreshTable(null);
                FilterTheTable(null);
                UpdateInspectionAverage();
            }
        }

        private async void ExecuteHistoryDialog(object o)
        {
            if (SelectedHome == null)
            {
                MessageQueue.Enqueue("No home Selected");
            }
            else
            {
                var view = new HistoryDialog
                {
                    DataContext = new HistVM(SelectedHome)
                };

                var result = await DialogHost.Show(view, "WindowDialogs", GenericClosingEventHandler);
            }
        }

        private async void ExecuteCompleteDialog(object o)
        {
            if (SelectedHome == null)
            {
                MessageQueue.Enqueue("No home Selected");
            }
            else
            {
                var view = new CompleteDialog
                {
                    DataContext = new CompleteVM(SelectedHome)
                };

                var result = await DialogHost.Show(view, "WindowDialogs", CompleteInspectionClosingEventHandler);
            }
        }

        private async void InactiveListDisplayAsync(object obj)
        {
            var vm = new InactiveHomeListVM(InActiveHomes);
            var view = new InactiveHomeList(vm);
            var result = await DialogHost.Show(view, "WindowDialogs", NewHomeClosingEventHandler);
            if (result.Equals("Submit"))
            {

                foreach (var reactive in vm.ReActiveHomes)
                {
                    foreach (var update in vm.UpdateHomeSchedules)
                    {
                        if (update.Contains(reactive.HomeID.ToString()))
                        {
                            String[] inspect = update.Split('-');
                            reactive.NextInspection = inspect[1];
                            reactive.EighteenthMonthDate = alg.DropDateMonth(reactive.RecentInspection, Drop.EIGHTEEN_MONTH);
                        }
                    }
                    Providers.Add(reactive);
                    InActiveHomes.Remove(reactive);
                }
            }
        }

        //private void DeleteHome(object obj)
        //{
        //    if (SelectedSchedule == null)
        //    {
        //        MessageService.ReleaseMessageBox("Please select a home to delete.");
        //        return;
        //    }
        //    if (MessageService.MessageConfirmation("Are you sure you want to delete " + SelectedSchedule.Address + "?" +
        //        " It will be removed from the database.", "Deleting Home"))
        //    {
        //        if (MessageService.MessageConfirmation("Are you TRULY sure you want to delete " + SelectedSchedule.Address + "?"
        //            , "Deleting Home"))
        //        {
        //            using (HomeInspectionEntities db = new HomeInspectionEntities())
        //            {
        //                try
        //                {
        //                    Provider_Homes deletingHome = db.Provider_Homes.First(r => r.PHome_ID == SelectedSchedule.HomeID);
        //                    db.Provider_Homes.Remove(deletingHome);
        //                    db.SaveChanges();

        //                    Scheduled_Inspections deletingSchedule = db.Scheduled_Inspections.First(r => r.FK_PHome_ID == SelectedSchedule.HomeID
        //                    && r.SInspections_Date == SelectedSchedule.NextInspection);
        //                    db.Scheduled_Inspections.Remove(deletingSchedule);
        //                    db.SaveChanges();
        //                    Providers.Remove(SelectedSchedule);
        //                    MessageService.ReleaseMessageBox("Selected Home has been removed from the table.");
        //                    RefreshTable("");
        //                }
        //                catch (InvalidOperationException e)
        //                {
        //                    MessageService.ReleaseMessageBox("Selected Home removal failed.");
        //                }
        //            }
        //        }
        //    }
        //}

        private async void ExecuteSettingDialog(object o)
        {
            var settingsVM = new SettingsVM(Convert.ToDouble(NormalCurve), DesiredAverage);
            var settingsView = new SettingsDialog(settingsVM);
            var result = await DialogHost.Show(settingsView, "WindowDialogs", SettingsClosingEventHandler);
        }

        private async void ExecuteEditDialog(object o)
        {
            if (SelectedHome == null)
            {
                MessageQueue.Enqueue("No home Selected");
            }
            else
            {
                var view = new EditDialog();
                view.DataContext = new EditVM(SelectedHome, DesiredAverage, Convert.ToDouble(NormalCurve));
                //view.setDataContext(SelectedHome);

                var result = await DialogHost.Show(view, "WindowDialogs", EditClosingEventHandler);

                //if (result.Equals("DELETE"))
                //{
                //    DeleteProviderConfirmationDialog(view);
                //}
            }
        }


        //private async void DeleteProviderConfirmationDialog(object view)
        //{
        //    var deleteView = new DeleteConfirmationDialog();

        //    var deleteResult = await DialogHost.Show(deleteView, "DeleteConfirmationDialog", GenericClosingEventHandler);

        //    if (deleteResult.Equals("Yes"))
        //    {
        //        using (HomeInspectionEntities db = new HomeInspectionEntities())
        //        {
        //            //Console.WriteLine("********" + ((EditVM)((EditDialog)eventArgs.Session.Content).DataContext).SelectedSchedule.HomeID);
        //            var home = db.Provider_Homes.SingleOrDefault(r => r.PHome_ID == ((EditVM)((EditDialog)view).DataContext).SelectedSchedule.HomeID);

        //            if (home != null)
        //            {
        //                db.Provider_Homes.Remove(home);
        //                db.SaveChanges();
        //            }
        //        }
        //    }
        //}

        private void EditHistoryDialogOpen(object obj)
        {
            /*HistoryModel historyModel = (HistoryModel)obj;
            HistoryDetailViewVM historyDetailView = new HistoryDetailViewVM(historyModel.HomeID, MessageService);
            var updateOrNot = MessageService.ShowDialog(historyDetailView);*/
            MessageService.ReleaseMessageBox("Outcome Code can not be edited currently.");
        }

        #endregion

        #region Closing Event Handlers

        private void SettingsClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            SettingsVM settingsContext = (SettingsVM)((SettingsDialog)eventArgs.Session.Content).DataContext;
            DesiredAverage = Convert.ToDouble(settingsContext.NormalCurve);
        }

        private void NewHomeClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("Dialog closed successfully");
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogHostSuccess = false;
                return;
            }
            DialogHostSuccess = true;
        }

        private void CompleteInspectionClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("Dialog closed successfully");

            if((String)eventArgs.Parameter == "SUBMIT")
            {
                CompleteVM completeDialogContext = ((CompleteVM)((CompleteDialog)eventArgs.Session.Content).DataContext);
                HomeModel updatedHomeValues = completeDialogContext.SelectedHome;
                string newNextInspectionDate = completeDialogContext.SelectedCode.IOutcome_Code;

                using(HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    string nextInspection = completeDialogContext.NextInspection;

                    Provider_Homes selectHome = db.Provider_Homes.FirstOrDefault(r => r.PHome_ID == updatedHomeValues.HomeID);

                    if (selectHome != null)
                    {
                        db.Home_History.Add(new Home_History
                        {
                            FK_Outcome_Code = db.Inspection_Outcome.FirstOrDefault(r => r.IOutcome_Code == newNextInspectionDate).IOutcome_Code,
                            FK_PHome_ID = updatedHomeValues.HomeID,
                            HHistory_Date = updatedHomeValues.NextInspection,
                            HHistory_ID = GenerateNewIDs.GenerateHistoryID()
                        });

                        Scheduled_Inspections homeDates = selectHome.Scheduled_Inspections.First();

                        homeDates.SInspections_Date = nextInspection;
                        homeDates.SInspections_EighteenMonth  = alg.DropDateMonth(homeDates.SInspections_Date, Drop.EIGHTEEN_MONTH);
                        homeDates.SInspections_SeventeenMonth = alg.DropDateMonth(homeDates.SInspections_Date, Drop.SEVENTEEN_MONTH);
                        homeDates.SInspection_ForecastedDate = SchedulingAlgorithm.NextScheduledDate(completeDialogContext.SelectedCode, nextInspection);
                    }

                    db.SaveChanges();
                }
            }

            RefreshTable(null);
            FilterTheTable(null);
            UpdateInspectionAverage();
        }

        private void EditClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("Dialog closed successfully");
            if ((String)eventArgs.Parameter == "Cancel") return;

            if ((String)eventArgs.Parameter == "SUBMIT")
            {
                EditVM editDialogContext = ((EditVM)((EditDialog)eventArgs.Session.Content).DataContext);
                HomeModel editedHomeData = editDialogContext.SelectedSchedule;
                string editedProvider = editDialogContext.TextSearch;
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    var licenseNum = editedHomeData.HomeLicenseNum;
                    var homeName = editedHomeData.HomeName;
                    var providerName = editedHomeData.ProviderName;
                    var address = editedHomeData.Address;
                    var city = editedHomeData.City;
                    var zip = editedHomeData.ZIP;
                    var phone = editedHomeData.Phone;
                    var rcsUnit = editedHomeData.RcsRegionUnit;
                    var nextInspection = editDialogContext.NextInspection;

                    Provider_Homes selectHome = db.Provider_Homes.FirstOrDefault(r => r.PHome_ID == editedHomeData.HomeID);

                    if (selectHome != null)
                    {
                        selectHome.PHome_LicenseNumber = licenseNum.ToString();
                        selectHome.PHome_Name = homeName;
                        selectHome.Provider = db.Providers.FirstOrDefault(r => r.Provider_Name == editedProvider);
                        selectHome.PHome_Address = address;
                        selectHome.PHome_City = city;
                        selectHome.PHome_Zipcode = zip;
                        selectHome.PHome_Phonenumber = phone;
                        selectHome.PHome_RCSUnit = rcsUnit;
                        Scheduled_Inspections homeDates = selectHome.Scheduled_Inspections.First();

                        Home_History homeHistory = selectHome.Home_History.FirstOrDefault(x => x.FK_PHome_ID == editedHomeData.HomeID);

                        String nextScheduledInspection = nextInspection.ToString("MM/dd/yyyy");

                        homeDates.SInspections_Date = nextScheduledInspection;
                        //homeDates.SInspections_EighteenMonth  = alg.DropDateMonth(homeHistory.HHistory_Date, Drop.EIGHTEEN_MONTH);
                        //homeDates.SInspections_SeventeenMonth = alg.DropDateMonth(homeHistory.HHistory_Date, Drop.SEVENTEEN_MONTH);
                        homeDates.SInspection_ForecastedDate = SchedulingAlgorithm.CalculateNextScheduledDate(homeHistory.Inspection_Outcome, nextScheduledInspection, Convert.ToDouble(NormalCurve), DesiredAverage);

                        db.SaveChanges();
                    }
                }
            }
            RefreshTable(null);
            FilterTheTable(null);
            UpdateInspectionAverage();
        }

        private void GenericClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("Dialog closed successfully");
        }
        #endregion

        #region Table Methods
        private void RefreshTable(object obj)
        {
            Providers.Clear();
            ReadHomeData();
        }

        private void FilterTheTable(object obj)
        {
            //RefreshTable(obj);//Comment out if you want to test the license number/name filter
            //if (SelectedFilter == null)
            //{
            //    MessageService.ReleaseMessageBox("You have not specified what to filter out.");
            //    return;
            //}
            SelectedProviders.Clear();

            if (TextSearch.Equals("") && !DatePickerEnabled)
            {
                foreach (var prov in Providers)
                {
                    SelectedProviders.Add(prov);
                }
            }
            else
            {
                if (SelectedFilter.ToString().Contains("Provider Name"))
                {
                    foreach (var item in Providers)
                    {
                        if (item.ProviderName.Contains(TextSearch))
                        {
                            SelectedProviders.Add(item);
                        }
                    }
                }

                else if (SelectedFilter.ToString().Contains("Address"))
                {
                    foreach (var item in Providers)
                    {
                        if (item.Address.Contains(TextSearch))
                        {
                            SelectedProviders.Add(item);
                        }
                    }
                }

                else if (SelectedFilter.ToString().Contains("License Number"))
                {
                    foreach (var item in Providers)
                    {
                        if (item.HomeLicenseNum.ToString().Contains(TextSearch))
                        {
                            SelectedProviders.Add(item);
                        }
                    }
                }

                else if (SelectedFilter.ToString().Contains("Home Name"))
                {
                    foreach (var item in Providers)
                    {
                        if (item.HomeName.Contains(TextSearch))
                        {
                            SelectedProviders.Add(item);
                        }
                    }
                }

                else if (SelectedFilter.ToString().Contains("Next Inspection Date"))
                {
                    foreach (var item in Providers)
                    {
                        if (IsInspectionWithinDateRange(item.NextInspection))
                        {
                            SelectedProviders.Add(item);
                        }
                    }
                }

                Providers.Clear();
                foreach (var returnItem in SelectedProviders)
                {
                    Providers.Add(returnItem);
                }
            }
        }
        #endregion

        #region Helper Methdos

        private void UpdateSearchBoxSuggestions(ObservableCollection<string> items)
        {
            var lv = (ListCollectionView)CollectionViewSource.GetDefaultView(items);
            lv.CustomSort = Comparer<String>.Create(StringSort);
            ComboBoxProviderItems = lv;
        }

        private bool IsInspectionWithinDateRange(string nextInspection)
        {
            var inspectDate = SchedulingAlgorithm.ExtractDateTime(nextInspection);
            if ((DateTime.Compare(inspectDate, StartDatePicked) >= 0) && (DateTime.Compare(inspectDate, EndDatePicked) <= 0))
            {
                return true;
            }
            return false;
        }

        private void UpdateInspectionAverage()
        {
            NormalCurve = String.Format("{0:0.00}", SchedulingAlgorithm.CalculateInspectionAverage());
            CheckNormalCurveState();
        }

        public void CheckNormalCurveState()
        {
            double curveValue = Convert.ToDouble(NormalCurve);
            if (curveValue < DesiredAverage - 0.3 || curveValue > DesiredAverage + 0.3)
            {
                NormalCurveState = System.Windows.Media.Brushes.Red;
                //NormalCurveResultMsg = "The list of inspections average out below the normal curve.";
            }
            else if (curveValue < DesiredAverage - 0.2 || curveValue > DesiredAverage + 0.2)
            {
                NormalCurveState = System.Windows.Media.Brushes.Orange;
            }
            else if (curveValue < DesiredAverage - 0.1 || curveValue > DesiredAverage + 0.1)
            {
                NormalCurveState = System.Windows.Media.Brushes.Yellow;
            }
            else
            {
                NormalCurveState = System.Windows.Media.Brushes.Green;
                //NormalCurveResultMsg = "The list of inspections average out approximatly at the normal curve.";
            }
        }

        private int StringSort(string x, string y)
        {
            return GetDistance(x).CompareTo(GetDistance(y));
        }

        private int GetDistance(string provider)
        {
            if (string.IsNullOrWhiteSpace(TextSearch))
            {
                return 0;
            }

            if(SelectedFilter.Equals("Provider Name"))
            {
                string[] splitName = provider.Split(' ');

                string first = splitName[0];
                string last = splitName[splitName.Length - 1];

                first = first.Substring(0, Math.Min(first.Length, TextSearch.Length));
                last = last.Substring(0, Math.Min(last.Length, TextSearch.Length));

                return Math.Min(GetDistance(first, TextSearch), GetDistance(last, TextSearch));
            }
            else
            {
                provider = provider.Substring(0, Math.Min(provider.Length, TextSearch.Length));

                return GetDistance(provider, TextSearch);
            }
        }

        //Taken from: https://github.com/dotnet/command-line-api/blob/master/src/System.CommandLine/Invocation/TypoCorrection.cs
        private static int GetDistance(string first, string second)
        {
            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            int n = first.Length;
            int m = second.Length;
            if (n == 0) return m;
            if (m == 0) return n;

            int curRow = 0, nextRow = 1;
            int[][] rows = { new int[m + 1], new int[m + 1] };

            for (int j = 0; j <= m; ++j)
            {
                rows[curRow][j] = j;
            }

            for (int i = 1; i <= n; ++i)
            {
                rows[nextRow][0] = i;
                for (int j = 1; j <= m; ++j)
                {
                    int dist1 = rows[curRow][j] + 1;
                    int dist2 = rows[nextRow][j - 1] + 1;
                    int dist3 = rows[curRow][j - 1] + (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

                    rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
                }

                // Swap the current and next rows
                if (curRow == 0)
                {
                    curRow = 1;
                    nextRow = 0;
                }
                else
                {
                    curRow = 0;
                    nextRow = 1;
                }
            }

            return rows[curRow][m];
        }

        public DataVM GetDataInstance()
        {
            return this;
        }

        #endregion

        #region User Methods
        public void ClearUser() { _usr = null; }
        #endregion
    }
}
