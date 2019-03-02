﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using Microsoft.Office.Interop.Excel;

namespace AFH_Scheduler.Dialogs
{
    public class ImportDataPreviewVM : ObservableObject, IPageViewModel
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();
        private static ObservableCollection<ScheduleModel> _importedHomes;
        public ObservableCollection<ScheduleModel> ImportedHomes
        {
            get { return _importedHomes; }
            set
            {
                if (value != _importedHomes)
                {
                    _importedHomes = value;
                    OnPropertyChanged("ImportedHomes");
                }
            }
        }

        private List<long> _uniqueProvIDs;
        public List<long> UniqueProvIDs
        {
            get { return _uniqueProvIDs; }
            set
            {
                if (value != _uniqueProvIDs)
                {
                    _uniqueProvIDs = value;
                    OnPropertyChanged("UniqueProvIDs");
                }
            }
        }

        private List<long> _uniqueHomeIDs;
        public List<long> UniqueHomeIDs
        {
            get { return _uniqueHomeIDs; }
            set
            {
                if (value != _uniqueHomeIDs)
                {
                    _uniqueHomeIDs = value;
                    OnPropertyChanged("UniqueHomeIDs");
                }
            }
        }

        private List<List<string>> _importedLicenseInfo;
        public List<List<string>> ImportedLicenseInfo
        {
            get { return _importedLicenseInfo; }
            set
            {
                if (value != _importedLicenseInfo)
                {
                    _importedLicenseInfo = value;
                    OnPropertyChanged("ImportedLicenseInfo");
                }
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

        public ImportDataPreviewVM()
        {
            _importedHomes = new ObservableCollection<ScheduleModel>();
            _importedLicenseInfo = new List<List<string>>();
            _uniqueProvIDs = new List<long>();
            _uniqueHomeIDs = new List<long>();
        }


        private RelayCommand _importDataCommand;
        public ICommand ImportDataCommand
        {
            get
            {
                if (_importDataCommand == null)
                    _importDataCommand = new RelayCommand(OpenExcelFileImport);
                return _importDataCommand;
            }
        }

        private void OpenExcelFileImport(object obj)
        {
            foreach (var listRow in ImportedLicenseInfo)
                listRow.Clear();
            ImportedLicenseInfo.Clear();
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
                    string filename = MessageService.ExcelOpenDialog();
                    if(filename is null)
                    {
                        return;
                    }
                    xlWorkbook = xlApp.Workbooks.Open(filename);
                    xlWorksheet = xlWorkbook.Worksheets[1];
                    var header = xlWorksheet.UsedRange.Columns;

                    foreach (Range col in header)
                    {
                        var colCell = col.Cells.Value2;

                        if (colCell[1, 1].IndexOf("ProviderName", StringComparison.OrdinalIgnoreCase) > -1)//Provider name
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in colCell)
                            {
                                if (cel == null)
                                    ImportedLicenseInfo[0].Add("No Provider");
                                else
                                ImportedLicenseInfo[0].Add(cel.ToString());
                            }
                        }

                        if (colCell[1,1].IndexOf("LicenseNumber", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in colCell)
                            {
                                ImportedLicenseInfo[1].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("FacilityName", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                ImportedLicenseInfo[2].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("LocationAddress", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                ImportedLicenseInfo[3].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("LocationCity", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                ImportedLicenseInfo[4].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("LocationZipCode", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                ImportedLicenseInfo[5].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("TelephoneNmbr", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                if(cel == null)
                                    ImportedLicenseInfo[6].Add("");
                                else
                                    ImportedLicenseInfo[6].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("NextInspection", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            String[] splitDate;
                            foreach (var cel in col.Cells.Value)
                            {
                                splitDate = cel.ToString().Split(' ');
                                var actualDate = splitDate[0];
                                ImportedLicenseInfo[7].Add(actualDate);
                            }
                        }
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
            LoadInToTable();
        }

        public void LoadInToTable()
        {
            /*  ImportedLicenseInfo[0][0]; //Provider Name
                ImportedLicenseInfo[1][0]; //LicenseNum
                ImportedLicenseInfo[2][0]; //Home Name
                ImportedLicenseInfo[3][0]; //Address
                ImportedLicenseInfo[4][0]; //City
                ImportedLicenseInfo[5][0]; //Zipcode
                ImportedLicenseInfo[6][0]; //Telephone
                ImportedLicenseInfo[7][0]; //NextInspection*/

            int rows = ImportedLicenseInfo[0].Count;
            for (int rowItem = 1; rowItem < rows; rowItem++)
            {
                ImportedHomes.Add(// * = From the Excel file
                                new ScheduleModel
                                (
                                    GenerateProviderID(),   //Provider ID
                                    GenerateHomeID(),     //Home Database ID
                                    ImportedLicenseInfo[0][rowItem],                //Provider Name*
                                    Convert.ToInt64(ImportedLicenseInfo[1][rowItem]),//License Number*
                                    ImportedLicenseInfo[2][rowItem],     //Home Name*
                                    ImportedLicenseInfo[6][rowItem],     //Phone Number*
                                    ImportedLicenseInfo[3][rowItem],     //Address*
                                    ImportedLicenseInfo[4][rowItem],     //City*
                                    ImportedLicenseInfo[5][rowItem],     //Zip*
                                    "",         //Recent
                                    ImportedLicenseInfo[7][rowItem],               //Next Inspection*
                                    null,               //DataVM
                                    alg.SettingEighteenthMonth(ImportedLicenseInfo[7][rowItem])//18th Month Drop Date
                                )
                            );
            }
        }

        public long GenerateProviderID()
        {
            long newID;
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                try
                {
                    var recentProviderID = db.Providers.OrderByDescending(r => r.Provider_ID).FirstOrDefault();
                    if (recentProviderID.Provider_ID == Int64.MaxValue)
                    {
                        newID = 0;
                        while (true)
                        {
                            var isUniqueID = db.Providers.Where(r => r.Provider_ID == newID).ToList();
                            if (isUniqueID.Count == 0 && !UniqueProvIDs.Contains(newID))
                            {
                                UniqueProvIDs.Add(newID);
                                return newID;
                            }
                            newID++;
                        }
                    }
                    else
                        newID = recentProviderID.Provider_ID + 1;
                    while(UniqueProvIDs.Contains(newID))
                    {
                        newID++;
                    }
                    UniqueProvIDs.Add(newID);
                    return newID;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

        public long GenerateHomeID()
        {
            long newID;
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                try
                {
                    var recentHomeID = db.Provider_Homes.OrderByDescending(r => r.PHome_ID).FirstOrDefault();
                    if (recentHomeID.PHome_ID == Int64.MaxValue)
                    {
                       newID = 0;
                        while (true)
                        {
                            var isUniqueID = db.Provider_Homes.Where(r => r.PHome_ID == newID).ToList();
                            if (isUniqueID.Count == 0 && !UniqueHomeIDs.Contains(newID))
                            {
                                UniqueHomeIDs.Add(newID);
                                return newID;
                            }
                            newID++;
                        }
                    }
                    else
                        newID = recentHomeID.PHome_ID + 1;
                    while(UniqueHomeIDs.Contains(newID))
                    {
                        newID++;
                    }
                    UniqueHomeIDs.Add(newID);
                    return newID;
                }
                catch (Exception e)
                {
                    return 0;
                }
            }
        }

        public string Name
        {
            get
            {
                return "Import Table from file";
            }
        }
    }
}