//using AFH_Scheduler.Database;
//using AFH_Scheduler.Schedules;
//using System;
//using Microsoft.Office.Interop.Excel;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace AFH_Scheduler.Excel
//{
//    class Excel
//    {

//        private IOpenMessageDialogService _messageService;
//        public IOpenMessageDialogService MessageService {
//            get {
//                if (_messageService == null)
//                    _messageService = new SchedulesOpenDialog();
//                return _messageService;
//            }
//        }


//        public void ExportTable(object obj)
//        {

            

//            string fileName = MessageService.ExcelSaveDialog();
//            if (fileName == null)
//            {
//                MessageService.ReleaseMessageBox("Excel File was not saved.");
//                return;
//            }
//            using (HomeInspectionEntities db = new HomeInspectionEntities())
//            {
//                try
//                { //write to an Excel file
//                    Application xlApp;
//                    Workbook xlWorkbook;
//                    Worksheet xlWorksheet;

//                    xlApp = new Application
//                    {
//                        //Visible = true
//                    };

//                    try
//                    {//Excel work here
//                        xlWorkbook = xlApp.Workbooks.Add("");
//                        xlWorksheet = (Worksheet)xlWorkbook.ActiveSheet;

//                        xlWorksheet.Cells[1, 1] = "License Number";
//                        xlWorksheet.Cells[1, 2] = "Provider";
//                        xlWorksheet.Cells[1, 3] = "Address";
//                        xlWorksheet.Cells[1, 4] = "City";
//                        xlWorksheet.Cells[1, 5] = "Zipcode";
//                        xlWorksheet.Cells[1, 6] = "Recent Inspection Date";
//                        xlWorksheet.Cells[1, 7] = "Next Inspection Date";
//                        xlWorksheet.Cells[1, 8] = "18th Month Drop Dead";

//                        int row = 2;
//                        foreach (var provider in Providers)
//                        {
//                            var home = db.Provider_Homes.Where(r => r.PHome_Address == provider.Address).First();

//                            xlWorksheet.Cells[row, 1] = provider.ProviderID;
//                            xlWorksheet.Cells[row, 2] = provider.ProviderName;
//                            xlWorksheet.Cells[row, 3] = provider.Address;
//                            xlWorksheet.Cells[row, 4] = home.PHome_City;
//                            xlWorksheet.Cells[row, 5] = home.PHome_Zipcode;
//                            xlWorksheet.Cells[row, 6] = provider.RecentInspection;
//                            xlWorksheet.Cells[row, 7] = provider.NextInspection;
//                            xlWorksheet.Cells[row, 8] = provider.EighteenthMonthDate;

//                            row++;
//                        }

//                        xlWorksheet.get_Range("A1", "H1").EntireColumn.AutoFit();

//                        //xlApp.Visible = false;
//                        //xlApp.UserControl = false;
//                        xlWorkbook.SaveAs(fileName, FileFormat: XlFileFormat.xlWorkbookDefault);
//                        //xlWorkbook.SaveAs(ExcelFileName + "\\TestSchedule.xlsx", FileFormat: XlFileFormat.xlWorkbookDefault);
//                        //xlWorkbook.SaveAs("C:\\excelLocationTest\\TestSchedule.xlsx", FileFormat: XlFileFormat.xlWorkbookDefault);


//                    }
//                    catch (Exception e)
//                    {
//                        Console.WriteLine("Problem with Excel " + e.ToString());

//                    }
//                    finally
//                    {
//                        xlApp.Workbooks.Close();
//                    }
//                }
//                catch (Exception e)
//                {
//                    Console.WriteLine("Problem with Excel " + e.ToString());

//                }
//            }
//        }
//    }
//}
