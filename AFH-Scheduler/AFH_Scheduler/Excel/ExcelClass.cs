using AFH_Scheduler.Database;
using System;
using Microsoft.Office.Interop.Excel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Dialogs;
using System.Collections.ObjectModel;
using AFH_Scheduler.Data;
using AFH_Scheduler.Algorithm;

namespace AFH_Scheduler.Excel
{
    public class ExcelClass
    {
        private static SchedulingAlgorithm alg = new SchedulingAlgorithm();

        private static IOpenMessageDialogService _messageService;
        public static IOpenMessageDialogService MessageService {
            get {
                if (_messageService == null)
                    _messageService = new SchedulesOpenDialog();
                return _messageService;
            }
        }


        #region excelstuff
        public static void ImportExcelTableNew(ObservableCollection<HomeModel> importedHomes)
        {
            foreach (var importedHome in importedHomes)
            {
                   using (HomeInspectionEntities db = new HomeInspectionEntities())
                   {
                        /*var removeHome = db.Provider_Homes.Where(r => r.PHome_LicenseNumber.Equals(importedHome.HomeLicenseNum.ToString())).First();
                        var removeSched = db.Scheduled_Inspections.Where(r => r.FK_PHome_ID == removeHome.PHome_ID).First();
                        var removehistory = db.Home_History.Where(r => r.FK_PHome_ID == removeHome.PHome_ID).First();

                    db.Home_History.Remove(removehistory);
                    db.SaveChanges();

                    db.Scheduled_Inspections.Remove(removeSched);
                    db.SaveChanges();

                    db.Provider_Homes.Remove(removeHome);
                    db.SaveChanges();*/

                    Nullable<long> provID;
                       if (importedHome.ProviderID == -1)
                       {
                           provID = null;
                       }
                       else
                       {
                           provID = importedHome.ProviderID;
                       }
                       var uniqueLicense = db.Provider_Homes.Where(r => r.PHome_LicenseNumber.Equals(importedHome.HomeLicenseNum.ToString())).ToList();
                       if (uniqueLicense.Count == 0)
                       {
                           if (provID != null)
                           {
                               var prov = db.Providers.Where(r => r.Provider_ID == provID).ToList();
                               if (prov.Count == 0) //New Provider
                               {
                                   db.Providers.Add(new Provider
                                   {
                                       Provider_ID = importedHome.ProviderID,
                                       Provider_Name = importedHome.ProviderName
                                   });
                                   db.SaveChanges();
                               }
                           }

                           db.Provider_Homes.Add(new Provider_Homes
                           {
                               PHome_ID = importedHome.HomeID,
                               PHome_Address = importedHome.Address,
                               PHome_City = importedHome.City,
                               PHome_Zipcode = importedHome.ZIP,
                               PHome_Phonenumber = importedHome.Phone,
                               FK_Provider_ID = provID,
                               PHome_Name = importedHome.HomeName,
                               PHome_LicenseNumber = importedHome.HomeLicenseNum.ToString(),
                               PHome_RCSUnit = importedHome.RcsRegionUnit
                           });
                           db.SaveChanges();


                           long newID;
                           try
                           {
                               var recentHomeID = db.Scheduled_Inspections.OrderByDescending(r => r.SInspections_Id).FirstOrDefault();
                               if (recentHomeID.SInspections_Id == Int64.MaxValue)
                               {
                                   newID = 0;
                               }
                               else
                                   newID = recentHomeID.SInspections_Id + 1;
                           }
                           catch (Exception e)
                           {
                               newID = 0;
                           }

                           var isUniqueID = db.Scheduled_Inspections.Where(r => r.SInspections_Id == newID).ToList();
                           if (isUniqueID.Count != 0)
                           {
                               do
                               {
                                   newID++;
                                   isUniqueID = db.Scheduled_Inspections.Where(r => r.SInspections_Id == newID).ToList();
                               } while (isUniqueID.Count != 0);
                           }

                        db.Scheduled_Inspections.Add(new Scheduled_Inspections
                        {
                            SInspections_Id = newID,
                            SInspections_Date = importedHome.NextInspection,
                            FK_PHome_ID = importedHome.HomeID,
                            SInspections_SeventeenMonth = importedHome.SeventeenMonthDate,
                            SInspections_EighteenMonth = importedHome.EighteenthMonthDate,
                            SInspection_ForecastedDate = importedHome.ForecastedDate
                        });
                           db.SaveChanges();

                        long newHistoryID;
                        try
                        {
                            var recentHomeID = db.Home_History.OrderByDescending(r => r.HHistory_ID).FirstOrDefault();
                            if (recentHomeID.HHistory_ID == Int64.MaxValue)
                            {
                                newHistoryID = 0;
                            }
                            else
                                newHistoryID = recentHomeID.HHistory_ID + 1;
                        }
                        catch (Exception e)
                        {
                            newHistoryID = 0;
                        }

                        var isUniqueHistoryID = db.Home_History.Where(r => r.HHistory_ID == newHistoryID).ToList();
                        if (isUniqueHistoryID.Count != 0)
                        {
                            do
                            {
                                newHistoryID++;
                                isUniqueHistoryID = db.Home_History.Where(r => r.HHistory_ID == newHistoryID).ToList();
                            } while (isUniqueHistoryID.Count != 0);
                        }

                        db.Home_History.Add(new Home_History
                        {
                           HHistory_ID = newHistoryID,
                           HHistory_Date = importedHome.RecentInspection,
                           FK_PHome_ID = importedHome.HomeID,
                           FK_Outcome_Code = db.Inspection_Outcome.FirstOrDefault(r => r.IOutcome_Code.Equals("NEW")).IOutcome_Code
                        });
                           db.SaveChanges();
                       }
                   }
            }
        }

