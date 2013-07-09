using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace PVLog.OutputProcessing
{
    public class UserControllViewProvider 
    {
        public UserControllViewProvider()
        {
           
        }

        /// <summary>
        /// Returns the html output from any ascx user control
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public string RenderView(string viewPath, object parameters)
        {
            Page pageHolder = new Page();
            UserControl viewControl = (UserControl)pageHolder.LoadControl(viewPath);

            if (parameters != null)
            {
                Type viewControlType = viewControl.GetType();
                FieldInfo field = viewControlType.GetField("Data");

                if (field != null)
                {
                    field.SetValue(viewControl, parameters);
                }
            }

            pageHolder.Controls.Add(viewControl);

            StringWriter output = new StringWriter();
            HttpContext.Current.Server.Execute(pageHolder, output, false);

            return output.ToString();
        }
    }
}