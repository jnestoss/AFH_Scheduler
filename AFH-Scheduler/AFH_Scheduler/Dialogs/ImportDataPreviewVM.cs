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
using AFH_Scheduler.Dialogs.Errors;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
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
            int pocRow = -1, licenseRow = -1, nameRow = -1, addressRow = -1, cityRow = -1, zipRow = -1,
                phoneRow = -1, inspRow = -1, rcsRow = -1, recentRow = -1;
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
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("");
                                    else
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
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("");
                                    else
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
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("");
                                    else
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
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("");
                                    else
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
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("");
                                    else
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
                            else if (colCell[1, 1].IndexOf("RecentInspection", StringComparison.OrdinalIgnoreCase) > -1)
                            {
                                ImportedLicenseInfo.Add(new List<string>());
                                index++;
                                recentRow = index;
                                String[] splitDate;
                                foreach (var cel in (dynamic)col.Cells.Value)
                                {
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("");
                                    else
                                    {
                                        splitDate = cel.ToString().Split(' ');
                                        var actualDate = splitDate[0];
                                        ImportedLicenseInfo[index].Add(actualDate);
                                    }
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
                                    if (cel == null)
                                        ImportedLicenseInfo[index].Add("");
                                    else
                                    {
                                        splitDate = cel.ToString().Split(' ');
                                        var actualDate = splitDate[0];
                                        ImportedLicenseInfo[index].Add(actualDate);
                                    }
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
            LoadInToTable(pocRow, licenseRow, nameRow, addressRow, cityRow, zipRow, phoneRow, inspRow, recentRow, rcsRow);
        }

        public void LoadInToTable(int pocRow, int licenseRow, int nameRow, int addressRow, int cityRow, 
            int zipRow, int phoneRow, int inspRow, int recentRow, int rcsRow)
        {
            int errorCount = 0;
            if (licenseRow == -1 || nameRow ==- 1 || addressRow == -1 || cityRow == -1 || zipRow == -1 || recentRow == -1 || rcsRow == -1)
            {
                string message = "Value(s):";
                if (licenseRow == -1)
                {
                    message += " LicenseNumber,";
                    errorCount++;
                }
                if (nameRow == -1)
                {
                    message += " FacilityName,";
                    errorCount++;
                }
                if (addressRow == -1)
                {
                    message += " LocationAddress,";
                    errorCount++;
                }
                if (cityRow == -1)
                {
                    message += " LocationCity,";
                    errorCount++;
                }
                if (zipRow == -1)
                {
                    message += " LocationZipCode,";
                    errorCount++;
                }
                if(recentRow == -1)
                {
                    message += " RecentInspection,";
                }
                if (rcsRow == -1)
                {
                    message += " RCSRegionUnit,";
                    errorCount++;
                }
                message = message.Remove(message.Count() - 1, 1);
                if (errorCount == 1)
                {
                    message += " is missing. Please include it in the excel sheet.";
                }
                else
                {
                    message += " are missing. Please include them in the excel sheet.";
                }
                MessageService.ReleaseMessageBox(message);
                return;
            }

            var errorlist = new List<ImportErrorModel>();
            int rows = ImportedLicenseInfo[0].Count;

            long provID;
            string provName, nextInspect;
            for (int rowItem = 1; rowItem < rows; rowItem++)
            {
                try
                {
                    if (ImportedLicenseInfo[licenseRow][rowItem].Equals("")
                        || ImportedLicenseInfo[nameRow][rowItem].Equals("")
                        || ImportedLicenseInfo[addressRow][rowItem].Equals("")
                        || ImportedLicenseInfo[cityRow][rowItem].Equals("")
                        || ImportedLicenseInfo[zipRow][rowItem].Equals("")
                        || ImportedLicenseInfo[recentRow][rowItem].Equals("")
                        || ImportedLicenseInfo[rcsRow][rowItem].Equals(""))
                    {
                        string message = "Value(s):";
                        if (ImportedLicenseInfo[licenseRow][rowItem].Equals(""))
                        {
                            message += " LicenseNumber,";
                            errorCount++;
                        }
                        if (ImportedLicenseInfo[nameRow][rowItem].Equals(""))
                        {
                            message += " FacilityName,";
                            errorCount++;
                        }
                        if (ImportedLicenseInfo[addressRow][rowItem].Equals(""))
                        {
                            message += " LocationAddress,";
                            errorCount++;
                        }
                        if (ImportedLicenseInfo[cityRow][rowItem].Equals(""))
                        {
                            message += " LocationCity,";
                            errorCount++;
                        }
                        if (ImportedLicenseInfo[zipRow][rowItem].Equals(""))
                        {
                            message += " LocationZipCode,";
                            errorCount++;
                        }
                        if (ImportedLicenseInfo[recentRow][rowItem].Equals(""))
                        {
                            message += " RecentInspection,";
                            errorCount++;
                        }
                        if (ImportedLicenseInfo[rcsRow][rowItem].Equals(""))
                        {
                            message += " RCSRegionUnit,";
                            errorCount++;
                        }
                        message = message.Remove(message.Count()-1, 1);
                        if(errorCount == 1)
                        {
                            message += " is missing.";
                        }
                        else
                        {
                            message += " are missing.";
                        }
                        errorlist.Add(new ImportErrorModel(rowItem+1, message));
                        errorCount = 0;
                    }
                    else
                    {
                        if (pocRow == -1 || ImportedLicenseInfo[pocRow][rowItem] == null || ImportedLicenseInfo[pocRow][rowItem].Length == 0
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
                                if (prov.Count != 0) //New Provider
                                {
                                    provID = prov[0].Provider_ID;
                                }
                                else
                                {
                                    provID = GenerateProviderID();
                                }
                            }
                        }

                        if (inspRow == -1 || ImportedLicenseInfo[inspRow][rowItem].Equals(""))
                        {
                            using (HomeInspectionEntities db = new HomeInspectionEntities())
                            {
                                var outcome = db.Inspection_Outcome.Where(r => r.IOutcome_Code.Equals("NEW")).First();

                                DateTime scheduleInspect = SchedulingAlgorithm.NextScheduledDate(outcome,
                                           alg.ExtractDateTime(ImportedLicenseInfo[recentRow][rowItem]));
                                if (!alg.CheckingForUniqueInspection(db, scheduleInspect, GenerateHomeID()))
                                {
                                    bool dateCleared = false;
                                    do
                                    {
                                        scheduleInspect.AddDays(1);
                                        SchedulingAlgorithm.CheckDay(scheduleInspect);
                                        if (alg.CheckingForUniqueInspection(db, scheduleInspect, GenerateHomeID()))
                                        {
                                            dateCleared = true;
                                        }
                                    } while (!dateCleared);
                                }
                                nextInspect = scheduleInspect.ToShortDateString();
                            }
                        }
                        else
                        {
                            nextInspect = ImportedLicenseInfo[inspRow][rowItem];
                        }
                        ImportedHomes.Add(// * = Required from the Excel file
                                        new ScheduleModel
                                        (
                                            provID,   //Provider ID
                                            GenerateHomeID(),     //Home Database ID
                                            provName,                //Provider Name
                                            Convert.ToInt64(ImportedLicenseInfo[licenseRow][rowItem]),//License Number*
                                            ImportedLicenseInfo[nameRow][rowItem],     //Home Name*
                                            ImportedLicenseInfo[phoneRow][rowItem],     //Phone Number
                                            ImportedLicenseInfo[addressRow][rowItem],     //Address*
                                            ImportedLicenseInfo[cityRow][rowItem],     //City*
                                            ImportedLicenseInfo[zipRow][rowItem],     //Zip*
                                            ImportedLicenseInfo[recentRow][rowItem],         //Recent
                                            nextInspect,               //Next Inspection*
                                            alg.DropDateMonth(ImportedLicenseInfo[recentRow][rowItem], false),//18th Month Drop Date
                                            true,
                                            ImportedLicenseInfo[rcsRow][rowItem]//RCSRegionUnit*
                                        )
                                    );
                    }
                }
                catch (Exception e)
                {
                    string message = "There has been an error with loading this row";
                    MessageService.ReleaseMessageBox(message);
                }
            }

            if (errorlist.Count > 0)
            {
                /*string message = "There has been an error with loading this row: " + errorlist.Count + " rows were not added to the table.\n" +
                    "Be sure that each row's cells filled in the required columns: " +
                    "LicenseNumber, FacilityName, LocationAddress, LocationCity, LocationZipCode, NextInspection, RCSRegionUnit.";*/
                //MessageService.ReleaseMessageBox(message);
                LoadErrorListAsync(errorlist);
            }
        }

        public async void LoadErrorListAsync(List<ImportErrorModel> errorlist)
        {
            var errorVM = new ImportErrorListVM(errorlist);
            var view = new ImportErrorList(errorVM);
            var result = await DialogHost.Show(view, "ImportErrorDialog", ClosingEventHandlerProviders);
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

        public void ClosingEventHandlerProviders(object sender, DialogClosingEventArgs eventArgs)
        {
            if ((String)eventArgs.Parameter == "Cancel")
            {
                return;
            }
        }
    }
}
