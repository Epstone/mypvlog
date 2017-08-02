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
TRUNCATE TABLE plants;
TRUNCATE TABLE user_has_plant;
TRUNCATE TABLE kwh_by_day;
TRUNCATE TABLE user_has_plant;
TRUNCATE TABLE measure;
TRUNCATE TABLE temporary_measure;
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
