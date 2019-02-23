using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AFH_Scheduler.Data;
using AFH_Scheduler.Helper_Classes;
using Microsoft.Office.Interop.Excel;

namespace AFH_Scheduler.Dialogs
{
    public class ImportDataPreviewVM : ObservableObject, IPageViewModel
    {
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
                        
                        if (colCell[1,1].IndexOf("LicenseNumber", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in colCell)
                            {
                                ImportedLicenseInfo[0].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("FacilityName", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                ImportedLicenseInfo[1].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("LocationAddress", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                ImportedLicenseInfo[2].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("LocationCity", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                ImportedLicenseInfo[3].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("LocationZipCode", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                ImportedLicenseInfo[4].Add(cel.ToString());
                            }
                        }
                        else if (colCell[1, 1].IndexOf("TelephoneNmbr", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            ImportedLicenseInfo.Add(new List<string>());
                            foreach (var cel in col.Cells.Value)
                            {
                                if(cel == null)
                                    ImportedLicenseInfo[5].Add("");
                                else
                                    ImportedLicenseInfo[5].Add(cel.ToString());
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
            /*var temp = ImportedLicenseInfo[0][0]; //LicenseNum
            var temp2 = ImportedLicenseInfo[1][0]; //Home Name
            var temp3 = ImportedLicenseInfo[2][0]; //Address
            var temp4 = ImportedLicenseInfo[3][0]; //City
            var temp5 = ImportedLicenseInfo[4][0]; //Zipcode
            var temp6 = ImportedLicenseInfo[5][0]; //Telephone*/

            int rows = ImportedLicenseInfo[0].Count;
            for (int rowItem = 1; rowItem < rows; rowItem++)
            {
                ImportedHomes.Add(
                                new ScheduleModel
                                (
                                    0,   //Provider ID
                                    Convert.ToInt64(ImportedLicenseInfo[0][rowItem]),     //Home License
                                    "",                                  //Provider Name
                                    ImportedLicenseInfo[1][rowItem],     //Home Name
                                    ImportedLicenseInfo[5][rowItem],     //Phone Number
                                    ImportedLicenseInfo[2][rowItem],     //Address
                                    ImportedLicenseInfo[3][rowItem],     //City
                                    ImportedLicenseInfo[4][rowItem],     //Zip
                                    "",         //Recent
                                    "",               //Next Inspection
                                    null,               //DataVM
                                    ""//18th Month Drop Date
                                )
                            );
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
