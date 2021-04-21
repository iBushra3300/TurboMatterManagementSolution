using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TurboMatterManagement.Utility
{
    public static class MyHTLMHelp
    {
        public static string YesNoString(this HtmlHelper htmlHelper, bool yesNo)
        {
            var text = yesNo ? "Yes" : "No";
            return text;
        }
    }
}