using AFH_Scheduler.Complete;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.History
{
    public class HistoryDetailViewVM : ObservableObject, IPageViewModel
    {
        public string Name
        {
            get
            {
                return "HistoryDetail";
            }
        }
        private ObservableCollection<HistoryDetailModel> _homeshistory;
        public ObservableCollection<HistoryDetailModel> HomesHistory
        {
            get { return _homeshistory; }
            set
            {
                if (value != _homeshistory)
                {
                    _homeshistory = value;
                    OnPropertyChanged("Homes");
                }
            }
        }
        public HistoryDetailViewVM(long homeID, OpenMessageDialogService messageService)
        {
            MessageService = messageService;
            HomeId = homeID;
            _homeshistory = new ObservableCollection<HistoryDetailModel>();
            GenData();
        }
        private long _homeid;
        public long HomeId
        {
            get
            {
                return _homeid;
            }
            set
            {
                _homeid = value;
                OnPropertyChanged("HomeId");
            }
        }
        private OpenMessageDialogService _messageService;
        public OpenMessageDialogService MessageService
        {
            get
            {
                if (_messageService == null)
                    _messageService = new openHistoryDetailDialog();
                return _messageService;
            }
            set
            {
                _messageService = value;
                OnPropertyChanged("MessageService");
            }
        }
        public void GenData()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                long providerID;
                var provs = db.Home_History.Where(x => x.FK_PHome_ID == HomeId).ToList();
                foreach (var item in provs)
                {
                    providerID = db.Provider_Homes.First(r => r.PHome_ID == item.FK_PHome_ID.Value).FK_Provider_ID.Value;//providerID;
                    //Console.WriteLine(item. + "*************************************************************************************************");
                    HomesHistory.Add(
                        new HistoryDetailModel
                        (
                            providerID,//providerID
                            item.FK_PHome_ID.Value,//Home_ID
                            item.HHistory_Date,//InspectionDate
                            item.Inspection_Outcome.IOutcome_Code
                        )
                    );
                }
            }
        }
    }
}
