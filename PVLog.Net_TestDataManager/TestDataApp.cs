using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PVLog.DataLayer;
using MySql.Data.MySqlClient;
using PVLog;
using System.Linq;
namespace File_Renamer
{
    public partial class TestDataApp : Form
    {

        public TestDataApp()
        {
            InitializeComponent();

            dtp_Source.Value = new DateTime(2011, 1, 22);
            dtp_target_From.Value = DateTime.Now;
            dtp_Target_To.Value = DateTime.Now.AddDays(7);
        }

        private void btn_createData_Click(object sender, EventArgs e)
        {
            //List<Measure> result = null;
            //string devConStr = GetTargetConnectionString("Development");

            //var measureDb = new MeasureRepository(devConStr, devConStr);

            //DateTime startDate = dtp_Source.Value.Date;
            //DateTime endDate = startDate.AddDays(1);

            //var measures = measureDb.GetAllMeasures(1);
            //result = (from measure in measures
            //          where (measure.DateTime > startDate && measure.DateTime < endDate)
            //          orderby measure.DateTime ascending
            //          select measure).ToList();


            //string targetConStr = GetTargetConnectionString(lbx_target.SelectedItem.ToString());

            //if (!string.IsNullOrEmpty(targetConStr))
            //{
            //    InsertToTarget(result, targetConStr);
            //}

            throw new NotImplementedException();

        }

        private void InsertToTarget(List<Measure> result, string targetConStr)
        {
            DateTime startDate = GetDay(dtp_target_From.Value);
            DateTime endDate = GetDay(dtp_Target_To.Value).AddDays(1);

            DateTime countDate = startDate;

            var db = new MeasureRepository(targetConStr, targetConStr);


            while (countDate < endDate)
            {
                foreach (var measure in result)
                {
                    var oldDate = measure.DateTime;
                    var newDate = new DateTime(countDate.Year, countDate.Month, countDate.Day, oldDate.Hour, oldDate.Minute, oldDate.Second);
                    measure.DateTime = newDate;

                    db.InsertMeasure(measure);
                }
                //increase countdate
                countDate = countDate.AddDays(1);
            }


        }

        private DateTime GetDay(DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
        }

        private string GetTargetConnectionString(string selectedValue)
        {
            if (selectedValue == "Development")
            {
                return "Data Source=localhost;user id=dev_logger;password=Winter2010;database=dev_pv_data;";
            }
            else if (selectedValue == "Test")
            {
                return "Data Source=192.168.0.75;user id=solar_admin;password=Winter2010;database=tst_pv_data;";
            }
            else
            {
                throw new ApplicationException();
            }
        }

    }
}
