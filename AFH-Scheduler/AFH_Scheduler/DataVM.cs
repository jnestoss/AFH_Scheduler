﻿
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

namespace AFH_Scheduler
{
    public class DataVM : ObservableObject, IPageViewModel
    {
        #region variables

        public string Name {
            get {
                return "Schedules";
            }
        }

        private SchedulingAlgorithm alg = new SchedulingAlgorithm();

        private User _usr;

        private HomeModel _selectedSchedule;
        private HomeModel SelectedSchedule {
            get { return _selectedSchedule; }
            set {
                if (_selectedSchedule == value) return;
                _selectedSchedule = value;
            }
        }

        //Observable and bound to DataGrid
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

        public object _selectedFilter;
        public object SelectedFilter
        {
            get { return _selectedFilter; }
            set
            {
                _selectedFilter = value;
                OnPropertyChanged("SelectedFilter");
            }
        }
        public string _filterItem;
        public string FilterItem
        {
            get { return _filterItem; }
            set
            {
                _filterItem = value;
                OnPropertyChanged("FilterItem");
            }
        }

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

        private RelayCommand _deleteHomeCommand;
        public ICommand DeleteHomeCommand {
            get {
                if (_deleteHomeCommand == null)
                    _deleteHomeCommand = new RelayCommand(DeleteHome);
                return _deleteHomeCommand;
            }
        }

        public RelayCommand RunEditDialogCommand => new RelayCommand(ExecuteEditDialog);

        public RelayCommand RunCompleteDialogCommand => new RelayCommand(ExecuteCompleteDialog);

        #endregion

        #region constructor
        public DataVM(User user)
        {
            _providers = new ObservableCollection<HomeModel>();
            _inActiveHomes = new ObservableCollection<HomeModel>();
            FilterItem = "";
            TextFieldEnabled = true;
            StartDatePicked = DateTime.Today;
            EndDatePicked = DateTime.Today;

            GenData();



            foreach (var provider in Providers)
            {
                GenHistoryData(provider);
            }
        }
        #endregion

