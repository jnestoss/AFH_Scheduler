using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AFH_Scheduler.Database;

namespace AFH_Scheduler.Algorithm
{
    [TestClass]
    class AlgorithmTests
    {
        [TestMethod]
        public void test1()
        {
            SchedulingAlgorithm test = new SchedulingAlgorithm();
            Inspection_Outcome outcome = null;
            DateTime test1 = test.NextScheduledDate(outcome, new DateTime());
            Assert.IsTrue(test1.Equals(null));
        }
    }
}
