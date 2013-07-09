using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PVLog.Utility;

namespace PVLog.Statistics
{
  /// <summary>
  /// This class is deprecated and can be removed after the beta test phase
  /// </summary>
    public class MinuteWiseAverageCalculator
    {
        Measure _resultMeasure;
        List<Measure> _inputList;

        public MinuteWiseAverageCalculator()
        {
            _resultMeasure = new Measure();
        }
       
        public List<Measure> CalculateAverage()
        {
            //return empty MeasureList if no measures where added
            if ((_inputList == null) || (_inputList.Count == 0))
            {
                return new List<Measure>();
            }

            //Get the average measures for all logminutes
            SortedList<DateTime, MeasureMinute> calcList = new SortedList<DateTime, MeasureMinute>();
            foreach (var measure in _inputList)
            {
                var currentMeasureTime = Utils.GetWith0Second(measure.DateTime);
                if (!calcList.ContainsKey(currentMeasureTime))
                {
                    calcList.Add(currentMeasureTime, new MeasureMinute(currentMeasureTime));
                }

                calcList[currentMeasureTime].AddMeasure(measure);
            }
            List<Measure> result = new List<Measure>();
            foreach (var logMinute in calcList.Values)
            {
                result.AddRange(logMinute.CalculateAverage());
            }

            return result;
        }

        public void AddMeasure(Measure measure)
        {
            if (_inputList == null)
            {
                _inputList = new List<Measure>();
            }
            _inputList.Add(measure);
        }
    }
}