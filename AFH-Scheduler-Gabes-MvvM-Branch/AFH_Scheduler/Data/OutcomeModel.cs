using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFH_Scheduler.Data
{
    class OutcomeModel
    {
        private string outcome_code { get; set; }
        private string outcome_mintime { get; set; }
        private string outcome_maxtime { get; set; }

        public OutcomeModel(string outcome_code, string outcome_mintime, string outcome_maxtime)
        {
            this.outcome_code = outcome_code;
            this.outcome_mintime = outcome_mintime;
            this.outcome_maxtime = outcome_maxtime;
        }
    }
}
