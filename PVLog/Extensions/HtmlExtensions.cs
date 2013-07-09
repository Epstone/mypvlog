using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using PVLog.Models;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Threading;
using System.Globalization;

namespace PVLog.Extensions
{
  public static class HtmlExtensions
  {
    public static string JavascriptImport(this HtmlHelper html, string path)
    {
      var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

      string newPath = urlHelper.Content(path);

      return string.Format(@"<script src=""{0}"" type=""text/javascript""></script>", newPath);
    }

    public static string CssImport(this HtmlHelper html, string path)
    {
      var urlHelper = new UrlHelper(html.ViewContext.RequestContext);

      string newPath = urlHelper.Content(path);

      return string.Format(@"<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", newPath);
    }

    public static MvcHtmlString PlantLink(this HtmlHelper html, string text, string action, SolarPlant plant)
    {
      string name = html.ResolveSubjectForUrl(plant.Name);
      return html.ActionLink(text, action, new { id = plant.PlantId, name });
    }

    public static string ResolveSubjectForUrl(this HtmlHelper source, string subject)
    {
      return HtmlExtensions.ResolveSubjectForUrl(subject);
    }
    public static string ResolveSubjectForUrl(string subject)
    {
      return Regex.Replace(Regex.Replace(subject, "[^\\w]", "-"), "[-]{2,}", "-");
    }

    public static MvcHtmlString Title(this HtmlHelper helper, string title)
    {
      return MvcHtmlString.Create("myPVLog > " + title);
    }

    public static string GetKaco1SampleLink(this HtmlHelper helper)
    {
      //build sample route data
      var routeData = new
      {
        data = "26.12.2009;23:53:00;5;158.0;3.20;134;229.6;1.34;0;0",
        inverter = 1,
        plant = 1,
        pw = "12345"
      };

      //generate url
      TagBuilder tag = GenerateAbsoluteExampleUrl("Kaco1", routeData, helper.GetUrlHelper());

      return HttpUtility.UrlDecode(tag.ToString());
    }

    public static string GetKaco2SampleLink(this HtmlHelper helper)
    {
      //build sample route data
      var routeData = new
      {
        data = "*020;4;378.2;3.96;1498;228.9;6.55;1438;29;5000",
        plant = 1,
        pw = "12345"
      };

      //generate url
      TagBuilder tag = GenerateAbsoluteExampleUrl("Kaco2", routeData, helper.GetUrlHelper());

      return HttpUtility.UrlDecode(tag.ToString());
    }


    public static string GetGenericSampleLink(this HtmlHelper html)
    {

      /* http://localhost:60311/Log/Generic?generatoramperage=1.5&generatorvoltage=220.1&generatorwattage=4200&gridamperage=1.9&gridvoltage=240
       * &feedinwattage=4000&plant=1&inverter=1&pw=12345&systemstatus=5&temperature=0&timestamp=1328527027
       */

      var routeData = new
      {
        generatorcurrent = 1.5,
        generatorvoltage = 220.1,
        generatorpower = 4200,
        gridcurrent = 1.9,
        gridvoltage = 240,
        feedinpower = 4000,
        plant = 1,
        inverter = 1,
        pw = 12345,
        systemstatus = 5,
        temperature = 0,
        timestamp = 1328527027
      };

      // store current culture info and switch to invariant culture
      var culture = Thread.CurrentThread.CurrentCulture;
      Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

      var tag = GenerateAbsoluteExampleUrl("Generic", routeData, html.GetUrlHelper());

      //go back to previous culture
      Thread.CurrentThread.CurrentCulture = culture;

      return HttpUtility.UrlDecode(tag.ToString());
    }

    public static UrlHelper GetUrlHelper(this HtmlHelper html)
    {
      return new UrlHelper(html.ViewContext.RequestContext);
    }

    public static Uri GetBaseUrl(this UrlHelper url)
    {
      Uri contextUri = new Uri(url.RequestContext.HttpContext.Request.Url, url.RequestContext.HttpContext.Request.RawUrl);
      UriBuilder realmUri = new UriBuilder(contextUri) { Path = url.RequestContext.HttpContext.Request.ApplicationPath, Query = null, Fragment = null };
      return realmUri.Uri;
    }

    private static TagBuilder GenerateAbsoluteExampleUrl(string action, object routeData, UrlHelper url)
    {
      string address = url.ActionAbsolute(action, "Log", routeData);
      TagBuilder tagBuilder = new TagBuilder("a");

      //generate a link tag
      tagBuilder.Attributes.Add("href", address);
      tagBuilder.SetInnerText(address);

      return tagBuilder;
    }

    public static string ActionAbsolute(this UrlHelper url, string actionName, string controllerName, object routeData)
    {
      return new Uri(GetBaseUrl(url), url.Action(actionName, controllerName, routeData)).AbsoluteUri;
    }

    public static string BackLink(this HtmlHelper html)
    {
      return "<a href='javascript: history.go(-1)'>Back</a>";
    }

    public static IHtmlString AdminLinkListItem(this HtmlHelper html)
    {
      IHtmlString result = new HtmlString("");

      if (Roles.IsUserInRole("Admin"))
        result = new HtmlString("<li class='red'>" + html.ActionLink("Admin", "Index", "Administration") + "</li>");

      return result;
    }

    public static IHtmlString AddPlantLinkListItem(this HtmlHelper html)
    {
      IHtmlString result = new HtmlString("");

      if (html.IsAuthenticated())
        result = new HtmlString("<li class='green'>" + html.ActionLink("Neue PV Anlage", "Add", "Plant") + "</li>");

      return result;
    }

    private static bool IsAuthenticated(this HtmlHelper html)
    {
      return html.ViewContext.HttpContext.Request.IsAuthenticated;
    }

    public static IHtmlString LogoffLinkListItem(this HtmlHelper html)
    {
      IHtmlString result = new HtmlString("");

      if (html.IsAuthenticated())
        result = new HtmlString("<li>" + html.ActionLink("Logout", "LogOff", "Account", null, new { @class = "win8box red" }) + "</li>");

      return result;
    }

    public static string ToKwpString(this HtmlHelper helper, int kwp)
    {
      return (kwp / 1000).ToString("#.##") + " kWp";
    }
  }
}