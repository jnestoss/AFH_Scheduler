﻿using AFH_Scheduler.Data;
using AFH_Scheduler.Database;
using AFH_Scheduler.Helper_Classes;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using AFH_Scheduler.Algorithm;

namespace AFH_Scheduler.Dialogs
{
    public class NewHomeDialogVM : ObservableObject, IPageViewModel
    {
        private readonly ObservableCollection<String> Providers;
        public ICollectionView ComboBoxProviderItems { get; }

        private string _TextSearch;
        public string TextSearch
        {
            get
            {
                return _TextSearch;
            }
            set
            {
                if (_TextSearch != value)
                {
                    _TextSearch = value;
                    ComboBoxProviderItems.Refresh();
                    OnPropertyChanged("TextSearch");
                }
            }
        }

        private HomeModel _newHomeCreated;
        public HomeModel NewHomeCreated
        {
            get { return _newHomeCreated; }
            set
            {
                _newHomeCreated = value;
                OnPropertyChanged("NewHomeCreated");
            }
        }

        private DateTime _datePicked;
        public DateTime DatePicked
        {
            get { return _datePicked; }
            set
            {
                _datePicked = value;
                NewHomeCreated.NextInspection = _datePicked.ToShortDateString();
                OnPropertyChanged("DatePicked");
            }
        }

        private List<String> _outcomeCodes;
        public List<String> OutcomeCodes {
            get => _outcomeCodes;
            set {
                if (!(_outcomeCodes == value)) _outcomeCodes = value;
            }
        }

        private bool _isProviderSelected;
        public bool IsProviderSelected
        {
            get { return _isProviderSelected; }
            set
            {
                _isProviderSelected = value;
                OnPropertyChanged("IsProviderSelected");
            }
        }

        private ProvidersModel _selectedProviderName;
        public ProvidersModel SelectedProviderName
        {
            get { return _selectedProviderName; }
            set
            {
                if (_selectedProviderName == value) return;
                _selectedProviderName = value;
                if (_selectedProviderName == null || _selectedProviderName.ProviderName.Equals(""))
                {
                    IsProviderSelected = false;
                }
                else
                {
                    IsProviderSelected = true;
                }
                OnPropertyChanged("SelectedProviderName");
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

        private RelayCommand _calcDate;
        public RelayCommand CalcDate {
            get {
                if (_calcDate == null) _calcDate = new RelayCommand(CalcNextInspectionDate);
                return _calcDate;
            }
        }

        public NewHomeDialogVM()
        {
            IsProviderSelected = false;
            Providers = new ObservableCollection<string>(GrabProviderInformation());

            var lv = (ListCollectionView)CollectionViewSource.GetDefaultView(Providers);

            ComboBoxProviderItems = lv;
            lv.CustomSort = Comparer<string>.Create(ProviderSort);

            NewHomeCreated = new HomeModel();
            DatePicked = DateTime.Today;
            GrabOutcomeCodes();
            SelectedCode = GrabStartingOutcomeCode();
            
        }

        private int ProviderSort(string x, string y)
        {
            return GetDistance(x).CompareTo(GetDistance(y));
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

        private Inspection_Outcome GrabStartingOutcomeCode()
        {
            using(HomeInspectionEntities db = new HomeInspectionEntities())
            {
                return db.Inspection_Outcome.First();
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
            NewHomeCreated.NextInspection = date;
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

        public string Name
        {
            get
            {
                return "Create New Home";
            }
        }
    }
}
