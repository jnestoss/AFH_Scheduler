using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    public class OutcomeModel : INotifyPropertyChanged
    {
        private string outcome_code { get; set; }
        public string Outcome_code
        {
            get { return outcome_code; }
            set
            {
                if (outcome_code == value) return;
                outcome_code = value;
                OnPropertyChanged("Outcome_code");
            }
        }
        private string outcome_mintime { get; set; }
        public string Outcome_mintime
        {
            get { return outcome_mintime; }
            set
            {
                if (outcome_mintime == value) return;
                outcome_mintime = value;
                OnPropertyChanged("Outcome_mintime");
            }
        }
        private string outcome_maxtime { get; set; }
        public string Outcome_maxtime
        {
            get { return outcome_maxtime; }
            set
            {
                if (outcome_maxtime == value) return;
                outcome_maxtime = value;
                OnPropertyChanged("Outcome_maxtime");
            }
        }

        public OutcomeModel(string outcome_code, string outcome_mintime, string outcome_maxtime)
        {
            this.outcome_code = outcome_code;
            this.outcome_mintime = outcome_mintime;
            this.outcome_maxtime = outcome_maxtime;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
