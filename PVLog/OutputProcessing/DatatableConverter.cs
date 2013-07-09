using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using PVLog.Enums;
using PVLog.Models;
using Bortosky.Google.Visualization;
using PVLog.Utility;

namespace PVLog.OutputProcessing
{
    public class DatatableConverter
    {
        SortedList<int, double> _euroPerKwH = new SortedList<int, double>();

        /// <summary>
        /// Creates a Table by a given "Sorted Measure List" with one date time column and one column for each inverter.
        /// </summary>
        /// <param name="kwhTable">Creates google data table compatible conent</param>
        /// <param name="yMode">Use Euro or kwh as y-axis</param>
        /// <param name="xMode">type of x-axis</param>
        /// <returns>Google DataTable content</returns>
        public string BuildGoogleDataTable(SortedKwhTable kwhTable, E_EurKwh yMode, E_TimeMode xMode) //, Type dateType)
        {
            //create datatable and add time column
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("Zeit", typeof(string)).Caption = "Zeit";

            //add one column for each inverter ID
            foreach (var inverterInfo in kwhTable.KnownInverters)
            {
                string caption = GetRowName(inverterInfo.Value);
                dt.Columns.Add(caption, typeof(System.Double)).Caption = caption;
            }

            //Add the data
            foreach (var row in kwhTable.Rows.Values)
            {
                //create a new row and add the time first (key)
                var rowToAdd = dt.NewRow();
                rowToAdd[0] = GetTimeCaption(row.Index, xMode);

                //add the values foreach inverter
                foreach (var measure in row.kwhValues)
                {
                    rowToAdd[GetRowName(measure.PublicInverterId)] = measure.Value * GetPerEuroFactor(yMode, measure.PublicInverterId);
                }

                //add the new row to the datatable
                dt.Rows.Add(rowToAdd);
            }

            GoogleDataTable gdt = new Bortosky.Google.Visualization.GoogleDataTable(dt);
            return gdt.GetJson();
        }

      

        /// <summary>
        /// Get the caption for the x-axis. Datetime is used for googledatatable, where the 
        /// real datetime value is needed.
        /// Otherwise use month name, or year number
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="xMode"></param>
        /// <param name="dateType"></param>
        /// <returns></returns>
        private object GetTimeCaption(DateTime dateTime, E_TimeMode xMode) // Type dateType)
        {
            object time;

            //if (dateType == typeof(DateTime))
            //{
            //    time = dateTime;
            //}
            //else
            //{//evaluate x-axis text

            switch (xMode)
            {
                 case E_TimeMode.hour:
                    time = dateTime.ToShortTimeString();
                    break;
                case E_TimeMode.day:
                    time = dateTime.Day.ToString();
                    break;
                case E_TimeMode.month:
                    time = dateTime.ToString("MMM");
                    break;
                case E_TimeMode.year:
                    time = dateTime.Year.ToString();
                    break;
                default:
                    throw new ApplicationException("Could not get Time Caption");
            }

            return time;
        }

        private double GetPerEuroFactor(E_EurKwh yMode, int inverterID)
        {
            switch (yMode)
            {
                case E_EurKwh.money:
                    return _euroPerKwH[inverterID];
                case E_EurKwh.kwh:
                    return 1;
                case E_EurKwh.watts:
                    return 1;
                default:
                    throw new ApplicationException();
            }

        }

        private static string GetRowName(int inverterID)
        {
            return "Inv " + inverterID.ToString();
        }

        public void AddEuroPerKwH(int inverterID, double eur)
        {
            _euroPerKwH.Add(inverterID, eur);
        }



       
    }
}