using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using PVLog.Utility;
using PVLog.Models;
using MySqlRepository;
using System.Configuration;
using Dapper;
using MvcMiniProfiler;
using System.Collections;
using PVLog.OutputProcessing;

namespace PVLog.DataLayer
{
    public class MeasureRepository : MySqlRepositoryBase, I_MeasureRepository
    {
        public MeasureRepository()
        {
            var connStr = ConfigurationManager.ConnectionStrings["pv_data"].ConnectionString;
            base.Initialize(connStr, connStr);
        }

        public MeasureRepository(string connectionString)
        {
            base.Initialize(connectionString, connectionString);
        }

        public MeasureRepository(string readConnectionString, string writeConnectionString)
        {
            base.Initialize(readConnectionString, writeConnectionString);
        }

        /* MINUTE WISE MEASURE */
        public long InsertMeasure(Measure measure)
        {
            long measureId = this.StoreWattage(measure.DateTime, measure.OutputWattage, measure.PrivateInverterId);

            if (measure.Temperature.HasValue)
                this.StoreTemperature(measureId, measure.Temperature.Value);

            // store generator data
            if (measure.GeneratorWattage.HasValue
              || measure.GeneratorVoltage.HasValue
              || measure.GeneratorAmperage.HasValue)
                this.StoreGeneratorData(measureId, measure.GeneratorAmperage, measure.GeneratorVoltage, measure.GeneratorWattage);

            //store grid data
            if (measure.GridAmperage.HasValue
              || measure.GridAmperage.HasValue)
                this.StoreGridData(measureId, measure.GridAmperage, measure.GridVoltage);

            return measureId;

        }

        //public Measure GetLatesMeasureByInverter(int inverterId)
        //{
        //    string cmd = @"SELECT * FROM measure WHERE InverterId = @inverterId Order By DateTime DESC LIMIT 1";

        //    return ProfiledReadConnection.Query<Measure>(cmd, new { inverterId }).First();
        //}

        private void StoreGridData(long measureId, double? gridAmperage, double? gridVoltage)
        {
            string cmd = @"INSERT INTO grid (MeasureId, GridAmperage, GridVoltage)  
                                            VALUES (@measureId, @gridAmperage, @gridVoltage);";

            base.ProfiledWriteConnection.Execute(cmd,
                        new { measureId, gridAmperage, gridVoltage });
        }

        private void StoreGeneratorData(long measureId, double? amperage, double? voltage, double? wattage)
        {
            string cmd = @"Insert INTO generator 
                    (MeasureId, GeneratorAmperage, GeneratorVoltage, GeneratorWattage)
                    VALUES
                        (@measureId,@amperage,@voltage,@wattage);";
            base.ProfiledWriteConnection.Execute(cmd,
                        new { measureId, amperage, voltage, wattage });

        }

        private void StoreTemperature(long measureId, int temperature)
        {
            string cmd = @"INSERT INTO temperature
                                (MeasureId, temperature)
                        VALUES
                                (@measureId,@temperature);";
            base.ProfiledWriteConnection.Execute(cmd, new { measureId, temperature });
        }

        private long StoreWattage(DateTime dateTime, double wattage, int inverterId)
        {
            try
            {
                string cmd = @"INSERT INTO measure
                                (DateTime, OutputWattage, InverterId)
                        VALUES
                                (@dateTime, @wattage, @inverterId);
                        SELECT LAST_INSERT_ID() AS ID;";

                var result = base.ProfiledWriteConnection.Query(cmd, new { dateTime, wattage, inverterId }).FirstOrDefault();

                if (result == null)
                {
                    throw new ApplicationException("could not get measure Id after insertion, object is null");
                }
                else
                {
                    return (long)result.ID;
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex);

                //if (result != null)
                //{
                //  Logger.LogInfo(string.Format("{0}, inverter: {1}, wattage: {2}, type: {3}", dateTime, inverterId, wattage, result.GetType()));
                //}
                //else
                //{
                //  Logger.LogInfo(string.Format("{0}, inverter: {1}, wattage: {2}, result still null", dateTime, inverterId, wattage));
                //}
                throw;
            }
        }

