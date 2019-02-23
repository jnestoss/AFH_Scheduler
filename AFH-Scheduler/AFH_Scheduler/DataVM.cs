
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

namespace AFH_Scheduler
{
    public class DataVM : ObservableObject, IPageViewModel
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();

        private ScheduleModel _selectedSchedule;
        private ScheduleModel SelectedSchedule {
            get { return _selectedSchedule; }
            set {
                if (_selectedSchedule == value) return;
                _selectedSchedule = value;
            }
        }

        //Observable and bound to DataGrid
        private static ObservableCollection<ScheduleModel> _providers;
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

        private ScheduleModel _selectedHome;
        public ScheduleModel SelectedHome {
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

        private void FilterTheTable(object obj)
        {
            RefreshTable(obj);
            if (SelectedFilter is null)
            {
                MessageService.ReleaseMessageBox("You have not specified what to filter out.");
                return;
            }
            if (FilterItem.Equals("") && !DatePickerEnabled)
            {
                return;
            }

            var temp = new ObservableCollection<ScheduleModel>();
            if (SelectedFilter.ToString().Contains("Provider ID"))
            {
                foreach(var item in Providers)
                {
                    if (item.ProviderID.ToString().Equals(FilterItem))
                    {
                        temp.Add(item);                        
                    }
                }
            }

            else if(SelectedFilter.ToString().Contains("Name"))
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

        private bool IsInspectionWithinDateRange(string nextInspection)
        {
            var inspectDate = alg.ExtractDateTime(nextInspection);
            if((DateTime.Compare(inspectDate, StartDatePicked) >= 0) && (DateTime.Compare(inspectDate, EndDatePicked) <= 0))
            {
                return true;
            }
            return false;
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

        private void EditHistoryDialogOpen(object obj)
        {
            /*HistoryModel historyModel = (HistoryModel)obj;
            HistoryDetailViewVM historyDetailView = new HistoryDetailViewVM(historyModel.HomeID, MessageService);
            var updateOrNot = MessageService.ShowDialog(historyDetailView);*/
            MessageService.ReleaseMessageBox("Outcome Code can not be edited currently.");
        }

        private RelayCommand _refreshTable;
        public ICommand RefreshTableCommand
        {
            get
            {
                if (_refreshTable == null)
                    _refreshTable = new RelayCommand(RefreshTable);
                return _refreshTable;
            }
        }

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

        private RelayCommand _importTableCommand;
        public ICommand ImportTableCommand
        {
            get
            {
                if (_importTableCommand == null)
                    _importTableCommand = new RelayCommand(ImportExcelTable);
                return _importTableCommand;
            }
        }
        private async void ImportExcelTable(object obj)
        {
            var importData = new ImportDataPreviewVM();
            var view = new ImportDataPreview(importData);
            var result = await DialogHost.Show(view, "WindowDialogs", ClosingEventHandlerNewHome);
            if (result.Equals("IMPORT"))
            {
                foreach(var importedHome in importData.ImportedHomes)
                {
                    Providers.Add(importedHome);
                }
            }
        }

        private RelayCommand _exportTable;
        public ICommand ExportTableCommand
        {
            get
            {
                if (_exportTable == null)
                    _exportTable = new RelayCommand(ExportTable);
                return _exportTable;
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
                        Visible = true
                    };
                    
                    try
                    {//Excel work here
                        xlWorkbook = xlApp.Workbooks.Add("");
                        xlWorksheet = (Worksheet)xlWorkbook.ActiveSheet;
                        
                        xlWorksheet.Cells[1, 1] = "License Number";
                        xlWorksheet.Cells[1, 2] = "Provider";
                        xlWorksheet.Cells[1, 3] = "Address";
                        xlWorksheet.Cells[1, 4] = "City";
                        xlWorksheet.Cells[1, 5] = "Zipcode";
                        xlWorksheet.Cells[1, 6] = "Recent Inspection Date";
                        xlWorksheet.Cells[1, 7] = "Next Inspection Date";
                        xlWorksheet.Cells[1, 8] = "18th Month Drop Dead";

                        int row = 2;
                        foreach (var provider in Providers)
                        {
                            var home = db.Provider_Homes.Where(r => r.PHome_Address == provider.Address).First();                            

                            xlWorksheet.Cells[row, 1] = provider.ProviderID;
                            xlWorksheet.Cells[row, 2] = provider.ProviderName;
                            xlWorksheet.Cells[row, 3] = provider.Address;
                            xlWorksheet.Cells[row, 4] = home.PHome_City;
                            xlWorksheet.Cells[row, 5] = home.PHome_Zipcode;
                            xlWorksheet.Cells[row, 6] = provider.RecentInspection;
                            xlWorksheet.Cells[row, 7] = provider.NextInspection;
                            xlWorksheet.Cells[row, 8] = provider.EighteenthMonthDate;

                            row++;
                        }

                        xlWorksheet.get_Range("A1", "H1").EntireColumn.AutoFit();

                        //xlApp.Visible = false;
                        //xlApp.UserControl = false;
                        if(fileName.Contains(".xlsx"))
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

        private RelayCommand _addNewHomeCommand;
        public ICommand AddNewHomeCommand
        {
            get
            {
                if (_addNewHomeCommand == null)
                    _addNewHomeCommand = new RelayCommand(CreateNewHomeAsync);
                return _addNewHomeCommand;
            }
        }

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
                  new ScheduleModel
                  (
                  Convert.ToInt64(home.ProviderID),
                  Convert.ToInt64(home.HomeID),
                  home.ProviderName,
                  "",//Home Name
                  "",//Phone Number
                  home.Address,
                  home.City,
                  home.Zipcode,
                  recentDate,
                  alg.ConvertDateToString(home.InspectionDate),//insp,
                  this,
                  alg.SettingEighteenthMonth(alg.ConvertDateToString(home.InspectionDate))
                  )
                  );

                //Add to database
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    db.Provider_Homes.Add(new Provider_Homes { PHome_ID = Convert.ToInt64(home.HomeID),
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

                    db.Scheduled_Inspections.Add(new Scheduled_Inspections { SInspections_Id = id,
                        SInspections_Date = alg.ConvertDateToString(home.InspectionDate),
                        FK_PHome_ID = Convert.ToInt64(home.HomeID) }
                    );
                    db.SaveChanges();

                }

                MessageService.ReleaseMessageBox("New Home has been added to the database");
            }

            
        }

        private RelayCommand _deleteHomeCommand;
        public ICommand DeleteHomeCommand
        {
            get
            {
                if (_deleteHomeCommand == null)
                    _deleteHomeCommand = new RelayCommand(DeleteHome);
                return _deleteHomeCommand;
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
                        catch(InvalidOperationException e)
                        {
                            MessageService.ReleaseMessageBox("Selected Home removal failed.");
                        }
                    }
                }
            }
        }

        public DataVM()
        {
            _providers = new ObservableCollection<ScheduleModel>();
            FilterItem = "";
            TextFieldEnabled = true;
            StartDatePicked = DateTime.Today;
            EndDatePicked = DateTime.Today;

            GenData();

            

            foreach(var provider in Providers)
            {
                GenHistoryData(provider);
            }
        }

        public string Name {
            get {
                return "Schedules";
            }
        }

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
                            new ScheduleModel
                            (
                                item.Provider_ID,
                                house.PHome_ID,
                                item.Provider_Name, //Provider Name
                                "",//Home Name
                                house.PHome_Phonenumber,
                                house.PHome_Address,//Address
                                house.PHome_City,
                                house.PHome_Zipcode,
                                recentDate,
                                insp,
                                this,
                                alg.SettingEighteenthMonth(insp)
                            )
                        );
                    }
                }

            }
        }

        public void GenHistoryData(ScheduleModel house)
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



        public RelayCommand RunEditDialogCommand => new RelayCommand(ExecuteEditDialog);

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


        public RelayCommand RunCompleteDialogCommand => new RelayCommand(ExecuteCompleteDialog);

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
                ScheduleModel editedHomeData = ((EditVM)((EditDialog)eventArgs.Session.Content).DataContext).SelectedSchedule;
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    var homeID = editedHomeData.HomeID;
                    var providerID = editedHomeData.ProviderID;
                    var address = editedHomeData.Address;
                    var city = editedHomeData.City;
                    var zip = editedHomeData.ZIP;
                    var nextInspection = editedHomeData.NextInspection;

                    var foo = EditVM._homeIDSave;

                    Provider_Homes selectHome = db.Provider_Homes.FirstOrDefault(r => r.PHome_ID == foo);

                    db.Provider_Homes.Remove(selectHome);

                    //create new record here


                    if (selectHome != null)
                    {
                        //selectHome.PHome_ID = homeID;
                        selectHome.FK_Provider_ID = providerID;
                        selectHome.PHome_Address = address;
                        selectHome.PHome_City = city;
                        selectHome.PHome_Zipcode = zip;
                        //selectHome.Scheduled_Inspections.FirstOrDefault(r => r.FK_PHome_ID == foo).SInspections_Date = nextInspection;

                        //db.Entry(selectHome).State = selectHome.PHome_ID == EditVM.HomeIDSave ? EntityState.Added : EntityState.Modified;

                        db.SaveChanges();
                    }
                }
            }
        }
        

        private void GenericClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("Dialog closed successfully");
        }


    }
}
