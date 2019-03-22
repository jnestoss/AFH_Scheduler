using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using Nager.Date;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AFH_Scheduler.Algorithm
{
    public class SchedulingAlgorithm
    {
        #region SCHEDULING ALGORITHM
        public static string NextScheduledDate(Inspection_Outcome outcome, string recent_inspection)
        {
            DateTime date = ExtractDateTime(recent_inspection);

            Random randomiz = new Random();
            string minTime = outcome.IOutcome_Mintime;
            string maxTime = outcome.IOutcome_Maxtime;

            int min = Convert.ToInt32(minTime);
            int max = Convert.ToInt32(maxTime);

            int added_months = randomiz.Next(min, max + 1);

            DateTime new_scheduled_inspection = date.AddMonths(added_months);

            while (date.Month == new_scheduled_inspection.Month)
            {
                added_months = randomiz.Next(min, max + 1);
                new_scheduled_inspection = date.AddMonths(added_months);
            }

            new_scheduled_inspection = CheckDay(new_scheduled_inspection);

            return new_scheduled_inspection.ToString("MM/dd/yyyy");
        }
        #endregion

        public static double CalculateInspectionAverage()
        {
            List<Provider_Homes> homes;
            double average = 0;

            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                homes = db.Provider_Homes.ToList();

                foreach (var home in homes)
                {
                    Scheduled_Inspections homeInspections = home.Scheduled_Inspections.First();
                    Home_History homeHistory = home.Home_History.First();

                    DateTime lastInspection = ExtractDateTime(homeHistory.HHistory_Date);
                    DateTime nextInspection = ExtractDateTime(homeInspections.SInspections_Date);

                    TimeSpan diff = nextInspection - lastInspection;

                    average += diff.TotalDays / 29.53;
                }
            }

            return average / homes.Count;
        }


        public static string CalculateNextScheduledDate(long homeID, Inspection_Outcome outcome, string recent_inspection, double averageInspectionTime, double desiredAverage)
        {
            List<DateTime> nextInspectionChoices = new List<DateTime>();

            DateTime date = ExtractDateTime(recent_inspection);

            int minMonthRange = Convert.ToInt32(outcome.IOutcome_Mintime);
            int maxMonthRange = Convert.ToInt32(outcome.IOutcome_Maxtime) + 1;

            DateTime minDate = date.AddMonths(minMonthRange);
            DateTime maxDate = date.AddMonths(maxMonthRange).AddDays(-1);

            while (minDate < maxDate)
            {
                if (!DateSystem.IsPublicHoliday(minDate, CountryCode.US) &&
                    !DateSystem.IsWeekend(minDate, CountryCode.US))
                {
                    nextInspectionChoices.Add(minDate);
                }

                minDate = minDate.AddDays(1);
            }

            int[] probs = new int[nextInspectionChoices.Count];


            double desiredAverageUpperLimit = desiredAverage + 0.3;
            double desiredAverageLowerLimit = desiredAverage - 0.3;
            if (averageInspectionTime < desiredAverageLowerLimit ||
                averageInspectionTime > desiredAverageUpperLimit)
            {
                for (int i = 0; i < nextInspectionChoices.Count; i++)
                {
                    probs[i] = i;
                }
            }
            else
            {
                for (int i = 0; i < nextInspectionChoices.Count; i++)
                {
                    probs[i] = 1;
                }
            }

            var loadedDie = new LoadedDie(probs);

            int number = loadedDie.NextValue();

            bool dateCleared = false;
            int min = 1, max, added_days;
            DateTime newDateObject;
            Random randomiz = new Random();
            if (averageInspectionTime > desiredAverageUpperLimit)
            {
                newDateObject = nextInspectionChoices[probs.Length - 1 - number];
                do
                {
                    if (!CheckingForUniqueInspection(newDateObject, homeID))
                    {

                        max = CheckingMonth(newDateObject);
                        added_days = randomiz.Next(min, max + 1);
                        newDateObject = newDateObject.AddDays(1);
                    }
                    else
                    {
                        dateCleared = true;
                    }

                } while (!dateCleared);
                return newDateObject.ToString("MM/dd/yyyy");
            }
            else
            {
                newDateObject = nextInspectionChoices[number - 1];
                do
                {
                    if (!CheckingForUniqueInspection(newDateObject, homeID))
                    {

                        max = CheckingMonth(newDateObject);
                        added_days = randomiz.Next(min, max + 1);
                        newDateObject = newDateObject.AddDays(1);
                    }
                    else
                    {
                        dateCleared = true;
                    }

                } while (!dateCleared);
                return newDateObject.ToString("MM/dd/yyyy");
            }
        }

        #region Grabbing Most Recent Inspection Date
        public Home_History GrabbingRecentInspection(long pHome_ID)
        {
            HomeInspectionEntities table = new HomeInspectionEntities();
            var history = table.Home_History.Where(r => r.FK_PHome_ID == pHome_ID).ToList();

            if (history.Count == 0)
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

        #region Checking For Unique Inspection
        public static bool CheckingForUniqueInspection(DateTime newInspection, long pHome_ID)
        {
            bool isUniqueDate = false;
            string dateComparison = newInspection.ToShortDateString();

            try
            {
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    long providerID = db.Provider_Homes.First(r => r.PHome_ID == pHome_ID).FK_Provider_ID.Value;
                    Scheduled_Inspections scheduled = db.Scheduled_Inspections.Where(r => r.SInspections_Date == dateComparison && r.FK_PHome_ID != pHome_ID).First();
                    var phID2 = scheduled.FK_PHome_ID;
                    var providerCompare = db.Provider_Homes.Where(r => r.PHome_ID == phID2).First().FK_Provider_ID;

                    if (providerID == providerCompare)
                    {//If both homes share the same day for inspections under the same provider, change the date
                        return false;
                    }
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
        public static string SettingFollowUps(string last_correctionDate)
        {
            DateTime followupDate = ExtractDateTime(last_correctionDate).AddDays(30);
            followupDate = CheckDay(followupDate);
            string followUp = followupDate.ToShortDateString();
            return followUp;
        }

        #endregion

        #region Month Drop Date
        public string DropDateMonth(string scheduled_Date, Drop dropPeriod)
        {
            if (scheduled_Date == null || scheduled_Date.Length == 0)
            {
                return "";
            }

            DateTime dropDateMonthDate;
            if (dropPeriod == Drop.SEVENTEEN_MONTH)
            {
                dropDateMonthDate = ExtractDateTime(scheduled_Date).AddDays(517);
            }
            else if (dropPeriod == Drop.EIGHTEEN_MONTH)
            {
                dropDateMonthDate = ExtractDateTime(scheduled_Date).AddDays(548);
            }
            else
            {
                dropDateMonthDate = ExtractDateTime(scheduled_Date).AddDays(548);
            }
            return dropDateMonthDate.ToShortDateString();
        }
        #endregion

        #region Interval between Inspections
        public double InspectionInterval(string recentInspecion, string currentInspection, bool monthOrDays)
        {
            if (recentInspecion == null || recentInspecion.Length == 0 || currentInspection == null || currentInspection.Length == 0)
            {
                return 0;
            }

            //true = months, false = days
            DateTime recent = ExtractDateTime(recentInspecion);
            DateTime current = ExtractDateTime(currentInspection);
            double resultTotal;
            if (monthOrDays)
            {
                resultTotal = (current - recent).TotalDays;
                resultTotal = resultTotal / 365;
                resultTotal = Math.Round(resultTotal * 12, 2);
            }
            else
            {
                resultTotal = (current - recent).TotalDays;
            }
            return resultTotal;
        }

        #endregion

        #region ExtractDateTime(string)
        public static DateTime ExtractDateTime(string date)//Date format: mm/dd/yy
        {
            String[] schedule = date.Split('/', ' ');
            int month = Convert.ToInt32(schedule[0]);
            int day = Convert.ToInt32(schedule[1]);
            int year = Convert.ToInt32(schedule[2]);

            return new DateTime(year, month, day);
        }
        #endregion

        #region Checking Month
        public static int CheckingMonth(DateTime date)
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

        #region Forecasting future inspections
        public Inspection_Outcome ForecastingFutureInspection(long homeID)
        {
            HomeInspectionEntities table = new HomeInspectionEntities();
            var history = table.Home_History.Where(r => r.FK_PHome_ID == homeID).ToList();

            if (history.Count == 0)
            {
                return null;
            }

            string forecastedOutcome = "";
            int mostOutcomes = 0;
            foreach (var outcome in history)
            {
                int outCount = table.Home_History.Where(r => r.FK_PHome_ID == homeID && r.FK_Outcome_Code == outcome.FK_Outcome_Code).Count();
                if (outCount > mostOutcomes)
                {
                    forecastedOutcome = outcome.FK_Outcome_Code;
                }
            }
            var resultedOutcome = table.Inspection_Outcome.Where(r => r.IOutcome_Code == forecastedOutcome).FirstOrDefault();
            return resultedOutcome;
        }
        #endregion
    }


}
