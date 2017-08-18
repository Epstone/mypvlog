using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PVLog;
using PVLog.Models;
using PVLog.Utility;
using PVLog.Enums;
using Bortosky.Google.Visualization;
//using Bortosky.Google.Visualization;

namespace solar_tests
{
    [TestFixture]
    public class CalculatorTest
    {
      private static List<IMeasure> GetTestDay(DateTime date)
      {
        //create measures from 12:30 - 15:39
        var averageListHour12 = TestdataGenerator.GetAverageMeasures(20, date, 1000); //check a data hole
        var averageListHour13 = TestdataGenerator.GetAverageMeasures(60, date.AddMinutes(30), 2000);
        var averageListHour14 = TestdataGenerator.GetAverageMeasures(60, date.AddMinutes(90), 1500);
        var averageListHour15 = TestdataGenerator.GetAverageMeasures(15, date.AddMinutes(165), 1000); //check a data hole

        //combine lists
        List<List<Measure>> measurelists = new List<List<Measure>>();
        measurelists.Add(averageListHour12);
        measurelists.Add(averageListHour13);
        measurelists.Add(averageListHour14);
        measurelists.Add(averageListHour15);
        List<IMeasure> testDay = TestdataGenerator.CombineMeasureLists(measurelists);
        return testDay;
      }

        /// <summary>
        /// Uses a test day with measures to verify that we get the correct kwh sums by each hour
        /// </summary>
        [Test]
        public void GetKwhOfDayTest()
        {
            DateTime date = TestdataGenerator.GetTestDate().AddMinutes(30);

            List<IMeasure> testDay = GetTestDay(date);

            //calculate day result
            SortedList<DateTime, MeasureKwH> KwhDay = KwhCalculator.GetKwhHourlyForOneDay(testDay);

            //Check result
            var result12 = KwhDay[TestdataGenerator.GetTestDate()];
            var result13 = KwhDay[TestdataGenerator.GetTestDate().AddHours(1)];
            var result14 = KwhDay[TestdataGenerator.GetTestDate().AddHours(2)];
            var result15 = KwhDay[TestdataGenerator.GetTestDate().AddHours(3)];

            Assert.AreEqual(0.5, result12.Value);
            Assert.AreEqual(2, result13.Value);
            Assert.AreEqual(1.5, result14.Value);
            Assert.AreEqual(0.500, result15.Value);

        }

      

        /// <summary>
        /// Just a test to verify that the Data Table to Google Data Table library works
        /// </summary>
        [Test]
        public void GoogleDataTableJSonTest()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("STYLE", typeof(System.String)).Caption = "Programming Style";
            dt.Columns.Add("FUN", typeof(System.Int32)).Caption = "Fun";
            dt.Columns.Add("WORK", typeof(System.Int32)).Caption = "Work";
            dt.Rows.Add(new object[] { "Hand Coding", 30, 200 });
            dt.Rows.Add(new object[] { "Using the .NET Library", 300, 10 });
            dt.Rows.Add(new object[] { "Skipping Visualization", -50, 0 });

            GoogleDataTable gdt = new GoogleDataTable(dt);
            Console.Write(gdt.GetJson());

        }


        [Test]
        public void SummarizeKwhToDaysTest()
        {
            //Create some measures for 10 days starting at 8am to 18 pm
            SortedKwhTable hourSums = new SortedKwhTable();
            int inverterID = 1;
            var startDate = new DateTime(2010, 1, 1);
            var endDate = new DateTime(2010, 2, 1).AddDays(-1);
            var countDate = new DateTime(2010, 1, 10, 8, 0, 0);

            //10 days from 10th to 20th
            for (int i = 0; i < 10; i++)
            {
                //8 - 17 o'clock
                while (countDate.Hour < 18)
                {
                    MeasureKwH measure = new MeasureKwH();
                    measure.DateTime = countDate;
                    measure.PrivateInverterId = inverterID;
                    measure.TimeMode = PVLog.Enums.E_TimeMode.hour;
                    measure.Value = 1; // 1kwh
                    //add to list
                    hourSums.AddMeasure(measure);
                    countDate = countDate.AddHours(1);
                }
                countDate = countDate.AddDays(1).AddHours(-10);
            }

            //we should get 10  kw for each day
            SortedKwhTable actual = KwhCalculator.SummarizeKwh(hourSums, startDate, endDate, E_TimeMode.day,  true);

            for (int i = 1; i < 10; i++)
            {
                var currentDaySum = actual.ToList()[i];

                // day 1 - 9  = 0 kwh
                if (i < 9)
                {
                    Assert.AreEqual(0, currentDaySum.Value);
                }
                // day 10 - 19 10 kwh per day
                else if (i < 19)
                {
                    Assert.AreEqual(10, currentDaySum.Value);
                }
                // day 20 - 31  = 0 kwh
                else if (i <= 31)
                {
                    Assert.AreEqual(0, currentDaySum.Value);
                }

                Assert.AreEqual(inverterID, currentDaySum.PrivateInverterId);
                
            }

            //null testing
            hourSums = new SortedKwhTable();
            actual = KwhCalculator.SummarizeKwh(hourSums, startDate, endDate, E_TimeMode.month,  true);

            foreach (var dayItem in actual.ToList())
            {
                Assert.AreEqual(0, dayItem.Value);
            }

        }

        [Test]
        public void SummarizeKwhToDaysOfMonthTest()
        {
            DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime endDate = startDate.AddMonths(1);

            MeasureKwH measure_day_1 = TestdataGenerator.GetKwhDay(1.0);
            MeasureKwH measure_day_2 = TestdataGenerator.GetKwhDay(3.0);
            measure_day_1.DateTime = startDate.AddDays(1);
            measure_day_2.DateTime = measure_day_1.DateTime.AddDays(1);

            SortedKwhTable measures = new SortedKwhTable();
            measures.AddMeasure(measure_day_1);
            measures.AddMeasure(measure_day_2);

            var actual = KwhCalculator.SummarizeKwh(measures, startDate, endDate, E_TimeMode.day, true);
            var expectedCount = DateTime.DaysInMonth(startDate.Year, startDate.Month);

            Assert.AreEqual(expectedCount, actual.Count);
            var actualList = actual.ToList();
            for (int i = 0; i < actualList.Count; i++)
            {
                if (i == 0) Assert.AreEqual(0, actualList[i].Value);
                else if (i == 1) Assert.AreEqual(1.0, actualList[i].Value);
                else if (i == 2) Assert.AreEqual(3.0, actualList[i].Value);
                else Assert.AreEqual(0.0, actual.ToList()[i].Value);

            }


        }

     
    }
}