        //    private int GetRealtimeMeasureCountForDateRange(DateTime startDate, DateTime endDate, int inverterId)
        //    {
        //      string cmd = @"SELECT COUNT(MeasureId) FROM measure 
        //                            WHERE `DateTime` BETWEEN @startDate AND @endDate
        //                            AND InverterId = @inverterId";

        //      return (int)ProfiledReadConnection.Query<long>(cmd, new { startDate, endDate, inverterId }).First();
        //    }

        public IList<Measure> GetLatestMeasuresByPlant(int plantId)
        {
            List<Measure> result = new List<Measure>();
            var inverterIds = ProfiledReadConnection.Query<int>("SELECT InverterId FROM inverter i WHERE i.PlantId = @plantId", new { plantId });

            foreach (var inverterId in inverterIds)
            {
                var measure = this.GetLatesMeasureByInverter(inverterId);

                if (measure != null)
                    result.Add(measure);
            }

            return result;
        }

        public Measure GetLatesMeasureByInverter(int inverterId)
        {

            string cmd = @"
SELECT i.InverterId as PrivateInverterId, i.*, m.* FROM temporary_measure m
INNER JOIN inverter i
  ON i.inverterId = m.InverterId
WHERE m.inverterId = @inverterId
ORDER BY m.DateTime DESC
LIMIT 1;";

            return ProfiledReadConnection.Query<Measure>(cmd, new { inverterId }).FirstOrDefault();
        }

        /// <summary>
        /// returns minutewise measures prepared for the flot line chart
        /// </summary>
        /// <param name="plantId">solar plant id</param>
        /// <param name="date">The requested day</param>
        /// <returns>multiple rows of type 'long'</returns>
        public FlotLineChartTable GetCumulatedMinuteWiseWattageChartData(int plantId, DateTime date)
        {
            var startDate = DateTimeUtils.CropHourMinuteSecond(date);
            var endDate = startDate.AddDays(1);

            var utcOffsetMs = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time").GetUtcOffset(DateTimeUtils.GetGermanNow()).TotalMilliseconds;

            string text = @"
SELECT (UNIX_TIMESTAMP(m.DateTime)*1000) as timeValue , ROUND(SUM(m.OutputWattage)) 
FROM inverter i 
	INNER JOIN measure m
		ON i.InverterId = m.InverterId
	WHERE i.PlantId = @plantId
        AND DateTime BETWEEN @startDate AND @endDate
	GROUP BY timeValue
	ORDER BY timeValue ASC;";

            var sqlCom = base.GetReadCommand(text);

            sqlCom.Parameters.AddWithValue("@plantId", plantId);
            sqlCom.Parameters.AddWithValue("@startDate", startDate);
            sqlCom.Parameters.AddWithValue("@endDate", endDate);

            var result = new FlotLineChartTable();

            using (var rdr = sqlCom.ExecuteReader())
            {
                while (rdr.Read())
                {
                    result.AddValue(rdr.GetDouble(0) + utcOffsetMs, rdr.GetDouble(1));
                }
            }
            return result;

        }

        /* MINUTE WISE WATTAGE */
        public IEnumerable<Measure> GetMinuteWiseMeasures(int inverterId)
        {
            string cmd = @"SELECT m.inverterId as PrivateInverterId, i.PublicInverterId, i.PlantId, m.*, t.Temperature, grd.*, gen.*  FROM measure m 
LEFT JOIN temperature t
	ON t.MeasureId = m.MeasureId
LEFT JOIN grid grd
	ON grd.MeasureId = m.MeasureId
LEFT JOIN generator gen
	ON gen.MeasureId = m.MeasureId
INNER JOIN inverter i
  ON i.inverterId = m.InverterId
WHERE m.InverterId = @inverterId;";

            return ProfiledReadConnection.Query<Measure>(cmd, new { inverterId });
        }

