using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace solar_tests
{
  public static class ObjectExtensions
  {
    public static bool PropertiesEqual<T>(this T obj1, T obj2)
    {
      return typeof(T).GetProperties().All(property =>
      {
        var prop1 = property.GetValue(obj1, null);
        var prop2 = property.GetValue(obj2, null);
        return prop1.Equals(prop2);
      });
    }
  }
}
