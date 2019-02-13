using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Dialogs.Confirmation
{
    public class TransferDeleteVM : ObservableObject, IPageViewModel
    {
        private SchedulingAlgorithm alg = new SchedulingAlgorithm();
        private static ObservableCollection<NewHomeModel> _remainingHomes;
        public ObservableCollection<NewHomeModel> RemainingHomes
        {
            get { return _remainingHomes; }
            set
            {
                if (value != _remainingHomes)
                {
                    _remainingHomes = value;
                    OnPropertyChanged("RemainingHomes");
                }
            }
        }
        private List<string> _providers;
        public List<string> ProvidersList
        {
            get { return _providers; }
            set
            {
                _providers = value;
                OnPropertyChanged("ProvidersList");
            }
        }

        private bool _providerIsChosen;
        public bool ProviderIsChosen
        {
            get { return _providerIsChosen; }
            set
            {
                _providerIsChosen = value;
                OnPropertyChanged("ProviderIsChosen");
            }
        }

        private string _chosenProvider;
        public string ChosenProvider
        {
            get { return _chosenProvider; }
            set
            {
                _chosenProvider = value;
                if (_chosenProvider != null)
                    ProviderIsChosen = true;

                OnPropertyChanged("ChosenProvider");
            }
        }

        private string _provName;
        public string ProvName
        {
            get { return _provName; }
            set
            {
                _provName = value;
                OnPropertyChanged("ProvName");
            }
        }

        public TransferDeleteVM(long id, string name)
        {
            _remainingHomes = new ObservableCollection<NewHomeModel>();
            _providers = new List<string>();

            ProvName = id + "-" + name;
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var providerNames = db.Providers.ToList();
                foreach(var provider in providerNames)
                {
                    if(provider.Provider_ID != id)
                    {
                        _providers.Add(provider.Provider_ID + "-" + provider.Provider_Name);
                    }
                }

                var totalHomes = db.Provider_Homes.Where(r => r.FK_Provider_ID == id).ToList();
                foreach (var house in totalHomes)
                {
                    var insp = db.Scheduled_Inspections.Where(r => r.FK_PHome_ID == house.PHome_ID).First().SInspections_Date;

                    RemainingHomes.Add(
                        new NewHomeModel
                        (
                            house.PHome_ID,
                            house.PHome_Address,
                            house.PHome_City,
                            house.PHome_Zipcode,
                            alg.ExtractDateTime(insp)
                        )
                    );
                }
            }
        }

        public string Name
        {
            get
            {
                return "Transfer or Delete Homes?";
            }
        }
    }
}