        /// <summary>
        /// Takes all temporary measures for the specified inverter, cumulates them to minutewise and stores them to the measure tables
        /// </summary>
        /// <param name="inverterId">The inverter which should be used for generating minutewise data</param>
        public void AggregateTemporaryToMinuteWiseMeasures(int inverterId)
        {
            // get latest datetime and set this as upper bound
            var endTime = this.GetLatestTemporaryMeasureDateTime(inverterId);

            //endtime is null if we don't have to calculate anything
            if (endTime != null)
            {
                //crop second information from date time
                endTime = DateTimeUtils.CropBelowSecondsInclusive(endTime.Value);

                /* get the measures minutewise cumulated */

                string selectSql = @"
	SELECT 
		tm.InverterId as PrivateInverterId
    , AVG(OutputWattage) as OutputWattage
    , CAST(DATE_FORMAT(DateTime, '%Y-%m-%d %H:%i:00') AS DATETIME) as croppedDate
    , i.PublicInverterId
    , i.PlantId
    , CAST(DATE_FORMAT(DateTime, '%Y-%m-%d %H:%i:00') AS DATETIME) as DateTime
    , AVG(tm.GeneratorVoltage) as GeneratorVoltage
    , AVG(tm.GeneratorWattage) as GeneratorWattage
    , AVG(tm.GeneratorAmperage) as GeneratorAmperage
    , AVG(tm.GridVoltage) as GridVoltage
    , AVG(tm.GridAmperage) as GridAmperage
    , MAX(Temperature) as Temperature

		  FROM temporary_measure tm
        INNER JOIN inverter i
          ON i.inverterId = tm.InverterId
		  WHERE tm.inverterId = @inverterId 
          AND `DateTime` < @endTime
		GROUP BY croppedDate;";

                var temporaryCumulated = ProfiledReadConnection.Query<Measure>(selectSql, new { inverterId, endTime });

                /* try to insert the cumulated measures into the measure table */
                foreach (var measure in temporaryCumulated)
                {
                    try
                    {
                        this.InsertMeasure(measure);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(ex, SeverityLevel.Warning, "could not store minute-wise measure plant: " + measure.PlantId);
                    }
                }

                /* delete all temporary measures as we have them now cumulated in the measure table */

                string deleteSql = @"
  DELETE FROM temporary_measure 
    WHERE DateTime < @endDate 
      AND inverterId = @inverterId;";

                ProfiledWriteConnection.Execute(deleteSql, new { inverterId, endDate = endTime.Value });

            }
        }

        /// <summary>
        /// Returns the datetime of the most recent measure in the temporary table
        /// </summary>
        /// <param name="inverterId"></param>
        /// <returns></returns>
        private DateTime? GetLatestTemporaryMeasureDateTime(int inverterId)
        {
            var result = ProfiledReadConnection.Query<DateTime>("SELECT MAX(DateTime) FROM temporary_measure WHERE InverterId = @inverterId GROUP BY InverterId",
              new { inverterId });

            if (result.Count() == 0)
                return null;
            else
                return result.First();
        }

        /// <summary>
        /// Returns measures only filled with Date, InverterID, PlantID and OutputWattage
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="inverterID"></param>
        /// <param name="systemID"></param>
        /// <returns></returns>
        public IEnumerable<Measure> GetMinuteWiseMeasures(DateTime startDate, DateTime endDate, int inverterID)
        {
            IEnumerable<Measure> measures = null;
            string sql = @"SELECT m.inverterId as PrivateInverterId, i.PublicInverterId, i.PlantId, m.*, t.Temperature, grd.*, gen.*  FROM measure m 
LEFT JOIN temperature t
	ON t.MeasureId = m.MeasureId
LEFT JOIN grid grd
	ON grd.MeasureId = m.MeasureId
LEFT JOIN generator gen
	ON gen.MeasureId = m.MeasureId
INNER JOIN inverter i
  ON i.inverterId = m.InverterId
    WHERE (i.InverterId = @inverterId) 
    AND (`DateTime` BETWEEN @startDate AND @endDate)
	Order By  DateTime ASC;";
            try
            {
                measures = ProfiledReadConnection.Query<Measure>(sql, new { inverterID, startDate, endDate });
            }
            catch (MySqlException ex)
            {
                //Logger.LogError(ex);
                throw;
            }
            return measures;
        }

