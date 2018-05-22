﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVLog.DataLayer;
using System.Globalization;
using PVLog.InputProcessing;
using System.Text;
using PVLog.Utility;

namespace PVLog.Controllers
{
    using System.Net;

    public class LogController : MyController
    {
        private MinuteWiseAggregator _minuteWiseAggregator;

        public LogController()
        {

        }

        public LogController(I_MeasureRepository measureRepository, I_PlantRepository plantRepository)
        {
            this._measureRepository = measureRepository;
            this._plantRepository = plantRepository;
        }

        public ActionResult Kaco1(string data, int plant, string pw, int inverter)
        {
            int publicInverterId = inverter;

            //validate plant id and password
            if (IsValidPlant(plant, pw))
            {
                // check inverterId or create one if autocreate is activated
                try
                {
                    //get private inverterId for the public inverterId
                    int privateInverterId = ValidateGetPrivateInverterId(plant, publicInverterId);

                    //parse the measure
                    var measure = MeasureReader.ReadKaco1Data(data, plant, privateInverterId);

                    //store measure in repository and return the success view
                    TrackPlantAcitivity(measure);

                    return new HttpStatusCodeResult(HttpStatusCode.OK);

                }
                catch (ArgumentException ex)
                {
                    LogMeasurementError(plant, ex, nameof(Kaco1));
                    return LogFailResult();
                }
            }

            return InvalidPlantResult();
        }

        private static HttpStatusCodeResult LogFailResult()
        {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Log Error - please contact me at Patrickeps@gmx.de");
        }

        private static HttpStatusCodeResult InvalidPlantResult()
        {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "invalid plant");
        }

        private static void LogMeasurementError(int plant, ArgumentException ex, string endpoint)
        {
            Logger.Log(ex, SeverityLevel.Verbose, $"Error saving {endpoint} measure for plantId:{plant}");
        }

