﻿using AFH_Scheduler.Helper_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Dialogs.Errors;
using AFH_Scheduler.Dialogs.Confirmation;
using MaterialDesignThemes.Wpf;
using AFH_Scheduler.Algorithm;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows;

namespace AFH_Scheduler.Dialogs
{
    public class EditVM : ObservableObject, IPageViewModel       //, ICloseable
    {
        public string Name => "Edit Page";

        public HomeModel _selectedSchedule;
        public HomeModel SelectedSchedule {
            get { return _selectedSchedule; }
            set {
                if (_selectedSchedule == value) return;
                _selectedSchedule = value;
                OnPropertyChanged("SelectedSchedule");
            }
        }

        private readonly ObservableCollection<String> Providers;
        public ICollectionView ComboBoxProviderItems { get; }

        private string _TextSearch;
        public string TextSearch {
            get
            {
                return _TextSearch;
            }
            set {
                if (_TextSearch != value)
                {
                    _TextSearch = value;
                    ComboBoxProviderItems.Refresh();
                    OnPropertyChanged("TextSearch");
                }
            }
        }

        private List<String> _outcomeCodes;
        public List<String> OutcomeCodes {
            get
            {
                return _outcomeCodes;
            }
            set {
                if (!(_outcomeCodes == value)) _outcomeCodes = value;
            }
        }

        private Inspection_Outcome _selectedCode;
        public Inspection_Outcome SelectedCode {
            get { return _selectedCode; }
            set {
                if (_selectedCode == value) return;
                _selectedCode = value;
            }
        }

        private DateTime _nextInspection;
        public DateTime NextInspection {
            get
            {
                return _nextInspection;
            }
            set {
                if (_nextInspection == value) return;
                _nextInspection = value;
            }
        }

        private string _previousInspection;
        public string PreviousInspection {
            get => _previousInspection;
            set {
                if (_previousInspection == value) return;
                _previousInspection = value;
            }
        }

        //private RelayCommand _saveAndClose;
        //public RelayCommand SaveAndClose {
        //    get {
        //        if (_saveAndClose == null) _saveAndClose = new RelayCommand<Window>(SaveAndCloseCommand);
        //        return _saveAndClose;
        //    }
        //}

        private RelayCommand _calcDate;
        public RelayCommand CalcDate {
            get {
                if (_calcDate == null) _calcDate = new RelayCommand(CalcNextInspectionDate);
                return _calcDate;
            }
        }

        private RelayCommand _deleteProviderCommand;
        public RelayCommand DeleteProviderCommand {
            get {
                if (_deleteProviderCommand == null)
                    _deleteProviderCommand = new RelayCommand(ShowProviderList);
                return _deleteProviderCommand;
            }
        }

        private async void ShowProviderList(object obj)
        {
            var view = new DeleteConfirmationDialog();
            var result = await DialogHost.Show(view, "DeleteConfirmationDialog", ClosingEventHandler);
            //ClosingEventHandler(this, new DialogClosingEventArgs());
            
        }

        public static long _homeIDSave;


        //public event EventHandler<EventArgs> RequestClose;      
        //public RelayCommand CloseCommand { get; private set; }

        public EditVM(HomeModel scheduleData)
        {
            SelectedSchedule = scheduleData;
            //CurrentProvider = new Tuple<int, string>((int) SelectedSchedule.ProviderID, SelectedSchedule.ProviderName);
            //ProviderIDs = new List<Tuple<int,string>>();
            Providers = new ObservableCollection<string>(GrabProviderInformation());

            var lv = (ListCollectionView)CollectionViewSource.GetDefaultView(Providers);

            ComboBoxProviderItems = lv;
            lv.CustomSort = Comparer<String>.Create(ProviderSort);

            _homeIDSave = SelectedSchedule.HomeID;
            GrabOutcomeCodes();
            SelectedCode = GetMostRecentOutcome();

            TextSearch = SelectedSchedule.ProviderName;

            //saving the previous date
            PreviousInspection = SelectedSchedule.NextInspection;
        }

        private int ProviderSort(string x, string y)
        {
            return GetDistance(x).CompareTo(GetDistance(y));
        }

