using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using solar_tests.DatabaseTest;

namespace solar_tests
{
    [SetUpFixture]
    class Setup
    {
        [TearDown]
        public void TearDown()
        {
            var testDB = new TestDbSetup();
            //Initialization.InstallFullPVDataDatabase();
        }
    }
}
