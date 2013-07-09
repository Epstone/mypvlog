using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;
using PVLog.Statistics;
using PVLog.Utility;
using PVLog.Models;
using System.Linq;
using MySqlRepository;
using System.Configuration;
using Dapper;

namespace PVLog.DataLayer
{
  public class KwhRepository : MySqlRepositoryBase
  {
    public KwhRepository()
    {
      var connStr = ConfigurationManager.ConnectionStrings["pv_data"].ConnectionString;
      base.Initialize(connStr, connStr);
    }

    public KwhRepository(string connectionString)
    {
      base.Initialize(connectionString, connectionString);
    }

    public void InsertDayKwh(MeasureKwH kwhDay)
    {
      string text = @"INSERT INTO kwh_by_day (Date, InverterId, kwh)
                                VALUES (@date, @inverterId, @kwh)
                            ON DUPLICATE KEY UPDATE kwh = @kwh;";
      try
      {
        ProfiledWriteConnection.Execute(text, new
        {
          inverterId = kwhDay.PrivateInverterId,
          date = kwhDay.DateTime,
          kwh = kwhDay.Value
        });

      }
      catch (MySqlException ex)
      {
        Logger.LogError(ex);
        throw ex;
      }
    }

    public SortedKwhTable GetDayKwhByDateRange(DateTime startDate, DateTime endDate, int systemID)
    {
      startDate = Utils.CropHourMinuteSecond(startDate);

      //mysql handles "between date" inclusive so we have to shrink the timeframe
      endDate = Utils.CropHourMinuteSecond(endDate).AddDays(-1);


      SortedKwhTable result = new SortedKwhTable();
      string text = @"
SELECT ID, Date, i.InverterId, i.PublicInverterId, kwh 
    FROM kwh_by_day k
		INNER JOIN inverter i                 
          ON k.InverterID = i.InverterId    
            AND i.PlantId = @plantId            
WHERE                                       
     (DATE BETWEEN @startDate AND @endDate)
ORDER BY Date ASC, PublicInverterId ASC;";
      var sqlCom = base.GetReadCommand(text);

      //Add Parameters
      sqlCom.Parameters.AddWithValue("@plantID", systemID);
      sqlCom.Parameters.AddWithValue("@startDate", startDate);
      sqlCom.Parameters.AddWithValue("@endDate", endDate);

      //Execute SQL and read data
      using (var rdr = sqlCom.ExecuteReader())
      {
        while (rdr.Read())
        {
          var measureKwh = new MeasureKwH();
          measureKwh.Value = rdr.GetDouble("kwh");
          measureKwh.ID = rdr.GetInt32("ID");
          measureKwh.DateTime = rdr.GetDateTime("Date");
          measureKwh.PublicInverterId = rdr.GetInt32("PublicInverterId");
          measureKwh.TimeMode = Enums.E_TimeMode.day;
          measureKwh.PrivateInverterId = rdr.GetInt32("InverterId");

          result.AddMeasure(measureKwh);
        }
      }
      return result;
    }

    internal double? GetKwhByDate(int id, DateTime date)
    {
      string query = @"
SELECT SUM(k.kwh) FROM kwh_by_day k
	INNER JOIN inverter i
		ON i.InverterId = k.InverterID
			AND i.PlantId = @plantId
   WHERE k.Date = @date
   GROUP BY i.PlantId;";

      double? result = null;

      try
      {
        result = ProfiledReadConnection.Query<double>(query, new
        {
          date,
          plantId = id
        }).First();
      }
      catch (Exception)
      {
        Logger.LogInfo("could not get todays kwh for plantId: " + id);
      }

      return result;
    }
    public void Cleanup()
    {
      base.Dispose();
    }

    internal DateTime GetFirstDateOfKwhDay(int plantId)
    {
      return ProfiledReadConnection.Query<DateTime>(@"
SELECT Coalesce(min(k.Date),NOW()) FROM kwh_by_day k
INNER JOIN inverter i
ON i.InverterId = k.InverterID
AND i.PlantId = @plantId;", new { plantId }).First();
    }
  }
}