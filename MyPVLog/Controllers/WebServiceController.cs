using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVLog.DataLayer;
using System.Data;
using PVLog.OutputProcessing;
using PVLog.Utility;
using PVLog.Enums;
using System.Threading;
using System.Globalization;
using PVLog.Models;

namespace PVLog.Controllers
{
    public class WebServiceController : MyController
    {
        private readonly IInverterTrackerRegistry _inverterTrackerRegistry;

        public WebServiceController(IInverterTrackerRegistry inverterTrackerRegistry)
        {
            _inverterTrackerRegistry = inverterTrackerRegistry;
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        }


        //
        // GET: /WebService/

        /// <summary>
        /// Returns the the current wattage for the inverterId
        /// </summary>
        /// <param name="inverterId"></param>
        /// <returns></returns>
        public JsonResult GaugeData(int plantId)
        {
            var allInvertersByPlant = _plantRepository.GetAllInvertersByPlant(plantId);
            var invertersByPlant = allInvertersByPlant.ToArray();
            var inverterTrackers = invertersByPlant.Select(x => _inverterTrackerRegistry.CreateOrGetTracker(x.InverterId));

            var lastestMeasures = inverterTrackers.Select(x=> x.GetLastestMeasure()).ToArray();

            IEnumerable<object> result = null;
            try
            {
                if (lastestMeasures.Length > 0)
                {
                    result = from measure in lastestMeasures
                             select new
                             {
                                 inverterId = measure.PublicInverterId,
                                 wattage = measure.OutputWattage,
                                 temperature = measure.Temperature,
                                 maxWattage = 15000,
                                 time = measure.DateTime.ToLongTimeString()
                             };
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult MinuteWiseWattageDay(int plantId, long timeStamp, E_InverterMode mode)
        {
            DateTime date = DateTimeUtils.JavascriptUtcTimestampToLocalTime(timeStamp);
            object result;

            if (mode == E_InverterMode.Cumulated)
            {
                var table = _measureRepository.GetCumulatedMinuteWiseWattageChartData(plantId, date);
                table.label = "Total";
                result = table;
            }
            else
                result = _measureRepository.GetInverterWiseMinuteWiseWattageChartData(plantId, date);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MonthData(int plantId, int month, int year, E_EurKwh yMode)
        {
            var startDate = DateTimeUtils.FirstDayOfMonth(month, year);
            var endDate = startDate.AddMonths(1);
            string googleTableContent = _dataProvider.GoogleDataTableContent(startDate, endDate,
                                                                  plantId, yMode, E_TimeMode.day);

            return Json(new
            {
                tableContent = googleTableContent,
                monthName = DateTimeUtils.GetMonthName(month),
                year = year
            }
            , JsonRequestBehavior.AllowGet);
        }

        public JsonResult YearData(int plantId, int year, E_EurKwh yMode)
        {
            string googleTableContent = _dataProvider.GoogleDataTableContent(DateTimeUtils.FirstDayOfYear(year),
                                                                    DateTimeUtils.FirstDayOfYear(year + 1),
                                                                    plantId, yMode, E_TimeMode.month);
            return Json(new
            {
                tableContent = googleTableContent
            }
            , JsonRequestBehavior.AllowGet);
        }

        public JsonResult DecadeData(int plantId, E_EurKwh yMode)
        {
            string googleTableContent = _dataProvider.GoogleDataTableContent(new DateTime(2010, 1, 1),
                                                                    new DateTime(2020, 1, 1),
                                                                    plantId, yMode, E_TimeMode.year);
            return Json(new
            {
                tableContent = googleTableContent
            }
            , JsonRequestBehavior.AllowGet);
        }







    }
}
