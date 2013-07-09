using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Statistics
{
  /// <summary>
  /// This class is deprecated and can be removed after the beta test phase
  /// </summary>
    public class MeasureMinute
    {
        DateTime currentMeasureTime;
        SortedList<int, List<Measure>> _measures = new SortedList<int, List<Measure>>();

        public MeasureMinute(DateTime currentMeasureTime)
        {
            // TODO: Complete member initialization
            this.currentMeasureTime = currentMeasureTime;
        }
        public void AddMeasure(Measure measure)
        {
            if (!_measures.ContainsKey(measure.PrivateInverterId))
            {
                _measures.Add(measure.PrivateInverterId, new List<Measure>());
            }
            _measures[measure.PrivateInverterId].Add(measure);
        }

        public List<Measure> CalculateAverage()
        {
            List<Measure> result = new List<Measure>();
            foreach (var measureList in _measures.Values)
            {
                Measure avgMeasure = CalcAvg(measureList);
                result.Add(avgMeasure);
            }

            return result;
        }

        private Measure CalcAvg(List<Measure> measureList)
        {
            var avgMeasure = new Measure();

            foreach (var measure in measureList)
            {
                avgMeasure.GeneratorAmperage += measure.GeneratorAmperage;
                avgMeasure.GeneratorVoltage += measure.GeneratorVoltage;
                avgMeasure.GeneratorWattage += measure.GeneratorWattage;
                avgMeasure.GridAmperage += measure.GridAmperage;
                avgMeasure.GridVoltage += measure.GridVoltage;
                avgMeasure.OutputWattage += measure.OutputWattage;
            }

            //constant values
            int counter = measureList.Count;
            int lastIndex = measureList.Count - 1;

            avgMeasure.DateTime = currentMeasureTime;
            avgMeasure.PlantId = measureList[lastIndex].PlantId;
            avgMeasure.PrivateInverterId = measureList[lastIndex].PrivateInverterId;
            avgMeasure.SystemStatus = measureList[lastIndex].SystemStatus;
            avgMeasure.Temperature = measureList[lastIndex].Temperature;

            //calc average
            avgMeasure.GeneratorAmperage = avgMeasure.GeneratorAmperage / counter;
            avgMeasure.GeneratorVoltage = avgMeasure.GeneratorVoltage / counter;
            avgMeasure.GeneratorWattage = avgMeasure.GeneratorWattage / counter;
            avgMeasure.GridAmperage = avgMeasure.GridAmperage / counter;
            avgMeasure.GridVoltage = avgMeasure.GridVoltage / counter;
            avgMeasure.OutputWattage = avgMeasure.OutputWattage / counter;

            return avgMeasure;
        }
    }
}