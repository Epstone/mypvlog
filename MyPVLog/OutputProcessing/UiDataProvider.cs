﻿namespace PVLog.OutputProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataLayer;
    using Enums;
    using Models;
    using Utility;

    public class UiDataProvider
    {
        internal string GoogleDataTableContent(DateTime startDate, DateTime endDate, int plantId,
            E_EurKwh yMode, E_TimeMode xMode)
        {
            //Get kwhdays from database
            using (var _kwhRepository = new KwhRepository())
            {
                var dailyResult = _kwhRepository.GetDayKwhByDateRange(startDate, endDate, plantId);

                //Fill up the month for the chart data
                var kwhTable = KwhCalculator.SummarizeKwh(dailyResult, startDate, endDate,
                    xMode, true);

                // calculate roi if neccesary and convert to Google Data Table
                return GenerateGoogleDataTableContent(plantId, yMode, kwhTable, xMode);
            }
        }

        private string GenerateGoogleDataTableContent(int plantId, E_EurKwh yMode,
            SortedKwhTable kwhTable, E_TimeMode xMode)
        {
            DatatableConverter dtConverter = new DatatableConverter();

            //include money per kwh mapping if neccessary
            if (yMode == E_EurKwh.money)
            {
                IncludeEuroPerKwhMapping(plantId, dtConverter);
            }

            //create google data table content string
            return dtConverter.BuildGoogleDataTable(kwhTable, yMode, xMode);
        }

        private void IncludeEuroPerKwhMapping(int plantId, DatatableConverter dtConverter)
        {
            using (var _plantRepository = new PlantRepository())
            {
                var inverters = _plantRepository.GetAllInvertersByPlant(plantId);

                foreach (var inverter in inverters)
                {
                    dtConverter.AddEuroPerKwH(inverter.PublicInverterId, inverter.EuroPerKwh);
                }
            }
        }

        internal KwhEurResult GetkwhAndMoneyPerTimeFrame(DateTime startDate, DateTime endDate, int plantId, E_TimeMode xMode)
        {
            using (var _plantRepository = new PlantRepository())
            using (var _kwhRepository = new KwhRepository())
            {
                //Get kwhdays from database
                var dailyResult = _kwhRepository.GetDayKwhByDateRange(startDate, endDate, plantId);

                //Fill up the month for the chart data
                var kwhTable = KwhCalculator.SummarizeKwh(dailyResult, startDate, endDate,
                    xMode, true);

                KwhEurResult result = new KwhEurResult
                {
                    Kwh = GetCumulatedKwh(kwhTable),
                    Euro = GetCumulatedEuro(kwhTable, _plantRepository.GetAllInvertersByPlant(plantId))
                };

                return result;
            }
        }

        private double? GetCumulatedEuro(SortedKwhTable kwhTable, IEnumerable<Inverter> inverters)
        {
            double result = 0;
            foreach (var row in kwhTable.Rows.Values)
            {
                foreach (var kwh in row.kwhValues)
                {
                    result += kwh.Value * inverters.Single(x => x.InverterId == kwh.PrivateInverterId).EuroPerKwh;
                }
            }

            return result;
        }

        private double GetCumulatedKwh(SortedKwhTable kwhTable)
        {
            double result = 0;
            foreach (var row in kwhTable.Rows.Values)
            {
                foreach (var kwh in row.kwhValues)
                {
                    result += kwh.Value;
                }
            }

            return result;
        }

        internal SortedKwhTable GetMonthTable(int plantId)
        {
            using (var _kwhRepository = new KwhRepository())
            {
                var startDate = _kwhRepository.GetFirstDateOfKwhDay(plantId);
                startDate = DateTimeUtils.FirstDayOfMonth(startDate.Month, startDate.Year);

                var endDate = DateTimeUtils.FirstDayNextMonth();

                //Get kwhdays from database
                var dailyResult = _kwhRepository.GetDayKwhByDateRange(startDate, endDate, plantId);

                //Fill up the month for the chart data
                return KwhCalculator.SummarizeKwh(dailyResult, startDate, endDate, E_TimeMode.month, false);
            }
        }
    }
}