using System;
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
        private readonly IInverterTrackerRegistry _inverterTrackerRegistry;
        private InverterTracker _inverterTracker;

        public LogController()
        {

        }

        public LogController(I_MeasureRepository measureRepository, I_PlantRepository plantRepository, IInverterTrackerRegistry inverterTrackerRegistry)
        {
            _inverterTrackerRegistry = inverterTrackerRegistry;
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
                    UpdateMinuteWiseMeasures(measure);

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

                    UpdateMinuteWiseMeasures(measure);
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
                    UpdateMinuteWiseMeasures(measure);
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

        private void UpdateMinuteWiseMeasures(Measure measure)
        {
            _plantRepository.SetPlantOnline(measure.PlantId, DateTime.UtcNow);
            _inverterTrackerRegistry.CreateOrGetTracker(measure.PrivateInverterId);
            _inverterTracker.TrackMeasurement(measure);
            var averagesForMinutes = _inverterTracker.GetAveragesForMinutes();

            if (averagesForMinutes.Count > 0)
            {
                _measureRepository.InsertMeasure(measure);
            }
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
}
