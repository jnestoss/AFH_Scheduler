using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;

namespace AFH_Scheduler.Dialogs
{
    public class HistVM : ObservableObject, IPageViewModel
    {

        private string _title;
        public string Title {
            get => _title;
            set {
                if(value != _title)
                {
                    _title = value;
                    OnPropertyChanged("Title");
                }
            }
        }

        #region variables
        private static ObservableCollection<HistoryDetailModel> _homeHistory;
        public ObservableCollection<HistoryDetailModel> HomeHistory {
            get { return _homeHistory; }
            set {
                if (value != _homeHistory)
                {
                    _homeHistory = value;
                    OnPropertyChanged("HomeHistory");
                }
            }
        }
        #endregion


        public string Name => "History Page";

        public HistVM(HomeModel home)
        {
            Title = $"History for {home.HomeName}";
            HomeHistory = new ObservableCollection<HistoryDetailModel>();
            GenHistoryData(home);
        }

        public void GenHistoryData(HomeModel house)
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                string homeName;
                var provs = db.Home_History.Where(x => x.FK_PHome_ID == house.HomeID).ToList();
                foreach (var item in provs)
                {
                    homeName = db.Provider_Homes.First(r => r.PHome_ID == item.FK_PHome_ID.Value).PHome_Name;//providerID;

                    HomeHistory.Add(
                        new HistoryDetailModel
                        (
                            homeName,
                            item.HHistory_Date,//InspectionDate
                            item.Inspection_Outcome.IOutcome_Code
                        )
                    );
                }
            }
        }
    }
}