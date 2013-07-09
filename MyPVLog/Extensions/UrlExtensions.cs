using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PVLog.Models;

namespace PVLog.Extensions
{
    public static class UrlExtensions
    {
        //public static ScriptUrl (this UrlHelper url,string path)
        //{
        //    return url.Content(path);
        //}

      public static string PlantUrl(this UrlHelper url, string text, string action, SolarPlant plant)
      {
        string name = HtmlExtensions.ResolveSubjectForUrl(plant.Name);
        return url.Action(action, new { id = plant.PlantId, name });
      }
         
    }
}