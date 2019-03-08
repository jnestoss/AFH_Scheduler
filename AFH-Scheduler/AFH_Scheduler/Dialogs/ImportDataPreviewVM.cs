using System;
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
        private static ObservableCollection<HomeModel> _importedHomes;
        public ObservableCollection<HomeModel> ImportedHomes
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
            _importedHomes = new ObservableCollection<HomeModel>();
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
            int pocRow = 0, licenseRow = 0, nameRow = 0, addressRow = 0, cityRow = 0, zipRow = 0,
                phoneRow = 0, inspRow = 0, rcsRow = 0;
            foreach (var listRow in ImportedLicenseInfo)
                listRow.Clear();
            ImportedLicenseInfo.Clear();
            try
            { //We write to an Excel file
                Microsoft.Office.Interop.Excel.Application xlApp;
                Workbook xlWorkbook;
                Worksheet xlWorksheet;

                xlApp = new Application
                {
                    //Visible = true
                };

                try
                {//Excel work here
                    string filename = MessageService.ExcelOpenDialog();
                    if(filename == null)
                    {
                        return;
                    }
                    xlWorkbook = xlApp.Workbooks.Open(filename);
                    try {

                        xlWorksheet = (Worksheet)xlWorkbook.Worksheets[1];
                        var header = xlWorksheet.UsedRange.Columns;

                        int index = -1;
                        foreach (Range col in header)
                        {

                            dynamic colCell = col.Cells.Value2;

                            if (colCell[1, 1].IndexOf("FacilityPOC", StringComparison.OrdinalIgnoreCase) > -1)//Provider name
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                pocRow = index;
                                foreach (var cel in colCell)
                                {
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("No Provider");
                                    else
                                        ImportedLicenseInfo[index].Add(cel.ToString());
                                }
                            }

                            if (colCell[1, 1].IndexOf("LicenseNumber", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                licenseRow = index;
                                foreach (var cel in colCell)
                                {
                                    ImportedLicenseInfo[index].Add(cel.ToString());
                                }
                            }
                            else if (colCell[1, 1].IndexOf("FacilityName", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                nameRow = index;
                                foreach (var cel in (dynamic)col.Cells.Value)
                                {
                                    ImportedLicenseInfo[index].Add(cel.ToString());
                                }
                            }
                            else if (colCell[1, 1].IndexOf("LocationAddress", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                addressRow = index;
                                foreach (var cel in (dynamic)col.Cells.Value)
                                {
                                    ImportedLicenseInfo[index].Add(cel.ToString());
                                }
                            }
                            else if (colCell[1, 1].IndexOf("LocationCity", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                cityRow = index;
                                foreach (var cel in (dynamic)col.Cells.Value)
                                {
                                    ImportedLicenseInfo[index].Add(cel.ToString());
                                }
                            }
                            else if (colCell[1, 1].IndexOf("LocationZipCode", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                zipRow = index;
                                foreach (var cel in (dynamic)col.Cells.Value)
                                {
                                    ImportedLicenseInfo[index].Add(cel.ToString());
                                }
                            }
                            else if (colCell[1, 1].IndexOf("TelephoneNmbr", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                phoneRow = index;
                                foreach (var cel in (dynamic)col.Cells.Value)
                                {
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("");
                                    else
                                        ImportedLicenseInfo[index].Add(cel.ToString());
                                }
                            }
                            else if (colCell[1, 1].IndexOf("NextInspection", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                inspRow = index;
                                String[] splitDate;
                                foreach (var cel in (dynamic)col.Cells.Value)
                                {
                                    splitDate = cel.ToString().Split(' ');
                                    var actualDate = splitDate[0];
                                    ImportedLicenseInfo[index].Add(actualDate);
                                }
                            }
                            else if (colCell[1, 1].IndexOf("RCSRegionUnit", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                rcsRow = index;
                                foreach (var cel in (dynamic)col.Cells.Value)
                                {
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("");
                                    else
                                        ImportedLicenseInfo[index].Add(cel.ToString());
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
                        xlWorkbook.Close(false);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Problem with Excel " + e.ToString());

                }
                finally
                {
                    xlApp.Quit();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Problem with Excel " + e.ToString());
            }
            LoadInToTable(pocRow, licenseRow, nameRow, addressRow, cityRow, zipRow, phoneRow, inspRow, rcsRow);
        }

        public void LoadInToTable(int pocRow, int licenseRow, int nameRow, int addressRow, int cityRow, 
            int zipRow, int phoneRow, int inspRow, int rcsRow)
        {
            int rows = ImportedLicenseInfo[0].Count;
            long provID;
            string provName;
            for (int rowItem = 1; rowItem < rows; rowItem++)
            {
                if (ImportedLicenseInfo[pocRow][rowItem] == null || ImportedLicenseInfo[pocRow][rowItem].Length == 0 
                    || ImportedLicenseInfo[pocRow][rowItem].Equals("") || 
                    ImportedLicenseInfo[pocRow][rowItem].Equals("No Provider"))
                {
                    provID = -1;
                    provName = "No Provider";
                }
                else
                {
                    provName = ImportedLicenseInfo[pocRow][rowItem];
                    using (HomeInspectionEntities db = new HomeInspectionEntities())
                    {
                        var prov = db.Providers.Where(r => r.Provider_Name.Equals(provName)).ToList();
                        if(prov.Count != 0) //New Provider
                        {
                            provID = prov[0].Provider_ID;
                        }
                        else
                        {
                            provID = GenerateProviderID();
                        }
                    }
                }
                ImportedHomes.Add(// * = From the Excel file
                                new HomeModel
                                {
                                    ProviderID = provID,
                                    HomeID = GenerateHomeID(),     //Home Database ID
                                    ProviderName = provName,                //Provider Name*
                                    HomeLicenseNum = Convert.ToInt64(ImportedLicenseInfo[licenseRow][rowItem]),//License Number*
                                    HomeName = ImportedLicenseInfo[nameRow][rowItem],     //Home Name*
                                    Phone = ImportedLicenseInfo[phoneRow][rowItem],     //Phone Number*
                                    Address = ImportedLicenseInfo[addressRow][rowItem],     //Address*
                                    City = ImportedLicenseInfo[cityRow][rowItem],     //City*
                                    ZIP = ImportedLicenseInfo[zipRow][rowItem],     //Zip*
                                    RecentInspection = "",         //Recent
                                    NextInspection = ImportedLicenseInfo[inspRow][rowItem],               //Next Inspection*
                                    EighteenthMonthDate = alg.DropDateMonth(ImportedLicenseInfo[inspRow][rowItem], false),//18th Month Drop Date
                                    HasNoProvider = true,
                                    RcsRegion = ImportedLicenseInfo[rcsRow][rowItem]//RCSRegionUnit*
                                }
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
