namespace PVLog.Management
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataLayer;
    using Enums;
    using Models;
    using Utility;

    public class MeasureManagement
    {

        /// <summary>
        ///     Recalculates the kwh values for the specified power plant in a given time range
        /// </summary>
        /// <param name="startDate">Defines the start of the time range.</param>
        /// <param name="endDate">Defines the end of the time range.</param>
        /// <param name="plantId">The plant which data should be recalculated</param>
        public void ReCalculateKwh_by_dayToDB(DateTime startDate, DateTime endDate, int plantId)
        {
            using (KwhRepository _kwhDb = new KwhRepository())
            using (PlantRepository _plantRepo = new PlantRepository())
            {

                startDate = DateTimeUtils.CropHourMinuteSecond(startDate);
                endDate = DateTimeUtils.CropHourMinuteSecond(endDate);

                List<int> inverterIDs = _plantRepo.GetPrivateInverterIdsByPlant(plantId);
                var hourly = GetkwhHourlyByTimeFrame(startDate, endDate, inverterIDs);

                SortedKwhTable result = KwhCalculator.SummarizeKwh(hourly, startDate, endDate, E_TimeMode.day, false);

                foreach (var measureKwh in result.ToList())
                {
                    var measureToAdd = measureKwh as MeasureKwH;
                    _kwhDb.InsertDayKwh(measureToAdd);
                }
            }
        }

        /// <summary>
        ///     Builds a kwh table which contains hourly kwh values for each inverter id
        /// </summary>
        /// <param name="startDate">Defines the beginning of the time range for the result.</param>
        /// <param name="endDate">Defines the end of the time range for the result</param>
        /// <param name="inverterIDs">The inverter ID's which should be contained in the result kwh table</param>
        /// <returns>A hourly kwh table with the provided time range and inverter ID's</returns>
        private SortedKwhTable GetkwhHourlyByTimeFrame(DateTime startDate, DateTime endDate, List<int> inverterIDs)
        {
            SortedKwhTable result = new SortedKwhTable();

            using (var _measureRepository = new MeasureRepository())
            {

                foreach (var inverterID in inverterIDs)
                {
                    var measures = _measureRepository.GetMinuteWiseMeasures(startDate, endDate, inverterID).Cast<IMeasure>().ToList();
                    SortedList<DateTime, MeasureKwH> kwhByHours =
                        KwhCalculator.GetKwhHourly(measures, MySettings.DiagDay_StartHour, MySettings.DiagDay_EndHour);

                    foreach (var kwhByHour in kwhByHours.Values)
                    {
                        result.AddMeasure(kwhByHour);
                    }
                }
            }

            return result;
        }
    }
}