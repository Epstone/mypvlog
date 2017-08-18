namespace PVLog.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Models;
    using MySql.Data.MySqlClient;

    public static class Utils
    {
        public static SortedList<DateTime, IMeasure> ConvertToSortedList(List<IMeasure> measures)
        {
            var result = new SortedList<DateTime, IMeasure>();
            foreach (var averageMeasure in measures)
            {
                try
                {
                    result.Add(averageMeasure.DateTime, averageMeasure);
                }
                catch (Exception ex)
                {
                    Logger.Log(ex, SeverityLevel.Error, "measure mit ist mehrfach vorhanden: " + averageMeasure.DateTime);
                }
            }

            return result;
        }

        /// <summary>
        ///     Gets the SH a1 hash.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string GetSHA1Hash(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Can't create hash from empty string");
            }

            var SHA1 = new SHA1CryptoServiceProvider();

            byte[] arrayData;
            byte[] arrayResult;
            string result = null;
            string temp = null;

            arrayData = Encoding.ASCII.GetBytes(text);
            arrayResult = SHA1.ComputeHash(arrayData);
            for (int i = 0; i < arrayResult.Length; i++)
            {
                temp = Convert.ToString(arrayResult[i], 16);
                if (temp.Length == 1)
                {
                    temp = "0" + temp;
                }
                result += temp;
            }
            return result;
        }

        public static string sqlCommandToString(MySqlCommand c, bool bVerbose)
        {
            string retval = c.CommandText + " ";
            MySqlParameter p;
            for (int i = 0; i < c.Parameters.Count; i++)
            {
                p = c.Parameters[i];
                if (bVerbose)
                {
                    retval = retval + p.ParameterName + "(" + p.MySqlDbType + ")=" + p.Value + ", ";
                }
                else
                {
                    retval = retval + "'" + p.Value + "',";
                }
            }
            return retval;
        }

        public static string GenerateRandomString(int p)
        {
            string rStr = Path.GetRandomFileName();
            rStr = rStr.Replace(".", "").Substring(0, p); // For Removing the .
            return rStr;
        }

        internal static List<int> SplitToIntList(string rawIDs)
        {
            var options = StringSplitOptions.RemoveEmptyEntries;
            return rawIDs.Split(new[] {','}, options).ToList().ConvertAll(s => Convert.ToInt32(s));
        }
    }
}