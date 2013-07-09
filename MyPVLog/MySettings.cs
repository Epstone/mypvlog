using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using PVLog.Utility;
using System.Web.Configuration;

namespace PVLog
{
    public class MySettings
    {
        //this is a test
        static string _endHour = "DiagDay_EndHour";
        static string _startHour = "DiagDay_StartHour";
        static string _applicationLogLevel = "ApplicationLogLevel";
        static string _keepMeasureDayCount = "KeepMeasureDayCount";
      

        public static int DiagDay_StartHour
        {
            get
            {
                return Convert.ToInt32(GetSetting(_startHour));
            }
            set
            {
                SetSetting(_startHour, value);
            }
        }

        public static int DiagDay_EndHour
        {
            get
            {
                return Convert.ToInt32(GetSetting(_endHour));
            }
        }

        public static int KeepMeasureDayCount
        {
          get
          {
            return int.Parse(GetSetting(_keepMeasureDayCount));
          }
        }


        public static string ApplicationLogLevel
        {
            get { return GetSetting(_applicationLogLevel); }
            set { SetSetting(_applicationLogLevel, value); }
        }

        private static string GetSetting(string setting)
        {
            return ConfigurationManager.AppSettings[setting];
        }

        private static void SetSetting(string key, object value)
        {
            var config = ConfigurationManager.OpenExeConfiguration("~");
            config.AppSettings.Settings[key].Value = Convert.ToString(value);


            config.Save();

        }


    }
}


