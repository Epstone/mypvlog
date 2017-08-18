using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Configuration;
using PVLog.DataLayer;
using System.IO;

namespace solar_tests.DatabaseTest
{
    [SetUpFixture]
    public class TestDbSetup
    {

        string _connectionString;

        public TestDbSetup()
        {
          _connectionString = ConfigurationManager.ConnectionStrings["pv_data"].ConnectionString;
        }

        private void ExecuteSqlFromFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string sqlText = File.ReadAllText(path);
                    using (var mySqlConn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
                    {
                        mySqlConn.Open();
                        var sqlCom = mySqlConn.CreateCommand();
                        sqlCom.CommandText = sqlText;

                        sqlCom.ExecuteNonQuery();
                        mySqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [TearDown]
        public void TearDown()
        {
            
        }


        
        internal void TruncateAllTables()
        {
            using (var mySqlConn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                mySqlConn.Open();
                var sqlCom = mySqlConn.CreateCommand();
                sqlCom.CommandText = @"
SET foreign_key_checks = 0;
TRUNCATE TABLE generator;
TRUNCATE TABLE grid;
TRUNCATE TABLE inverter;
TRUNCATE TABLE kwh_by_day;
TRUNCATE TABLE logs;
TRUNCATE TABLE measure;
TRUNCATE TABLE plants;
TRUNCATE TABLE temperature;
TRUNCATE TABLE temporary_measure;
TRUNCATE TABLE user_has_plant;
SET foreign_key_checks = 1;";
                sqlCom.ExecuteNonQuery();
                
                mySqlConn.Close();
            }
        }



        internal void TruncateMinuteWiseMeasures()
        {
            using (var mySqlConn = new MySql.Data.MySqlClient.MySqlConnection(_connectionString))
            {
                mySqlConn.Open();
                var sqlCom = mySqlConn.CreateCommand();
                sqlCom.CommandText = @"TRUNCATE TABLE minute_wise;";
                sqlCom.ExecuteNonQuery();

                mySqlConn.Close();
            }
        }
    }
}