        private Inspection_Outcome GetMostRecentOutcome()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                List<Home_History> history = db.Home_History.ToList();
                //searches history database for the most recent inspection outcome
                Home_History mostRecentInspectionOutcome = history.Where(x => x.FK_PHome_ID == SelectedSchedule.HomeID).FirstOrDefault();

                List<Inspection_Outcome> outcomes = db.Inspection_Outcome.ToList();

                if (mostRecentInspectionOutcome == null)
                {
                    //initiate next inspection to most recent inspection outcome
                    try
                    {
                        return outcomes.Where(x => x.IOutcome_Code == mostRecentInspectionOutcome.Inspection_Outcome.IOutcome_Code).FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        return outcomes[0];
                    }
                }
                else
                {
                    //Grab first outcome from database
                    return outcomes.FirstOrDefault();
                }
            }
        }

        private void GrabOutcomeCodes()
        {
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                List<Inspection_Outcome> outcomes = db.Inspection_Outcome.ToList();
                OutcomeCodes = outcomes.Select(x => x.IOutcome_Code).ToList();
            }
        }

        private void CalcNextInspectionDate(object o)
        {
            string date = SchedulingAlgorithm.NextScheduledDate(SelectedCode, DateTime.Now.ToString("MM/dd/yyyy"));
            SelectedSchedule.NextInspection = date;
        }

        private List<String> GrabProviderInformation()
        {
            List<string> providerNames = new List<string>();
            using(HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();

                foreach(Provider prov in provs)
                {
                    providerNames.Add(prov.Provider_Name);
                }
            }
            return providerNames;
        }

        private void SaveAndCloseCommand(Window window)
        {

        }

        private int GetDistance(string provider)
        {
            if (string.IsNullOrWhiteSpace(TextSearch))
            {
                return 0;
            }

            string[] splitName = provider.Split(' ');

            string first = splitName[0];
            string last = splitName[splitName.Length - 1];

            first = first.Substring(0, Math.Min(first.Length, TextSearch.Length));
            last = last.Substring(0, Math.Min(last.Length, TextSearch.Length));

            return Math.Min(GetDistance(first, TextSearch), GetDistance(last, TextSearch));
        }

        //Taken from: https://github.com/dotnet/command-line-api/blob/master/src/System.CommandLine/Invocation/TypoCorrection.cs
        private static int GetDistance(string first, string second)
        {

            if (first == null)
            {
                throw new ArgumentNullException(nameof(first));
            }
            if (second == null)
            {
                throw new ArgumentNullException(nameof(second));
            }

            int n = first.Length;
            int m = second.Length;
            if (n == 0) return m;
            if (m == 0) return n;

            int curRow = 0, nextRow = 1;
            int[][] rows = { new int[m + 1], new int[m + 1] };

            for (int j = 0; j <= m; ++j)
            {
                rows[curRow][j] = j;
            }

            for (int i = 1; i <= n; ++i)
            {
                rows[nextRow][0] = i;
                for (int j = 1; j <= m; ++j)
                {
                    int dist1 = rows[curRow][j] + 1;
                    int dist2 = rows[nextRow][j - 1] + 1;
                    int dist3 = rows[curRow][j - 1] + (first[i - 1].Equals(second[j - 1]) ? 0 : 1);

                    rows[nextRow][j] = Math.Min(dist1, Math.Min(dist2, dist3));
                }

                // Swap the current and next rows
                if (curRow == 0)
                {
                    curRow = 1;
                    nextRow = 0;
                }
                else
                {
                    curRow = 0;
                    nextRow = 1;
                }
            }

            return rows[curRow][m];
        }



        public void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if((String)eventArgs.Parameter == "YES")
            {
                using (HomeInspectionEntities db = new HomeInspectionEntities())
                {
                    //Console.WriteLine("********" + ((EditVM)((EditDialog)eventArgs.Session.Content).DataContext).SelectedSchedule.HomeID);
                    var home = db.Provider_Homes.SingleOrDefault(r => r.PHome_ID == SelectedSchedule.HomeID);

                    if (home != null)
                    {
                        db.Provider_Homes.Remove(home);
                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
