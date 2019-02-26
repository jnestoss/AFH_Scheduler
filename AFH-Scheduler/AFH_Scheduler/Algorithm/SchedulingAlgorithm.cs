using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Database;
//Team 6: Scott Allen, Gabriel Evans, Josh Nestoss, George Polyak 
namespace AFH_Scheduler.Algorithm
{
    public class SchedulingAlgorithm
    {
        #region Grabbing Most Recent Inspection Date
        public Home_History GrabbingRecentInspection(int pHome_ID)
        {
            HomeInspectionEntities table = new HomeInspectionEntities();
            var history = table.Home_History.Where(r => r.FK_PHome_ID == pHome_ID).ToList();

            if(history.Count == 0)
            {
                return null;
            }
            DateTime recentDate = ExtractDateTime(history[0].HHistory_Date);
            DateTime temp;

            Home_History historyReturn = history[0];
            foreach (var date in history)
            {
                temp = ExtractDateTime(date.HHistory_Date);
                if (DateTime.Compare(recentDate, temp) < 0)
                {
                    recentDate = temp;
                    historyReturn = date;
                }//otherwise, recentDate >= temp

            }
            return historyReturn;
        }
        #endregion

        #region Scheduling Next Inspection
        public string SchedulingNextDate(int pHome_ID)//This will probably be a class somewhere in Main
        {
            HomeInspectionEntities table = new HomeInspectionEntities();
            var history = GrabbingRecentInspection(pHome_ID);
            if(history == null)
            {
                return "";
            }

            Inspection_Outcome outcome = table.Inspection_Outcome.First(r => r.IOutcome_Code == history.FK_IOutcome_Code);

            DateTime newInspection = NextScheduledDate(outcome, ExtractDateTime(history.HHistory_Date));

            bool dateCleared = false;
            Random randomiz = new Random();
            int min = 1, max, added_days;
            do
            {
                if (!CheckingForUniqueInspection(table, newInspection, pHome_ID))
                {//If the newly calculated date is shared with another home, it must be adjusted.

                    max = CheckingMonth(newInspection);
                    added_days = randomiz.Next(min, max + 1);
                    newInspection = newInspection.AddDays(1);
                }
                else
                {
                    dateCleared = true;
                }

            } while (!dateCleared);

            /*table.Scheduled_Inspections.Add(new Scheduled_Inspections { SInspections_Id = 456789, SInspections_Date = ConvertDateToString(newInspection), FK_PHome_ID = pHome_ID });
            table.SaveChanges();*/

            return ConvertDateToString(newInspection);
        }
        #endregion

        #region Checking For Unique Inspection
        public bool CheckingForUniqueInspection(HomeInspectionEntities table, DateTime newInspection, int pHome_ID)
        {
            bool isUniqueDate = false;
            string dateComparison = ConvertDateToString(newInspection);

            long providerID = table.Provider_Homes.First(r => r.PHome_ID == pHome_ID).FK_Provider_ID.Value;
            try
            {
                Scheduled_Inspections scheduled = table.Scheduled_Inspections.Where(r => r.SInspections_Date == dateComparison && r.FK_PHome_ID != pHome_ID).First();
                var phID2 = scheduled.FK_PHome_ID;
                var providerCompare = table.Provider_Homes.Where(r => r.PHome_ID == phID2).First().FK_Provider_ID;

                if (providerID == providerCompare)
                {//If both homes share the same day for inspections under the same provider, change the date
                    return false;
                }
            }
            catch (InvalidOperationException e)
            {
                //That means this new inspection date is unique, no other provider is on that day.
                isUniqueDate = true;
            }

            return isUniqueDate;
        }
        #endregion

        #region SCHEDULING ALGORITHM
        public static DateTime NextScheduledDate(Inspection_Outcome outcome, DateTime recent_inspection)
        {
            Random randomiz = new Random();
            string minTime = outcome.IOutcome_Mintime;
            string maxTime = outcome.IOutcome_Maxtime;

            int min = Convert.ToInt32(minTime);
            int max = Convert.ToInt32(maxTime);

            int added_months = randomiz.Next(min, max + 1);

            DateTime new_scheduled_inspection = recent_inspection.AddMonths(added_months);

            while (recent_inspection.Month == new_scheduled_inspection.Month)
            {
                added_months = randomiz.Next(min, max + 1);
                new_scheduled_inspection = recent_inspection.AddMonths(added_months);
            }

            new_scheduled_inspection = CheckDay(new_scheduled_inspection);

            return new_scheduled_inspection;
        }
        #endregion

        #region Check If Weekend
        public static DateTime CheckDay(DateTime new_inspectionDate)
        {
            while (new_inspectionDate.DayOfWeek == DayOfWeek.Saturday || new_inspectionDate.DayOfWeek == DayOfWeek.Sunday)
            {
                new_inspectionDate = new_inspectionDate.AddDays(1);
            }

            //If we wanted to check for holidays, we would need to create a table full of holidays that we want to look for.
            //Otherwise, we might want to leave that to the user manually editing the date if it lands on a holiday for now.
            return new_inspectionDate;
        }
        #endregion

        #region FOLLOW-UPS
        public string SettingFollowUps(string last_correctionDate)
        {
            //int min = 50; int max = 60;
            //for now I set up the follow-up to be as soon as possible and let the user manually edit it

            /* ArrayList followUp_list = new ArrayList();//This is an idea for if the user can select a follow up date in a popUp
             DateTime temp;
             for (int x = 50; x <= 60; x++)
             {
                 temp = last_correctionDate.AddDays(x);
                 followUp_list.Add(temp);
             }

             followUp_list.Clear();*/
            DateTime followupDate = ExtractDateTime(last_correctionDate).AddDays(60);
            string followUp = ConvertDateToString(followupDate);
            return followUp;
        }

        #endregion

        #region 18th Month Drop Date
        public string SettingEighteenthMonth(string scheduled_Date)
        {
            if (scheduled_Date == null || scheduled_Date.Length == 0)
                return "";

            DateTime eighteenthMonthDate = ExtractDateTime(scheduled_Date).AddDays(548);
            return ConvertDateToString(eighteenthMonthDate);
        }

        #endregion

        #region ExtractDateTime(string)
        public DateTime ExtractDateTime(string date)//Date format: mm/dd/yy
        {
            String[] schedule = date.Split('/', ' ');
            int month = Convert.ToInt32(schedule[0]);
            int day = Convert.ToInt32(schedule[1]);
            int year = Convert.ToInt32(schedule[2]);

            return new DateTime(year, month, day);
        }
        #endregion

        #region Convert DateTime to Text
        public string ConvertDateToString(DateTime inspection)
        {
            String date = "";
            date += inspection.Month.ToString();
            date += "/";
            date += inspection.Day.ToString();
            date += "/";
            date += inspection.Year.ToString();

            return date;
        }
        #endregion

        #region Checking Month
        public int CheckingMonth(DateTime date)
        {
            int month = date.Month;
            switch (month)
            {
                case 1://Jan
                    return 31;

                case 2://Feb
                    if (DateTime.IsLeapYear(date.Year))
                    {
                        return 29;
                    }
                    return 28;

                case 3://March
                    return 31;

                case 4://April
                    return 30;

                case 5://May
                    return 31;

                case 6://June
                    return 30;

                case 7://July
                    return 31;

                case 8://Aug
                    return 31;

                case 9://Sep
                    return 30;

                case 10://Oct
                    return 31;

                case 11://Nov
                    return 30;

                case 12://Dec
                    return 31;
            }
            return 0;
        }
        #endregion
    }
}