        public ActionResult Kaco2(string data, int plant, string pw)
        {
            if (IsValidPlant(plant, pw))
            {
                try
                {
                    var measure = MeasureReader.ReadKaco2Data(data, plant);

                    measure.PrivateInverterId = ValidateGetPrivateInverterId(plant, measure.PublicInverterId);

                    TrackPlantAcitivity(measure);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (ArgumentException ex)
                {
                    LogMeasurementError(plant, ex, nameof(Kaco2));
                    return LogFailResult();
                }

            }
            return InvalidPlantResult();
        }

        private bool IsValidPlant(int plant, string pw)
        {
            bool isValid = false;

            try
            {
                isValid = _plantRepository.IsValidPlant(plant, pw);
            }
            catch (ArgumentException ex)
            {
                Logger.LogError(ex);
            }

            return isValid;

        }

        public ActionResult Generic(int plant, string pw, int inverter, double feedinpower,
                                    double? gridvoltage, double? gridcurrent,
                            double? generatorvoltage, double? generatorcurrent, double? generatorpower, int? systemstatus, int? temperature, long? timestamp)
        {
            int publicInverterId = inverter;

            if (IsValidPlant(plant, pw))
            {
                // check inverterId or create one if autocreate is activated
                try
                {
                    //get private inverterId for the public inverterId
                    int privateInverterId = ValidateGetPrivateInverterId(plant, publicInverterId);

                    //parse the measure
                    var measure = new Measure()
                    {
                        DateTime = (timestamp == null) ? DateTimeUtils.GetGermanNow()
                                                     : DateTimeUtils.UnixTimeStampToDateTime(timestamp.Value),
                        GeneratorAmperage = generatorcurrent,
                        GeneratorVoltage = generatorvoltage,
                        GeneratorWattage = generatorpower,
                        GridAmperage = gridcurrent,
                        GridVoltage = gridvoltage,
                        OutputWattage = feedinpower,
                        PlantId = plant,
                        PrivateInverterId = privateInverterId,
                        PublicInverterId = publicInverterId,
                        SystemStatus = systemstatus,
                        Temperature = temperature

                    };

                    //store measure in repository and return the success view
                    TrackPlantAcitivity(measure);
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                catch (ArgumentException ex)
                {
                    LogMeasurementError(plant, ex, nameof(Generic));
                    return LogFailResult();
                }

            }

            return InvalidPlantResult();
        }

        private void TrackPlantAcitivity(Measure measure)
        {
            _plantRepository.SetPlantOnline(measure.PlantId, DateTime.UtcNow);
            _measureRepository.InsertTemporary(measure);
            _minuteWiseAggregator = new MinuteWiseAggregator();
        }


        /// <summary>
        /// Returns the private inverterId by a given public inverterId and plantId. Throws an 
        /// Argument Exception if the publich inverter Id is unknown and auto creation is disabled.
        /// </summary>
        /// <param name="plant">The solar plant Id</param>
        /// <param name="publicInverterId">The public InverterId for this inverter</param>
        /// <returns>The private inverterId if available</returns>
        private int ValidateGetPrivateInverterId(int plant, int publicInverterId)
        {
            int privateInverterId = -1;
            if (_plantRepository.IsValidInverter(plant, publicInverterId))
            {
                //exchange public and private inverterId if the public inverterId is valid for this plant
                privateInverterId = _plantRepository.GetPrivateInverterId(plant, publicInverterId);

            }
            else if (_plantRepository.IsAutoCreateInverterActive(plant))
            {
                //automatically add a new inverter for this plant
                privateInverterId = _plantRepository.CreateInverter(plant, publicInverterId, 0.1F, "Generator (neu)");

            }
            else
            {
                //the inverter is not yet existing and auto create inverters is turned off.
                throw new ArgumentException("Inverter is not valid for this plant");
            }
            return privateInverterId;
        }

    }

    public class MinuteWiseAggregator
    {
        private object syncroot = new object();
        List<Measure> measures = new List<Measure>();

        public void TrackMeasurement(Measure sample)
        {
            lock (syncroot)
            {
                this.measures.Add(sample);
            }
        }


        /// <summary>
        /// Measures contain only measures from one plant at a time
        /// </summary>
        /// <param name="measures"></param>
        /// <param name="inverterId"></param>
        public void UpdateMinuteWiseToDatabase(List<Measure> measures, int inverterId)
        {


        }

        public void TrackMeasurements(IList<Measure> measures)
        {
            lock (syncroot)
            {
                this.measures.AddRange(measures);
            }
        }

        public IList<Measure> GetAveragesForMinutes()
        {
            lock (syncroot)
            {
                var samplesPerInverter = this.measures.GroupBy(x => x.PrivateInverterId, x => x, (key, list) => list.ToList());

                var result = samplesPerInverter.Select(CalculateMinutesForInverter).SelectMany(x => x).ToList();

                foreach (var measure in this.MeasuresToRemove)
                {
                    this.measures.Remove(measure);
                }
                this.MeasuresToRemove.Clear();

                return result;
            }
        }

        private List<Measure> CalculateMinutesForInverter(List<Measure> inverterMeasures)
        {
            var maxDatetime = inverterMeasures.Max(x => x.DateTime);

            var minuteLimitDateTime = DateTimeUtils.CropBelowSecondsInclusive(maxDatetime);

            TimeSpan oneMinute = TimeSpan.FromMinutes(1);

            var minutes = inverterMeasures.Where(x => x.DateTime < minuteLimitDateTime)
                .GroupBy(x => x.DateTime.Ticks / oneMinute.Ticks, m => m, (l, measures) => measures)
                .Select(AverageSamplesToOneMinute).ToList();

            MarkMeasuresForDeletion(minuteLimitDateTime, inverterMeasures);

            return minutes;
        }

        internal List<Measure> MeasuresToRemove = new List<Measure>();

        private void MarkMeasuresForDeletion(DateTime minuteLimitDateTime, List<Measure> inverterMeasures)
        {
            this.MeasuresToRemove.AddRange(inverterMeasures.Where(x => x.DateTime < minuteLimitDateTime));
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
            lock (syncroot)
            {
                return this.measures.Count;
            }
        }
    }
}
