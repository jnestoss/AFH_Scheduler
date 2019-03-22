using AFH_Scheduler.Algorithm;
using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Dialogs.Confirmation;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace AFH_Scheduler.Dialogs
{
    public class EditVM : ObservableObject, IPageViewModel       //, ICloseable
    {
        public string Name => "Edit Page";

        public HomeModel _selectedSchedule;
        public HomeModel SelectedSchedule {
            get { return _selectedSchedule; }
            set {
                if (_selectedSchedule == value)
                {
                    return;
                }

                _selectedSchedule = value;
                OnPropertyChanged("SelectedSchedule");
            }
        }

        private readonly ObservableCollection<String> Providers;
        public ICollectionView ComboBoxProviderItems { get; }

        private string _TextSearch;
        public string TextSearch {
            get {
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
            get {
                return _outcomeCodes;
            }
            set {
                if (!(_outcomeCodes == value))
                {
                    _outcomeCodes = value;
                }
            }
        }

        private Inspection_Outcome _selectedCode;
        public Inspection_Outcome SelectedCode {
            get { return _selectedCode; }
            set {
                if (_selectedCode == value)
                {
                    return;
                }

                _selectedCode = value;
            }
        }

        private double _currentAverage;
        public double CurrentAverage {
            get { return _currentAverage; }
            set {
                if (_currentAverage == value)
                {
                    return;
                }

                _currentAverage = value;
                OnPropertyChanged("CurrentAverage");
            }
        }

        private double _desiredAverage;
        public double DesiredAverage {
            get => _desiredAverage;
            set {
                if (_desiredAverage == value)
                {
                    return;
                }

                _desiredAverage = value;
                OnPropertyChanged("DesiredAverage");
            }
        }

        private DateTime _nextInspection;
        public DateTime NextInspection {
            get {
                return _nextInspection;
            }
            set {
                if (_nextInspection == value)
                {
                    return;
                }

                _nextInspection = value;
                OnPropertyChanged("NextInspection");
            }
        }

        private string _previousInspection;
        public string PreviousInspection {
            get => _previousInspection;
            set {
                if (_previousInspection == value)
                {
                    return;
                }

                _previousInspection = value;
            }
        }
        
        private RelayCommand _calcDate;
        public RelayCommand CalcDate {
            get {
                if (_calcDate == null)
                {
                    _calcDate = new RelayCommand(CalcNextInspectionDate);
                }

                return _calcDate;
            }
        }

        private RelayCommand _deleteProviderCommand;
        public RelayCommand DeleteProviderCommand {
            get {
                if (_deleteProviderCommand == null)
                {
                    _deleteProviderCommand = new RelayCommand(ShowProviderList);
                }

                return _deleteProviderCommand;
            }
        }

        private async void ShowProviderList(object obj)
        {
            var view = new DeleteConfirmationDialog();
            var result = await DialogHost.Show(view, "DeleteConfirmationDialog", ClosingEventHandler);
            if (result.Equals("YES"))
            {
                DialogHost.CloseDialogCommand.Execute("DELETE", null);
            }

        }

        public static long _homeIDSave;
        
        public EditVM(HomeModel scheduleData, double desiredAverage, double currentAverage)
        {
            SelectedSchedule = scheduleData;
            
            Providers = new ObservableCollection<string>(GrabProviderInformation());

            var lv = (ListCollectionView)CollectionViewSource.GetDefaultView(Providers);

            ComboBoxProviderItems = lv;
            lv.CustomSort = Comparer<String>.Create(ProviderSort);

            _homeIDSave = SelectedSchedule.HomeID;
            GrabOutcomeCodes();
            SelectedCode = GetMostRecentOutcome();

            TextSearch = SelectedSchedule.ProviderName;
            NextInspection = SchedulingAlgorithm.ExtractDateTime(SelectedSchedule.NextInspection);

            //saving the previous date
            PreviousInspection = SelectedSchedule.NextInspection;

            DesiredAverage = desiredAverage;
            CurrentAverage = currentAverage;
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
                    catch (Exception)
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
            string date = SchedulingAlgorithm.CalculateNextScheduledDate(SelectedSchedule.HomeID, SelectedCode, DateTime.Now.ToString("MM/dd/yyyy"), CurrentAverage, DesiredAverage);
            NextInspection = SchedulingAlgorithm.ExtractDateTime(date);
        }

        private List<String> GrabProviderInformation()
        {
            List<string> providerNames = new List<string>();
            using (HomeInspectionEntities db = new HomeInspectionEntities())
            {
                var provs = db.Providers.ToList();

                foreach (Provider prov in provs)
                {
                    providerNames.Add(prov.Provider_Name);
                }
            }
            return providerNames;
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
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

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
            if ((String)eventArgs.Parameter == "YES")
            {//The delete will be handeled in the dataVM
            }
        }
    }
}
