
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
using AFH_Scheduler.Dialogs;
using AFH_Scheduler.Dialogs.Errors;
using System.ComponentModel;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using AFH_Scheduler.Dialogs.Confirmation;

namespace AFH_Scheduler.Schedules
{
    public class SchedulerVM : ObservableObject, IPageViewModel
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();

        private ScheduleModel _selectedSchedule;
        private ScheduleModel SelectedSchedule {
            get { return _selectedSchedule; }
            set {
                if (_selectedSchedule == value) return;
                _selectedSchedule = value;
            }
        }

        //Observable and bound to DataGrid
        private static ObservableCollection<ScheduleModel> _providers;
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
            //SelectedSchedule = Providers.First();
            //SelectedSchedule.IsSelected = true;
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

        public void ClearSelected2(ScheduleModel sm)
        {
            if (SelectedSchedule != null)
                SelectedSchedule.IsSelected = false;
            SelectedSchedule = sm;
        }

        //clear all selected items
        public void ClearSelected()
        {
            //for (int i = 0; i < Providers.Count; i++)
            //{
                
            var item = Providers.Where(X => X.IsSelected == true);
            foreach (ScheduleModel p in item)
            {
                p.IsSelected = false;
            }
            //Providers.Wher
            //while (item != null)
            //{
            //    if (item != null) item.IsSelected = false;
            //    item = Providers.Where(X => X.IsSelected == true).FirstOrDefault();
            //}
            //}

            //foreach(ScheduleModel sm in Providers)
            //{
            //    sm.IsSelected = false;
            //}
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
                                house.PHome_ID,
                                item.Provider_Name,
                                house.PHome_Phonenumber, //Phone Number
                                house.PHome_Address,//Address
                                house.PHome_City,
                                house.PHome_Zipcode,
                                alg.GrabbingRecentInspection(Convert.ToInt32(house.PHome_ID)).HHistory_Date,
                                insp,
                                this
                            )
                        );
                    }
                }

            }
        }





        public ICommand RunEditDialogCommand => new RelayCommand(ExecuteEditDialog);

        private async void ExecuteEditDialog(object o)
        {
            if (SelectedSchedule == null)
            {
                var view = new NoHomeSelectedErrorDialog();

                var result = await DialogHost.Show(view, GenericClosingEventHandler);
            }
            else
            {
                var view = new EditDialog();

                view.setDataContext(SelectedSchedule);

                var result = await DialogHost.Show(view, "EditDialog", ClosingEventHandler);

                if(result.Equals("Delete"))
                {
                    var deleteView = new DeleteConfirmationDialog();

                    var deleteResult = await DialogHost.Show(deleteView, GenericClosingEventHandler);

                    if (deleteResult.Equals("Yes"))
                    {
                        using (HomeInspectionEntities db = new HomeInspectionEntities())
                        {
                            //Console.WriteLine("********" + ((EditVM)((EditDialog)eventArgs.Session.Content).DataContext).SelectedSchedule.HomeID);
                            var home = db.Provider_Homes.SingleOrDefault(r => r.PHome_ID == ((EditVM)view.DataContext).SelectedSchedule.HomeID);

                            if (home != null)
                            {
                                db.Provider_Homes.Remove(home);
                                var waitOnSave = db.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }


        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs) 
        {
            if((String)eventArgs.Parameter == "Submit")
            {
                ScheduleModel editedHomeData = ((EditVM)((EditDialog)eventArgs.Session.Content).DataContext).SelectedSchedule;
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    var homeID = editedHomeData.HomeID;
                    var providerID = editedHomeData.ProviderID;
                    var address = editedHomeData.Address;
                    var city = editedHomeData.City;
                    var zip = editedHomeData.ZIP;
                    var nextInspection = editedHomeData.NextInspection;

                }
            }   
            else if ((String)eventArgs.Parameter == "Delete") {

            }
        }

        private void GenericClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("Dialog closed successfully");
        }


    }
}
