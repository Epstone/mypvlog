using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PVLog.Models
{
    public class ErrorPresentation
    {
        public string ErrorMessage { get; set; }

        public Exception TheException { get; set; }

        public bool ShowMessage { get; set; }
    }
}