using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PVLog.OutputProcessing;
using PVLog.Models;

namespace PVLog.DataLayer
{
    public interface IMeasureRepository : IDisposable
    {
        long InsertMeasure(Measure measure);

        IEnumerable<Measure> GetMinuteWiseMeasures(int inverterId);
        
        IEnumerable<Measure> GetMinuteWiseMeasures(DateTime startDate, DateTime endDate, int inverterID);

        FlotLineChartTable GetCumulatedMinuteWiseWattageChartData(int plantId, DateTime date);

        List<FlotLineChartTable> GetInverterWiseMinuteWiseWattageChartData(int plantId, DateTime date);

        void InsertTemporary(Measure measure);

        void RemoveMeasuresOlderThan(int dayCount);
    }
}
