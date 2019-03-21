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
using AFH_Scheduler.HelperClasses;

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

                            Provider_Homes newHome = new Provider_Homes
                            {
                                PHome_ID = importedHome.HomeID,
                                PHome_Address = importedHome.Address,
                                PHome_City = importedHome.City,
                                PHome_Zipcode = importedHome.ZIP,
                                PHome_Phonenumber = importedHome.Phone,
                                FK_Provider_ID = provID,
                                PHome_Name = importedHome.HomeName,
                                PHome_LicenseNumber = importedHome.HomeLicenseNum.ToString(),
                                PHome_RCSUnit = importedHome.RcsRegionUnit,
                                PHome_Active = 1 //Null = Inactive, Whole Number = Active
                            };

                        Scheduled_Inspections dates = new Scheduled_Inspections
                        {
                            SInspections_Id = GenerateNewIDs.GenerateScheduleID(),
                            SInspections_Date = importedHome.NextInspection,
                            FK_PHome_ID = importedHome.HomeID,
                            SInspections_SeventeenMonth = importedHome.SeventeenMonthDate,
                            SInspections_EighteenMonth = importedHome.EighteenthMonthDate,
                            SInspection_ForecastedDate = importedHome.ForecastedDate
                        };

                        Home_History history = new Home_History
                        {
                            HHistory_ID = GenerateNewIDs.GenerateHistoryID(),
                            HHistory_Date = importedHome.RecentInspection,
                            FK_PHome_ID = importedHome.HomeID,
                            FK_Outcome_Code = db.Inspection_Outcome.FirstOrDefault(r => r.IOutcome_Code.Equals("NEW")).IOutcome_Code
                        };

                        db.Provider_Homes.Add(newHome);
                        db.SaveChanges();

                        db.Scheduled_Inspections.Add(dates);
                        db.SaveChanges();

                        db.Home_History.Add(history);
                        db.SaveChanges();
                    }
                   }
            }
        }

        public static void ExportTableNew(string fileName, ObservableCollection<HomeModel> providers)
        {
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

                            xlWorksheet.Cells[1, 1] = "LicenseNumber";
                            xlWorksheet.Cells[1, 2] = "FacilityName";
                            xlWorksheet.Cells[1, 3] = "FacilityPOC";
                            xlWorksheet.Cells[1, 4] = "LocationAddress";
                            xlWorksheet.Cells[1, 5] = "LocationCity";
                            xlWorksheet.Cells[1, 6] = "LocationZipCode";
                            xlWorksheet.Cells[1, 7] = "Recent Inspection Date";
                            xlWorksheet.Cells[1, 8] = "Next Inspection Date";
                            xlWorksheet.Cells[1, 9] = "Interval in Months";
                            xlWorksheet.Cells[1, 10] = "Interval in Days";
                            xlWorksheet.Cells[1, 11] = "17th Month Drop Dead";
                            xlWorksheet.Cells[1, 12] = "18th Month Drop Dead";
                            xlWorksheet.Cells[1, 13] = "Current Outcome"; //From current inspection
                            xlWorksheet.Cells[1, 14] = "Forecasted Next Inspection";//forecasted next inspection date
                            xlWorksheet.Cells[1, 15] = "RCSRegionUnit";


                            int row = 2;
                            foreach (var provider in providers)
                            {
                                var forecastedOutcome = alg.ForecastingFutureInspection(provider.HomeID);

                                xlWorksheet.Cells[row, 1] = provider.HomeLicenseNum;
                                xlWorksheet.Cells[row, 2] = provider.HomeName;
                                xlWorksheet.Cells[row, 3] = provider.ProviderName;
                                xlWorksheet.Cells[row, 4] = provider.Address;
                                xlWorksheet.Cells[row, 5] = provider.City;
                                xlWorksheet.Cells[row, 6] = provider.ZIP;
                                xlWorksheet.Cells[row, 7] = provider.RecentInspection;
                                xlWorksheet.Cells[row, 8] = provider.NextInspection;
                                xlWorksheet.Cells[row, 9] = alg.InspectionInterval(provider.RecentInspection, provider.NextInspection, true);//Interval in Months
                                xlWorksheet.Cells[row, 10] = alg.InspectionInterval(provider.RecentInspection, provider.NextInspection, false);//Interval in Days
                                xlWorksheet.Cells[row, 11] = provider.SeventeenMonthDate;//17th Month Drop Date
                                xlWorksheet.Cells[row, 12] = provider.EighteenthMonthDate;

                                if (forecastedOutcome == null)
                                {
                                    xlWorksheet.Cells[row, 13] = "";//Outcome from current inspection
                                    xlWorksheet.Cells[row, 14] = "";//forecasted next inspection date
                                }
                                else
                                {
                                    xlWorksheet.Cells[row, 13] = forecastedOutcome.IOutcome_Code;//Outcome from current inspection

                                    var insp = SchedulingAlgorithm.NextScheduledDate(forecastedOutcome, provider.NextInspection);

                                    var inspection = SchedulingAlgorithm.ExtractDateTime(insp);

                                    if (SchedulingAlgorithm.CheckingForUniqueInspection(inspection, provider.HomeID))
                                    {
                                        xlWorksheet.Cells[row, 14] = insp;//forecasted next inspection date
                                    }
                                    else
                                    {
                                        bool dateCleared = false;
                                        do
                                        {
                                            inspection = inspection.AddDays(1);
                                            SchedulingAlgorithm.CheckDay(inspection);
                                            if (SchedulingAlgorithm.CheckingForUniqueInspection(inspection, provider.HomeID))
                                            {
                                                xlWorksheet.Cells[row, 14] = insp;//forecasted next inspection date
                                                dateCleared = true;
                                            }
                                        } while (!dateCleared);
                                    }
                                }

                                xlWorksheet.Cells[row, 15] = provider.RcsRegionUnit;//RCS Region

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
