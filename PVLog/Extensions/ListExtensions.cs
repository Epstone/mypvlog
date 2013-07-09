using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace PVLog.Extensions
{
    public static class ListExtensions
    {
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer oSerializer = new JavaScriptSerializer();
            string json = oSerializer.Serialize(obj);
            return json;
        }
    }
}