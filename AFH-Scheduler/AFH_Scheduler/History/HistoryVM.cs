using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AFH_Scheduler;
using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Complete;
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

        public HistoryVM()
        {
            _homes = new ObservableCollection<HistoryModel>();
            _isexpanded = false;
            GenData();
        }
        private RelayCommand _openEditHistoryDialog;
        public ICommand OpenEditHistoryDialogCommand
        {
            get
            {
                if (_openEditHistoryDialog == null)
                    _openEditHistoryDialog = new RelayCommand(EditHistoryDialogOpen);
                return _openEditHistoryDialog;
            }
        }
        private RelayCommand _loadhomehistory;
        public ICommand LoadHomeHistoryCommand
        {
            get
            {
                if (_loadhomehistory == null)
                    _loadhomehistory = new RelayCommand(LoadHomeHistory);
                return _loadhomehistory;
            }
        }
        private OpenMessageDialogService _messageService;
        public OpenMessageDialogService MessageService
        {
            get
            {
                if (_messageService == null)
                    _messageService = new OpenMessageDialogs();
                return _messageService;
            }
        }
        private string _followUpDate;
        public string FollowUpDate
        {
            get { return _followUpDate; }
            set
            {
                _followUpDate = value;
                OnPropertyChanged("FollowUpDate");
            }
        }
        private bool _isexpanded;
        public bool IsExpanded
        {
            get { return _isexpanded; }
            set
            {
                _isexpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }
        private void LoadHomeHistory(object obj)
        {
            IsExpanded = true;
        }
        private void EditHistoryDialogOpen(object obj)
        {
            RescheduleVM rescheduleVM = new RescheduleVM(FollowUpDate, MessageService);
            var updateOrNot = MessageService.ShowDialog(rescheduleVM);
            if (updateOrNot == true)
            {
                FollowUpDate = rescheduleVM.RescheduledFollowUpDate;
            }
        }
        public string Name
        {
            get
            {
                return "History";
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
        public void GenData()
        {
            /**
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
            **/
        }
    }
}
