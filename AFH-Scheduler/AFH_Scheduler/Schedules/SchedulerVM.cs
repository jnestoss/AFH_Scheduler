
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
using System.IO;

namespace AFH_Scheduler.Schedules
{
    public class SchedulerVM : ObservableObject, IPageViewModel
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
                        xlWorkbook.SaveAs(fileName, FileFormat: XlFileFormat.xlWorkbookDefault);
                        //xlWorkbook.SaveAs(ExcelFileName + "\\TestSchedule.xlsx", FileFormat: XlFileFormat.xlWorkbookDefault);
                        //xlWorkbook.SaveAs("C:\\excelLocationTest\\TestSchedule.xlsx", FileFormat: XlFileFormat.xlWorkbookDefault);


                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Problem with Excel " + e.ToString());

                    }
                    finally
                    {
                        xlApp.Workbooks.Close();
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
            var createdHome = new NewHomeCreatorVM(MessageService);
            var view = new NewHomeDialog(createdHome);
            var result = await DialogHost.Show(view, "EditDialog", ClosingEventHandler);

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
                  home.Address,
                  home.City,
                  home.Zipcode,
                  recentDate,
                  alg.ConvertDateToString(home.InspectionDate),//insp,
                  this,
                  alg.SettingEighteenthMonth(alg.ConvertDateToString(home.InspectionDate))
                  )
                  );
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
                    Providers.Remove(SelectedSchedule);
                    MessageService.ReleaseMessageBox("Selected Home has been removed from the table.");
                }
            }
            ClearSelected2(null);
        }

        public SchedulerVM()
        {
            _providers = new ObservableCollection<ScheduleModel>();
            FilterItem = "";
            TextFieldEnabled = true;
            StartDatePicked = DateTime.Today;
            EndDatePicked = DateTime.Today;
            /*
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                Home_History deletingHistory = db.Home_History.First(r => r.HHistory_Date == "3/2/2016" && r.FK_PHome_ID == 1111);
                db.Home_History.Remove(deletingHistory);
                db.SaveChanges();
                var temp = db.Scheduled_Inspections.ToList();
                foreach (var removeThis in temp)
                {
                    Scheduled_Inspections deletingSchedule = db.Scheduled_Inspections.Where(r => r.FK_PHome_ID == removeThis.FK_PHome_ID && r.SInspections_Date.Equals(removeThis.SInspections_Date)).First();
                    db.Scheduled_Inspections.Remove(deletingSchedule);
                    db.SaveChanges();
                }
                int id = 111;
                var provs = db.Providers.ToList();
                foreach (var item in provs)
                {
                    var homes = db.Provider_Homes.Where(r => r.FK_Provider_ID == item.Provider_ID).ToList();
                    foreach (var house in homes)
                    {
                        db.Scheduled_Inspections.Add(new Scheduled_Inspections { SInspections_Id = id, SInspections_Date = alg.SchedulingNextDate(Convert.ToInt32(house.PHome_ID)), FK_PHome_ID = house.PHome_ID });
                        db.SaveChanges();
                        id++;
                    }
                }
            }*/

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

        public void ClearSelected2(ScheduleModel sm)
        {
            if (SelectedSchedule != null)
                SelectedSchedule.IsSelected = false;
            SelectedSchedule = sm;
        }

        //clear all selected items
        public void ClearSelected()
        {
            //for (int i = 0; i < Providers.Count; i++)
            //{
                
            var item = Providers.Where(X => X.IsSelected == true);
            foreach (ScheduleModel p in item)
            {
                p.IsSelected = false;
            }
            //Providers.Wher
            //while (item != null)
            //{
            //    if (item != null) item.IsSelected = false;
            //    item = Providers.Where(X => X.IsSelected == true).FirstOrDefault();
            //}
            //}

            //foreach(ScheduleModel sm in Providers)
            //{
            //    sm.IsSelected = false;
            //}
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
                                item.Provider_Name, //Phone Number
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



        public ICommand RunEditDialogCommand => new RelayCommand(ExecuteEditDialog);

        private async void ExecuteEditDialog(object o)
        {
            if (SelectedSchedule == null)
            {
                var view = new NoHomeSelectedErrorDialog();
                
                var result = await DialogHost.Show(view, ClosingEventHandler2);
            }
            else
            {
                var view = new EditDialog();

                Console.WriteLine(SelectedSchedule.ProviderID);
                Console.WriteLine(SelectedSchedule.ProviderName);
                Console.WriteLine(SelectedSchedule.HomeID);
                Console.WriteLine(SelectedSchedule.Address);
                Console.WriteLine(SelectedSchedule.City);
                Console.WriteLine(SelectedSchedule.ZIP);
                Console.WriteLine(SelectedSchedule.NextInspection);
                Console.WriteLine(SelectedSchedule.IsSelected);

                view.setDataContext(SelectedSchedule);

                //if (view.DataContext == null) Environment.Exit(0);

                var result = await DialogHost.Show(view, "EditDialog", ClosingEventHandler); 

                Console.WriteLine(result);
            }
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs) 
        {
            Console.WriteLine("Dialog closed successfully");
            if ((String)eventArgs.Parameter == "Cancel")
            {
                DialogHostSuccess = false;
                return;
            }
            DialogHostSuccess = true;

            //((EditDialog)eventArgs.Session.Content).
            //Console.WriteLine(eventArgs.OriginalSource);


            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var homes = db.Providers.ToList();
            }
            
        }

        private void ClosingEventHandler2(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("");
        }

    }
}
