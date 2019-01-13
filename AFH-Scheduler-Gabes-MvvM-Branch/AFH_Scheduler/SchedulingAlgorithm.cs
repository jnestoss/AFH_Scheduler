using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFH_Scheduler.Database;
//Team 6: Scott Allen, Gabriel Evans, Josh Nestoss, George Polyak 
namespace AFH_Scheduler
{
    class SchedulingAlgorithm
    {
        #region EXTRACTING RECENT INSPECTION
        private void SchedulingNextDate(int PHome_ID)//This will probably be a class somewhere in Main
        {
            HomeInspectionEntities table = new HomeInspectionEntities();
            Home_History history = table.Home_History.FirstOrDefault(r => r.HHistory_ID == PHome_ID);

            string historyOutcome_Code = history.FK_IOutcome_Code;
            string historyDate = history.HHistory_Date;
            Inspection_Outcome outcome = table.Inspection_Outcome.First(r => r.IOutcome_Code == historyOutcome_Code);

            DateTime recentDate = ExtractDateTime(historyDate);

            DateTime newInspection = NextScheduledDate(outcome, recentDate);

            bool dateCleared = false;
            do
            {
                string dateComparison = ConvertDateToString(newInspection);

                bool isUniqueDate = false;
                try
                {
                    Scheduled_Inspections scheduled = table.Scheduled_Inspections.First(r => r.SInspections_Date == dateComparison && r.SInspections_Id != PHome_ID);
                }
                catch (ArgumentNullException e)
                {
                    //That means this new inspection date is unique, no other provider is on that day.
                    isUniqueDate = true;
                    dateCleared = true;
                }

                if (!isUniqueDate)//If the newly calculated date is shared with another provider, it must be adjusted.
                {
                    //add day(s) or randomize another month.
                    newInspection.AddDays(1);
                }
            } while (!dateCleared);
        }
        #endregion

        #region SCHEDULING ALGORITHM
        public DateTime NextScheduledDate(Inspection_Outcome outcome, DateTime recent_inspection)
        {
            Random randomiz = new Random();
            string minTime = outcome.IOutcome_Mintime;
            string maxTime = outcome.IOutcome_Maxtime;

            int min = Convert.ToInt32(minTime);
            int max = Convert.ToInt32(maxTime);

            //added_months = random integer between min-max;
            int added_months = randomiz.Next(min, max + 1);

            //new_scheduled_inspection = recent_inspection + added_months;
            DateTime new_scheduled_inspection = recent_inspection.AddMonths(added_months);

            //If the new inspection is scheduled the same month as the old inspection
            while (recent_inspection.Month == new_scheduled_inspection.Month)
            {
                //added_months = random integer between min-max;	
                //new_scheduled_inspection = recent_inspection + added_months;

                added_months = randomiz.Next(min, max + 1);
                new_scheduled_inspection = recent_inspection.AddMonths(added_months);
            }

            //edit time of day, week, or month if needed later
            new_scheduled_inspection = CheckDay(new_scheduled_inspection);

            return new_scheduled_inspection;
        }

        public DateTime CheckDay(DateTime new_inspectionDate)
        {
            //check the day of the week
            //new_inspectionDate.day == weekend
            while (new_inspectionDate.DayOfWeek == DayOfWeek.Saturday || new_inspectionDate.DayOfWeek == DayOfWeek.Sunday)
            {
                new_inspectionDate.AddDays(1);
            }

            //If we wanted to check for holidays, we would need to create a table full of holidays that we want to look for.
            //Otherwise, we might want to leave that to the user manually editing the date if it lands on a holiday for now.
            return new_inspectionDate;
        }
        #endregion

        #region FOLLOW-UPS
        public DateTime SettingFollowUps(DateTime last_correctionDate)
        {
            //int min = 50; int max = 60;
            //perhaps offer a range of dates from min to max, unless this also has to be randomized
            //like a pop-up window to select which day works perfectly for the follow-up

            ArrayList followUp_list = new ArrayList();
            DateTime temp;
            for (int x = 50; x <= 60; x++)
            {
                //add followUp dates from min to max into the arraylist
                temp = last_correctionDate.AddDays(x);
                followUp_list.Add(temp);
            }

            //create a pop-up windows to select from the list of followUps
            followUp_list.Clear();
            //return default for now.
            return last_correctionDate.AddDays(60); ;
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

    }
}
