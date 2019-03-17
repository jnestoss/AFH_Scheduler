using AFH_Scheduler.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.HelperClasses
{
    public class GenerateNewIDs
    {

        public static long GenerateProviderID()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                long newProvID;
                try
                {
                    var recentHomeID = db.Providers.OrderByDescending(r => r.Provider_ID).FirstOrDefault();
                    if (recentHomeID.Provider_ID == Int64.MaxValue)
                    {
                        newProvID = 0;
                    }
                    else
                        newProvID = recentHomeID.Provider_ID + 1;
                }
                catch (Exception e)
                {
                    newProvID = 0;
                }

                var isUniqueID = db.Providers.Where(r => r.Provider_ID == newProvID).ToList();
                if (isUniqueID.Count != 0)
                {
                    do
                    {
                        newProvID++;
                        isUniqueID = db.Providers.Where(r => r.Provider_ID == newProvID).ToList();
                    } while (isUniqueID.Count != 0);
                }
                return newProvID;
            }
        }

        public static long GenerateHomeID()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                long newHomeID;
                try
                {
                    var recentHomeID = db.Provider_Homes.OrderByDescending(r => r.PHome_ID).FirstOrDefault();
                    if (recentHomeID.PHome_ID == Int64.MaxValue)
                    {
                        newHomeID = 0;
                    }
                    else
                        newHomeID = recentHomeID.PHome_ID + 1;
                }
                catch (Exception e)
                {
                    newHomeID = 0;
                }

                var isUniqueID = db.Provider_Homes.Where(r => r.PHome_ID == newHomeID).ToList();
                if (isUniqueID.Count != 0)
                {
                    do
                    {
                        newHomeID++;
                        isUniqueID = db.Provider_Homes.Where(r => r.PHome_ID == newHomeID).ToList();
                    } while (isUniqueID.Count != 0);
                }
                return newHomeID;
            }
        }

        public static long GenerateHistoryID()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                long newHistoyID;
                try
                {
                    var recentHomeID = db.Home_History.OrderByDescending(r => r.HHistory_ID).FirstOrDefault();
                    if (recentHomeID.HHistory_ID == Int64.MaxValue)
                    {
                        newHistoyID = 0;
                    }
                    else
                        newHistoyID = recentHomeID.HHistory_ID + 1;
                }
                catch (Exception e)
                {
                    newHistoyID = 0;
                }

                var isUniqueID = db.Home_History.Where(r => r.HHistory_ID == newHistoyID).ToList();
                if (isUniqueID.Count != 0)
                {
                    do
                    {
                        newHistoyID++;
                        isUniqueID = db.Home_History.Where(r => r.HHistory_ID == newHistoyID).ToList();
                    } while (isUniqueID.Count != 0);
                }
                return newHistoyID;
            }
        }

        public static long GenerateScheduleID()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                long newschedID;
                try
                {
                    var recentHomeID = db.Scheduled_Inspections.OrderByDescending(r => r.SInspections_Id).FirstOrDefault();
                    if (recentHomeID.SInspections_Id == Int64.MaxValue)
                    {
                        newschedID = 0;
                    }
                    else
                        newschedID = recentHomeID.SInspections_Id + 1;
                }
                catch (Exception e)
                {
                    newschedID = 0;
                }

                var isUniqueID = db.Scheduled_Inspections.Where(r => r.SInspections_Id == newschedID).ToList();
                if (isUniqueID.Count != 0)
                {
                    do
                    {
                        newschedID++;
                        isUniqueID = db.Scheduled_Inspections.Where(r => r.SInspections_Id == newschedID).ToList();
                    } while (isUniqueID.Count != 0);
                }
                return newschedID;
            }
        }

    }
}