        #region excelstuff
        private async void ImportExcelTable(object obj)
        {
            var importData = new ImportDataPreviewVM();
            var view = new ImportDataPreview(importData);
            var result = await DialogHost.Show(view, "WindowDialogs", ClosingEventHandlerNewHome);
            if (result.Equals("IMPORT"))
            {
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
            //Console.WriteLine("WWWWWWWWWWWWWWWW   " + obj.GetType() + "WWWWWWWWWWWWWWWWW");
            /*if (ExcelFileName.Equals("") || !(Directory.Exists(ExcelFileName)))
            {
                MessageService.ReleaseMessageBox("Directory not found");
                return;
            }*/
            string fileName = MessageService.ExcelSaveDialog();
            if (fileName == null)
            {
                MessageService.ReleaseMessageBox("Excel File was not saved.");
                return;
            }
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                try
                { //We write to an Excel file
                    Microsoft.Office.Interop.Excel.Application xlApp;
                    Workbook xlWorkbook;
                    Worksheet xlWorksheet;

                    xlApp = new Microsoft.Office.Interop.Excel.Application
                    {
                        //Visible = true
                    };

                    try
                    {//Excel work here
                        xlWorkbook = xlApp.Workbooks.Add("");
                        xlWorksheet = (Worksheet)xlWorkbook.ActiveSheet;
                        /*"Please include:  
                         * Region/unit; Checked
                         * last inspection; Checked
                         * current inspection; Checked
                         * how many months and days those are apart; Checked
                         *** code (result) from current inspection (we have shared codes to use based performance); 
                         *** forecasted next inspection date; 
                         * 17 month drop dead date; Checked
                         * 18 month drop dead date (for forecasted inspection). Checked"
                         */

                        xlWorksheet.Cells[1, 1] = "License Number";
                        xlWorksheet.Cells[1, 2] = "Provider";
                        xlWorksheet.Cells[1, 3] = "Address";
                        xlWorksheet.Cells[1, 4] = "City";
                        xlWorksheet.Cells[1, 5] = "Zipcode";
                        xlWorksheet.Cells[1, 6] = "Recent Inspection Date";
                        xlWorksheet.Cells[1, 7] = "Next Inspection Date";
                        xlWorksheet.Cells[1, 8] = "Interval in Months";
                        xlWorksheet.Cells[1, 9] = "Interval in Days";
                        xlWorksheet.Cells[1, 10] = "17th Month Drop Dead";
                        xlWorksheet.Cells[1, 11] = "18th Month Drop Dead";
                        xlWorksheet.Cells[1, 12] = "Outcome"; //From current inspection
                        xlWorksheet.Cells[1, 13] = "Forecasted Next Inspection";//forecasted next inspection date
                        xlWorksheet.Cells[1, 14] = "RCS Region";
                        xlWorksheet.Cells[1, 15] = "RCS Unit";


                        int row = 2;
                        foreach (var provider in Providers)
                        {
                            //var home = db.Provider_Homes.Where(r => r.PHome_Address == provider.Address).First();                            

                            xlWorksheet.Cells[row, 1] = provider.ProviderID;
                            xlWorksheet.Cells[row, 2] = provider.ProviderName;
                            xlWorksheet.Cells[row, 3] = provider.Address;
                            xlWorksheet.Cells[row, 4] = provider.City;
                            xlWorksheet.Cells[row, 5] = provider.ZIP;
                            xlWorksheet.Cells[row, 6] = provider.RecentInspection;
                            xlWorksheet.Cells[row, 7] = provider.NextInspection;
                            xlWorksheet.Cells[row, 8] = alg.InspectionInterval(provider.RecentInspection, provider.NextInspection, true);//Interval in Months
                            xlWorksheet.Cells[row, 9] = alg.InspectionInterval(provider.RecentInspection, provider.NextInspection, false);//Interval in Days
                            xlWorksheet.Cells[row, 10] = alg.DropDateMonth(provider.NextInspection, true);//17th Month Drop Date
                            xlWorksheet.Cells[row, 11] = provider.EighteenthMonthDate;
                            xlWorksheet.Cells[row, 12] = "";//Outcome from current inspection
                            xlWorksheet.Cells[row, 13] = "";//forecasted next inspection date
                            xlWorksheet.Cells[row, 14] = provider.RcsRegion;//RCS Region
                            xlWorksheet.Cells[row, 15] = provider.RcsUnit;//RCS Unit

                            row++;
                        }

                        xlWorksheet.get_Range("A1", "N1").EntireColumn.AutoFit();

                        //xlApp.Visible = false;
                        //xlApp.UserControl = false;
                        if (fileName.Contains(".xlsx"))
                            xlWorkbook.SaveAs(fileName, FileFormat: XlFileFormat.xlOpenXMLWorkbook);
                        else if (fileName.Contains(".csv"))
                        {
                            xlWorkbook.SaveAs(fileName, FileFormat: XlFileFormat.xlCSVWindows);
                            xlWorkbook.Close(false);
                        }


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Problem with Excel " + e.ToString());

                    }
                    finally
                    {
                        xlApp.Workbooks.Close();
                        xlApp.Quit();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Problem with Excel " + e.ToString());

                }
            }
        }
        #endregion

        #region Generators

        public void GenData()
        {
            using(HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();
                
                foreach (var item in provs)
                {
                    var homes = db.Provider_Homes.Where(r=> r.FK_Provider_ID == item.Provider_ID).ToList();
                    foreach (var house in homes)
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


                        Providers.Add(
                            new HomeModel
                            {
                                ProviderID = item.Provider_ID,
                                HomeID = house.PHome_ID,
                                ProviderName = item.Provider_Name,
                                HomeLicenseNum = long.Parse(house.PHome_LicenseNumber),
                                HomeName = house.PHome_Name,
                                Phone = house.PHome_Phonenumber,
                                Address = house.PHome_Address,
                                City = house.PHome_City,
                                ZIP = house.PHome_Zipcode,
                                RecentInspection = recentDate,
                                NextInspection = insp,
                                EighteenthMonthDate = alg.DropDateMonth(insp, false),
                                IsActive = true,
                                RcsRegion = "",
                                RcsUnit = ""
                            }
                        );
                    }
                }

            }
        }

