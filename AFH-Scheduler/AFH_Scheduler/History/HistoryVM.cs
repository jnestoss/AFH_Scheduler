using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AFH_Scheduler;
using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;

namespace AFH_Scheduler.History
{
    public class HistoryVM : ObservableObject, IPageViewModel
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();
        //Observable and bound to DataGrid
        private ObservableCollection<HistoryModel> _homes;
        public ObservableCollection<HistoryModel> Homes
        {
            get { return _homes; }
            set
            {
                if (value != _homes)
                {
                    _homes = value;
                    OnPropertyChanged("Homes");
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
            Homes.Clear();
            GenData();
        }

        public HistoryVM()
        {
            _homes = new ObservableCollection<HistoryModel>();

            GenData();
        }


        public string Name {
            get {
                return "History";
            }
        }

        public void GenData()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                long providerID;
                var provs = db.Home_History.ToList();
                foreach (var item in provs)
                {
                    providerID = db.Provider_Homes.First(r => r.PHome_ID == item.FK_PHome_ID.Value).FK_Provider_ID.Value;//providerID;
                    //Console.WriteLine(item. + "*************************************************************************************************");
                    Homes.Add(
                        new HistoryModel
                        (
                            providerID,//providerID
                            item.FK_PHome_ID.Value,//Home_ID
                            item.HHistory_Date//recentDate
                        )
                    );
                }

            }
        }
    }
}
