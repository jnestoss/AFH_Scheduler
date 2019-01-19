
using System.Linq;
using System.Text;
using System;
using AFH_Scheduler.Helper_Classes;
using AFH_Scheduler.Database;
using System.Collections.ObjectModel;
using AFH_Scheduler.Data;
using System.Windows;
using AFH_Scheduler.Algorithm;
using System.Windows.Input;

namespace AFH_Scheduler.Schedules
{
    public class SchedulerVM : ObservableObject, IPageViewModel
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();
        //Observable and bound to DataGrid
        private ObservableCollection<ScheduleModel> _providers;
        public ObservableCollection<ScheduleModel> Providers {
            get { return _providers; }
            set {
                if (value != _providers)
                {
                    _providers = value;
                    OnPropertyChanged("Providers");
                }
            }
        }

        private RelayCommand _refreshTable;
        public ICommand RefreshTableCommand
        {
            get
            {
                if (_refreshTable == null)
                    _refreshTable = new RelayCommand(RefreshTable);
                return _refreshTable;
            }
        }

        private void RefreshTable(object obj)
        {
            Providers.Clear();
            GenData();
        }

        public SchedulerVM()
        {
            _providers = new ObservableCollection<ScheduleModel>();
            /*
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                Home_History deletingHistory = db.Home_History.First(r => r.HHistory_Date == "3/2/2016" && r.FK_PHome_ID == 1111);
                db.Home_History.Remove(deletingHistory);
                db.SaveChanges();
                var temp = db.Scheduled_Inspections.ToList();
                foreach (var removeThis in temp)
                {
                    Scheduled_Inspections deletingSchedule = db.Scheduled_Inspections.Where(r => r.FK_PHome_ID == removeThis.FK_PHome_ID && r.SInspections_Date.Equals(removeThis.SInspections_Date)).First();
                    db.Scheduled_Inspections.Remove(deletingSchedule);
                    db.SaveChanges();
                }
                int id = 111;
                var provs = db.Providers.ToList();
                foreach (var item in provs)
                {
                    var homes = db.Provider_Homes.Where(r => r.FK_Provider_ID == item.Provider_ID).ToList();
                    foreach (var house in homes)
                    {
                        db.Scheduled_Inspections.Add(new Scheduled_Inspections { SInspections_Id = id, SInspections_Date = alg.SchedulingNextDate(Convert.ToInt32(house.PHome_ID)), FK_PHome_ID = house.PHome_ID });
                        db.SaveChanges();
                        id++;
                    }
                }
            }*/

            GenData();
        }

        public string Name {
            get {
                return "Schedules";
            }
        }

        public void GenData()
        {
            using(HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();
                foreach (var item in provs)
                {
                    var homes = db.Provider_Homes.Where(r=> r.FK_Provider_ID == item.Provider_ID).ToList();
                    foreach (var house in homes)
                    {
                        var insp = db.Scheduled_Inspections.Where(r => r.FK_PHome_ID == house.PHome_ID).First().SInspections_Date;
                        //Console.WriteLine(item. + "*************************************************************************************************");
                        Providers.Add(
                            new ScheduleModel
                            (
                                item.Provider_ID,
                                item.Provider_Name,
                                house.PHome_Phonenumber, //Phone Number
                                house.PHome_Address,//Address
                                alg.GrabbingRecentInspection(Convert.ToInt32(house.PHome_ID)).HHistory_Date,
                                insp,
                                "Banana"
                            )
                        );
                    }
                }

            }
        }

        public static void RadioSelect()
        {
            Console.WriteLine("Button is selected");
        }

    }
}
