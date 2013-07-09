using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using PVLog.Enums;

namespace PVLog.Models
{
  public class SortedKwhTable
  {
    //SortedList<DateTime, SortedList<int, IMeasure>> _theList;
    Dictionary<int, int> _knownInverterIDs;
    int _count = 0;
    public int Count { get { return _count; } }

    /// <summary>
    /// Key is the Private InverterId, Value = Public InverterId for User Interface
    /// </summary>
    public Dictionary<int, int> KnownInverters
    {
      get { return _knownInverterIDs; }
      private set { this._knownInverterIDs = value; }
    }

    public SortedList<DateTime, KwhTableRow> Rows { get; set; }


    //public SortedList<DateTime, SortedList<int, IMeasure>> MeasureListForUi
    //{
    //    get { return this._theList; }
    //    private set { this._theList = value; }
    //}

    public SortedKwhTable()
    {
      _knownInverterIDs = new Dictionary<int, int>();
      //_theList = new SortedList<DateTime, SortedList<int, IMeasure>>();
      this.Rows = new SortedList<DateTime, KwhTableRow>();
    }

    /// <summary>
    /// Adds a new measure item to the list
    /// </summary>
    /// <param name="measure"></param>
    public void AddMeasure(IMeasure measure)
    {
      //add a new sorted list if neccesary
      if (!this.Rows.ContainsKey(measure.DateTime))
      {
        this.Rows.Add(measure.DateTime, new KwhTableRow(measure.DateTime));
      }

      //Add the measure
      this.Rows[measure.DateTime].kwhValues.Add(measure);
      _count++;

      //Add the inverter to the Inverter List if neccessary
      if (!this._knownInverterIDs.ContainsKey(measure.PrivateInverterId))
      {
        this._knownInverterIDs.Add(measure.PrivateInverterId, measure.PublicInverterId);
      }
    }

    /// <summary>
    /// Returns all containing measures in a List
    /// </summary>
    /// <returns></returns>
    public List<IMeasure> ToList()
    {
      List<IMeasure> result = new List<IMeasure>();

      foreach (var row in Rows.Values)
      {
        foreach (var measure in row.kwhValues)
        {
          result.Add(measure);
        }
      }
      return result;
    }


    public IMeasure GetKwh(DateTime currentTimePoint, int publicInverterId)
    {
      return this.Rows[currentTimePoint].GetKwh(publicInverterId);
    }

    internal bool ContainsMeasureForPublicInverterId(DateTime dateTime, int publicInverterID)
        {
          if (this.Rows.ContainsKey(dateTime))
            return (this.Rows[dateTime].kwhValues.Any(x => x.PublicInverterId == publicInverterID));
          else
            return false;
        }
  }
}