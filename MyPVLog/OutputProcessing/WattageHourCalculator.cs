using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.OutputProcessing
{
  /// <summary>
  /// Calculates the WattageHour sum for any given time span. Used for min_day.js production
  /// </summary>
  public class WattageHourCalculator
  {
    Dictionary<int, double> inverterWattageHourResult = new Dictionary<int, double>();

    /// <summary>
    /// Add a power wattage value and a timespan for that
    /// </summary>
    /// <param name="inverterId">The inverterId which produces the wattage value.</param>
    /// <param name="wattage">The power value itself.</param>
    /// <param name="duration">The duration, how long we had this power.</param>
    public void AddPowerValue(int inverterId, double wattage, TimeSpan duration)
    {
      if (!this.inverterWattageHourResult.ContainsKey(inverterId))
        this.inverterWattageHourResult.Add(inverterId, 0);

      this.inverterWattageHourResult[inverterId] += wattage / (60 / duration.TotalMinutes);

    }

    /// <summary>
    /// Builds the result wattage hour
    /// </summary>
    /// <param name="inverterId">The inverterId from whom we want the result</param>
    /// <returns>A rounded integer wattage hour result</returns>
    public int GetWattageHour(int inverterId)
    {
      return (int)Math.Round(this.inverterWattageHourResult[inverterId]);
    }
  }
}