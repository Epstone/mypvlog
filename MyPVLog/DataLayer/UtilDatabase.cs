using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVLog.Utility;
using System.Data;
using MySql.Data.MySqlClient;
using MySqlRepository;
using System.Configuration;
using Dapper;

namespace PVLog.DataLayer
{
  public class UtilDatabase : MySqlRepositoryBase
  {
    public UtilDatabase()
    {
      var connStr = ConfigurationManager.ConnectionStrings["pv_data"].ConnectionString;
      base.Initialize(connStr, connStr);
    }

    public void AddLog(SeverityLevel level, string ExceptionMessage, string ExceptionStacktrace, string customMessage)
    {
      var sqlCom = GetWriteCommand(@"INSERT INTO logs 
                                 (
                                  LogLevel,
                                  ExceptionMessage,
                                  ExceptionStacktrace,
                                  CustomMessage,
                                  Date
                                  )
                                Values
                                (
                                  @LogLevel,
                                  @ExceptionMessage,
                                  @ExceptionStacktrace,
                                  @CustomMessage,
                                  @Date
                                );");

      sqlCom.Parameters.AddWithValue("@LogLevel", level.ToString());
      sqlCom.Parameters.AddWithValue("@ExceptionMessage", ExceptionMessage);
      sqlCom.Parameters.AddWithValue("@ExceptionStacktrace", ExceptionStacktrace);
      sqlCom.Parameters.AddWithValue("@CustomMessage", customMessage);
      sqlCom.Parameters.AddWithValue("@Date", Utils.GetGermanNow());

      sqlCom.ExecuteNonQuery();
    }

    public IEnumerable<AppLog> GetAllLogs(string severityLevel, int count)
    {
      if (string.IsNullOrEmpty(severityLevel))
        severityLevel = "%";


      return ProfiledReadConnection.Query<AppLog>(@"SELECT * FROM logs 
                                                          WHERE (LogLevel like @logLevel )
                                                            ORDER BY Date DESC
                                                            LIMIT @count;", new { logLevel = severityLevel, count });


    }

    internal void ClearLogs()
    {
      string text = "TRUNCATE TABLE logs";
      var sqlCom = GetWriteCommand(text);

      try
      {
        sqlCom.ExecuteNonQuery();
      }
      catch (MySqlException ex)
      {
        Logger.LogError(ex);
      }
    }

    internal IEnumerable<AppLog> GetAllLogs(int count)
    {
      return GetAllLogs("", count);
    }

    public void Cleanup()
    {
      base.Dispose();
    }
  }
}