        public void GenHistoryData(HomeModel house)
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                long providerID;
                var provs = db.Home_History.Where(x => x.FK_PHome_ID == house.HomeID).ToList();
                foreach (var item in provs)
                {
                    providerID = db.Provider_Homes.First(r => r.PHome_ID == item.FK_PHome_ID.Value).FK_Provider_ID.Value;//providerID;
                    
                    house.HomesHistory.Add(
                        new HistoryDetailModel
                        (
                            providerID,//providerID
                            item.FK_PHome_ID.Value,//Home_ID
                            item.HHistory_Date,//InspectionDate
                            item.Inspection_Outcome.IOutcome_Code
                        )
                    );
                }
            }
        }
        #endregion

        #region Dialogs

        private async void CreateNewHomeAsync(object obj)
        {
            var createdHome = new NewHomeDialogVM();
            var view = new NewHomeDialog(createdHome);
            var result = await DialogHost.Show(view, "WindowDialogs", ClosingEventHandlerNewHome);

            if (DialogHostSuccess)
            {
                string recentDate;
                var home = createdHome.NewHomeCreated;

                var recentInspec = alg.GrabbingRecentInspection(Convert.ToInt32(home.HomeID));
                if (recentInspec == null)
                {
                    recentDate = "";
                }
                else
                    recentDate = recentInspec.HHistory_Date;
                Providers.Add(
                    new HomeModel
                    {
                        ProviderID = Convert.ToInt64(home.SelectedProviderName.ProviderID),
                        HomeID = home.HomeID,
                        ProviderName = home.SelectedProviderName.ProviderName,
                        HomeLicenseNum = Convert.ToInt64(home.HomeLicenseNum),
                        HomeName = home.HomeLicensedName,
                        Phone = home.HomePhoneNumber,
                        Address = home.Address,
                        City = home.City,
                        ZIP = home.Zipcode,
                        RecentInspection = recentDate,
                        NextInspection = home.InspectionDate.ToShortDateString(),
                        EighteenthMonthDate = alg.DropDateMonth(home.InspectionDate.ToShortDateString(), false),
                        IsActive = true,
                        RcsRegion = home.RcsRegion,
                        RcsUnit = home.RcsUnit
                    }
                );

                //Add to database
                /*
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    db.Provider_Homes.Add(new Provider_Homes { 
                        PHome_ID = Convert.ToInt64(home.HomeID),
                        PHome_Address = home.Address,
                        PHome_City = home.City,
                        PHome_Zipcode = home.Zipcode,
                        PHome_Phonenumber = "don't call",
                        FK_Provider_ID = Convert.ToInt64(home.ProviderID) });
                    db.SaveChanges();

                    Random randomiz = new Random();
                    int id = randomiz.Next(111, 8000 + 1);
                    while (true)
                    {
                        var scheduled = db.Scheduled_Inspections.Where(r => r.SInspections_Id == id).ToList();
                        if (scheduled.Count != 0)
                        {
                            id = randomiz.Next(111, 8000 + 1);//456789
                        }
                        else
                        {
                            break;
                        }
                    }

                    db.Scheduled_Inspections.Add(new Scheduled_Inspections { 
                        SInspections_Id = id,
                        SInspections_Date = alg.ConvertDateToString(home.InspectionDate),
                        FK_PHome_ID = Convert.ToInt64(home.HomeID) }
                    );
                    db.SaveChanges();

                }
                */
                MessageService.ReleaseMessageBox("New Home has been added to the database");
            }


        }
        private async void ExecuteCompleteDialog(object o)
        {
            if (SelectedSchedule == null)
            {
                var view = new NoHomeSelectedErrorDialog();

                var result = await DialogHost.Show(view, "WindowDialogs", GenericClosingEventHandler);
            }
            else
            {
                var view = new CompleteDialog
                {
                    DataContext = new CompleteVM(SelectedSchedule)
                };

                var result = await DialogHost.Show(view, "WindowDialogs", ClosingEventHandler);
            }
        }

        private async void InactiveListDisplayAsync(object obj)
        {
            var vm = new InactiveHomeListVM(InActiveHomes);
            var view = new InactiveHomeList(vm);
            var result = await DialogHost.Show(view, "WindowDialogs", ClosingEventHandlerNewHome);
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
                            reactive.EighteenthMonthDate = alg.DropDateMonth(reactive.NextInspection, false);
                        }
                    }
                    Providers.Add(reactive);
                    InActiveHomes.Remove(reactive);
                }
            }
        }

        private void DeleteHome(object obj)
        {
            if (SelectedSchedule == null)
            {
                MessageService.ReleaseMessageBox("Please select a home to delete.");
                return;
            }
            if (MessageService.MessageConfirmation("Are you sure you want to delete " + SelectedSchedule.Address + "?" +
                " It will be removed from the database.", "Deleting Home"))
            {
                if (MessageService.MessageConfirmation("Are you TRULY sure you want to delete " + SelectedSchedule.Address + "?"
                    , "Deleting Home"))
                {
                    using (HomeInspectionEntities db = new HomeInspectionEntities())
                    {
                        try
                        {
                            Provider_Homes deletingHome = db.Provider_Homes.First(r => r.PHome_ID == SelectedSchedule.HomeID);
                            db.Provider_Homes.Remove(deletingHome);
                            db.SaveChanges();

                            Scheduled_Inspections deletingSchedule = db.Scheduled_Inspections.First(r => r.FK_PHome_ID == SelectedSchedule.HomeID
                            && r.SInspections_Date == SelectedSchedule.NextInspection);
                            db.Scheduled_Inspections.Remove(deletingSchedule);
                            db.SaveChanges();
                            Providers.Remove(SelectedSchedule);
                            MessageService.ReleaseMessageBox("Selected Home has been removed from the table.");
                            RefreshTable("");
                        }
                        catch (InvalidOperationException e)
                        {
                            MessageService.ReleaseMessageBox("Selected Home removal failed.");
                        }
                    }
                }
            }
        }



        private async void ExecuteEditDialog(object o)
        {
            if (SelectedHome == null)
            {
                var view = new NoHomeSelectedErrorDialog();

                var result = await DialogHost.Show(view, "WindowDialogs", GenericClosingEventHandler);
            }
            else
            {
                var view = new EditDialog();

                view.setDataContext(SelectedHome);

                var result = await DialogHost.Show(view, "WindowDialogs", ClosingEventHandler);

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
        private void ClosingEventHandlerNewHome(object sender, DialogClosingEventArgs eventArgs)

        {
            Console.WriteLine("Dialog closed successfully");
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogHostSuccess = false;
                return;
            }
            DialogHostSuccess = true;
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
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
                    var nextInspection = editedHomeData.NextInspection;

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
                        SelectedHome.NextInspection = nextInspection;

                        db.SaveChanges();
                    }
                }
            }
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
            GenData();
            foreach (var provider in Providers)
            {
                GenHistoryData(provider);
            }
            //SelectedSchedule = Providers.First();
            //SelectedSchedule.IsSelected = true;
        }

        private void FilterTheTable(object obj)
        {
            RefreshTable(obj);//Comment out if you want to test the license number/name filter
            if (SelectedFilter is null)
            {
                MessageService.ReleaseMessageBox("You have not specified what to filter out.");
                return;
            }
            if (FilterItem.Equals("") && !DatePickerEnabled)
            {
                return;
            }

            var temp = new ObservableCollection<HomeModel>();

            /*if (SelectedFilter.ToString().Contains("Provider ID"))
            {
                foreach(var item in Providers)
                {
                    if (item.ProviderID.ToString().Equals(FilterItem))
                    {
                        temp.Add(item);                        
                    }
                }
            }*/

            if (SelectedFilter.ToString().Contains("Provider Name"))
            {
                foreach (var item in Providers)
                {
                    if (item.ProviderName.Contains(FilterItem))
                    {
                        temp.Add(item);
                    }
                }
            }

            else if (SelectedFilter.ToString().Contains("Address"))
            {
                foreach (var item in Providers)
                {
                    if (item.Address.Contains(FilterItem))
                    {
                        temp.Add(item);
                    }
                }
            }

            else if (SelectedFilter.ToString().Contains("License Number"))
            {
                foreach (var item in Providers)
                {
                    if (item.HomeLicenseNum.ToString().Contains(FilterItem))
                    {
                        temp.Add(item);
                    }
                }
            }

            else if (SelectedFilter.ToString().Contains("License Name"))
            {
                foreach (var item in Providers)
                {
                    if (item.HomeName.Contains(FilterItem))
                    {
                        temp.Add(item);
                    }
                }
            }

            else if (SelectedFilter.ToString().Contains("Next Inspection Date"))
            {
                foreach (var item in Providers)
                {
                    if (IsInspectionWithinDateRange(item.NextInspection))
                    {
                        temp.Add(item);
                    }
                }
            }

            Providers.Clear();
            foreach (var returnItem in temp)
            {
                Providers.Add(returnItem);
            }

        }
        #endregion

        #region Helper Methdos
        private bool IsInspectionWithinDateRange(string nextInspection)
        {
            var inspectDate = alg.ExtractDateTime(nextInspection);
            if ((DateTime.Compare(inspectDate, StartDatePicked) >= 0) && (DateTime.Compare(inspectDate, EndDatePicked) <= 0))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region User Methods
        public void ClearUser() { _usr = null; }
        #endregion
    }
}
