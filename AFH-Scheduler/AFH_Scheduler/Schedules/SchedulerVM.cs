
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
using Microsoft.Office.Interop.Excel;
using System.IO;

namespace AFH_Scheduler.Schedules
{
    public class SchedulerVM : ObservableObject, IPageViewModel
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();
        //Observable and bound to DataGrid
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

        public string _excelFilename;
        public string ExcelFileName
        {
            get { return _excelFilename; }
            set
            {
                _excelFilename = value;
                OnPropertyChanged("ExcelFileName");
            }
        }

        private OpenMessageDialogService _messageService;
        public OpenMessageDialogService MessageService
        {
            get
            {
                if (_messageService == null)
                    _messageService = new SchedulesOpenDialog();
                return _messageService;
            }
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
            if (ExcelFileName.Equals("") || !(Directory.Exists(ExcelFileName)))
            {
                MessageService.ReleaseMessageBox("Directory not found");
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
                        xlWorkbook.SaveAs(ExcelFileName + "\\TestSchedule.xlsx", FileFormat: XlFileFormat.xlWorkbookDefault);
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

        public SchedulerVM()
        {
            _providers = new ObservableCollection<ScheduleModel>();
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
                        var insp = db.Scheduled_Inspections.Where(r => r.FK_PHome_ID == house.PHome_ID).First().SInspections_Date;
                        //Console.WriteLine(item. + "*************************************************************************************************");
                        Providers.Add(
                            new ScheduleModel
                            (
                                item.Provider_ID,
                                item.Provider_Name,
                                house.PHome_Phonenumber, //Phone Number
                                house.PHome_Address,//Address
                                alg.GrabbingRecentInspection(Convert.ToInt32(house.PHome_ID)).HHistory_Date,
                                insp,
                                alg.SettingEighteenthMonth(insp)
                            )
                        );
                    }
                }

            }
        }


    }
}