        public static void ExportTableNew(ObservableCollection<HomeModel> providers)
        {
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
                        try
                        {
                            xlWorksheet = (Worksheet)xlWorkbook.ActiveSheet;
                            //"Please include:  
                            // Region/unit; Checked
                            // last inspection; Checked
                            // current inspection; Checked
                            // how many months and days those are apart; Checked
                            // code (result) from current inspection (we have shared codes to use based performance); 
                            // forecasted next inspection date; 
                            // 17 month drop dead date; Checked
                            // 18 month drop dead date (for forecasted inspection). Checked"

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
                            xlWorksheet.Cells[1, 12] = "Current Outcome"; //From current inspection
                            xlWorksheet.Cells[1, 13] = "Forecasted Next Inspection";//forecasted next inspection date
                            xlWorksheet.Cells[1, 14] = "RCSRegionUnit";


                            int row = 2;
                            foreach (var provider in providers)
                            {
                                var forecastedOutcome = alg.ForecastingFutureInspection(provider.HomeID);

                                xlWorksheet.Cells[row, 1] = provider.ProviderID;
                                xlWorksheet.Cells[row, 2] = provider.ProviderName;
                                xlWorksheet.Cells[row, 3] = provider.Address;
                                xlWorksheet.Cells[row, 4] = provider.City;
                                xlWorksheet.Cells[row, 5] = provider.ZIP;
                                xlWorksheet.Cells[row, 6] = provider.RecentInspection;
                                xlWorksheet.Cells[row, 7] = provider.NextInspection;
                                xlWorksheet.Cells[row, 8] = alg.InspectionInterval(provider.RecentInspection, provider.NextInspection, true);//Interval in Months
                                xlWorksheet.Cells[row, 9] = alg.InspectionInterval(provider.RecentInspection, provider.NextInspection, false);//Interval in Days
                                xlWorksheet.Cells[row, 10] = provider.SeventeenMonthDate;//17th Month Drop Date
                                xlWorksheet.Cells[row, 11] = provider.EighteenthMonthDate;

                                if (forecastedOutcome == null)
                                {
                                    xlWorksheet.Cells[row, 12] = "";//Outcome from current inspection
                                    xlWorksheet.Cells[row, 13] = "";//forecasted next inspection date
                                }
                                else
                                {
                                    xlWorksheet.Cells[row, 12] = forecastedOutcome.IOutcome_Code;//Outcome from current inspection

                                    var insp = SchedulingAlgorithm.NextScheduledDate(forecastedOutcome, provider.NextInspection);

                                    var inspection = SchedulingAlgorithm.ExtractDateTime(insp);

                                    if (alg.CheckingForUniqueInspection(db, inspection, provider.HomeID))
                                    {
                                        xlWorksheet.Cells[row, 13] = insp;//forecasted next inspection date
                                    }
                                    else
                                    {
                                        bool dateCleared = false;
                                        do
                                        {
                                            inspection.AddDays(1);
                                            SchedulingAlgorithm.CheckDay(inspection);
                                            if (alg.CheckingForUniqueInspection(db, inspection, provider.HomeID))
                                            {
                                                xlWorksheet.Cells[row, 13] = insp;//forecasted next inspection date
                                                dateCleared = true;
                                            }
                                        } while (!dateCleared);
                                    }
                                }

                                xlWorksheet.Cells[row, 14] = provider.RcsRegionUnit;//RCS Region

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
            }
        }
        #endregion

    }
}