        public List<FlotLineChartTable> GetInverterWiseMinuteWiseWattageChartData(int plantId, DateTime date)
        {
            var utcOffsetMs = DateTimeUtils.GetGermanTimeZone().GetUtcOffset(DateTimeUtils.GetGermanNow()).TotalMilliseconds;
            var startDate = DateTimeUtils.CropHourMinuteSecond(date);
            var endDate = startDate.AddDays(1);

            // sorting the measures is important, first publicInverterId then DateTime
            string text = @"
SELECT i.PublicInverterId, (UNIX_TIMESTAMP(m.DateTime)*1000) as timeValue , ROUND(m.OutputWattage)
FROM inverter i 
	INNER JOIN measure m
		ON i.InverterId = m.InverterId
	WHERE i.PlantId = @plantId
        AND DateTime BETWEEN @startDate AND @endDate
	ORDER BY i.PublicInverterId ASC, timeValue ASC;";

            var sqlCom = base.GetReadCommand(text);

            sqlCom.Parameters.AddWithValue("@plantId", plantId);
            sqlCom.Parameters.AddWithValue("@startDate", startDate);
            sqlCom.Parameters.AddWithValue("@endDate", endDate);

            List<FlotLineChartTable> result = new List<FlotLineChartTable>();
            FlotLineChartTable currentTable = new FlotLineChartTable();
            bool isFirst = true;
            int publicInverterId = -1;
            int currentInverterId;

            using (var rdr = sqlCom.ExecuteReader())
            {

                while (rdr.Read())
                {
                    //get the current inverter Id
                    currentInverterId = rdr.GetInt32(0);

                    if (isFirst)
                    {
                        //if this is the first read, initialize the public inverterId
                        publicInverterId = currentInverterId;
                        isFirst = false;
                        currentTable.SetSeriesNameForInverter(currentInverterId);
                    }

                    //read all measures one inverter after another
                    if (publicInverterId != currentInverterId)
                    {
                        // we have completed one wattage series and can build the next one

                        result.Add(currentTable);
                        currentTable = new FlotLineChartTable();
                        publicInverterId = currentInverterId;
                        currentTable.SetSeriesNameForInverter(currentInverterId);
                    }

                    // read the current row
                    currentTable.AddValue(rdr.GetDouble(1) + utcOffsetMs, rdr.GetDouble(2));
                }

                // add the last result table
                result.Add(currentTable);
            }
            return result;
        }

        public void Cleanup()
        {
            base.Dispose();
        }

        /* Temporary Measure */

        public void InsertTemporary(Measure measure)
        {
            ProfiledWriteConnection.Execute(@"
    INSERT INTO temporary_measure
	          (DateTime
          , `InverterId`
          , `OutputWattage`
          , `GeneratorVoltage`
          , `GeneratorWattage`
          , `GeneratorAmperage`
          , `GridVoltage`
          , `GridAmperage`
          ,  `Temperature`
            )
	VALUES (  @dateTime
          , @privateInverterId
          , @outputWattage
          , @generatorVoltage
          , @generatorWattage
          , @generatorAmperage
          , @gridVoltage
          , @gridAmperage
          , @temperature
          );",

              measure);
        }

        public void RemoveMeasuresOlderThan(int dayCount)
        {
            var dateTime = DateTimeUtils.GetGermanNow().AddDays(dayCount * (-1));

            var sql = "DELETE FROM measure WHERE DateTime < @dateTime";

            base.ProfiledWriteConnection.Execute(sql, new { dateTime });
        }
    }
}