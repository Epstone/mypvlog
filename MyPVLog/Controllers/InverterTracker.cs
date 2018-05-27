using System;
using System.Collections.Generic;
using System.Linq;
using PVLog.Utility;

namespace PVLog.Controllers
{
    public class InverterTracker
    {
        private readonly object _syncroot = new object();
        readonly List<Measure> _measures = new List<Measure>();
        private int _inverterId;
        private static readonly TimeSpan OneMinute = TimeSpan.FromMinutes(1);

        public InverterTracker(int inverterId)
        {
            this._inverterId = inverterId;
        }

        public void TrackMeasurement(Measure sample)
        {
            lock (_syncroot)
            {
                this._measures.Add(sample);
            }
        }

        public void TrackMeasurements(IList<Measure> measures)
        {
            lock (_syncroot)
            {
                this._measures.AddRange(measures);
            }
        }

        public IList<Measure> GetAveragesForMinutes()
        {
            lock (_syncroot)
            {
                return this.CalculateMinutesForInverter().ToList();
            }
        }

        private List<Measure> CalculateMinutesForInverter()
        {
            var maxDatetime = this._measures.Max(x => x.DateTime);
            var minDateTime = this._measures.Min(x => x.DateTime);

            if (maxDatetime - minDateTime < OneMinute)
            {
                return new List<Measure>();
            }

            var minuteLimitDateTime = DateTimeUtils.CropBelowSecondsInclusive(maxDatetime);


            var minutes = this._measures.Where(x => x.DateTime < minuteLimitDateTime)
                .GroupBy(x => x.DateTime.Ticks / OneMinute.Ticks, m => m, (l, measures) => measures)
                .Select(AverageSamplesToOneMinute).ToList();

            this._measures.RemoveAll(x => x.DateTime < maxDatetime);

            return minutes;
        }

        private static Measure AverageSamplesToOneMinute(IEnumerable<Measure> arg)
        {
            if (!arg.Any())
            {
                return null;
            }

            var average = new Measure();
            var firstSample = arg.First();
            average.DateTime = DateTimeUtils.CropBelowSecondsInclusive(firstSample.DateTime);
            average.Value = arg.Average(m => m.Value);
            average.GeneratorAmperage = arg.Average(m => m.GeneratorAmperage);
            average.GeneratorVoltage = arg.Average(m => m.GeneratorVoltage);
            average.GeneratorWattage = arg.Average(m => m.GeneratorWattage);
            average.GridAmperage = arg.Average(m => m.GridAmperage);
            average.GridVoltage = arg.Average(m => m.OutputWattage);
            average.OutputWattage = arg.Average(m => m.OutputWattage);
            average.PlantId = firstSample.PlantId;
            average.PrivateInverterId = firstSample.PrivateInverterId;
            average.PublicInverterId = firstSample.PublicInverterId;
            average.SystemStatus = firstSample.SystemStatus;
            average.Temperature = arg.Max(m => m.Temperature);

            return average;
        }

        public int GetSampleCount()
        {
            lock (_syncroot)
            {
                return this._measures.Count;
            }
        }

        public Measure GetLastestMeasure()
        {
            lock (this._syncroot)
            {
                return this._measures.LastOrDefault();
            }
        }
    }
}
