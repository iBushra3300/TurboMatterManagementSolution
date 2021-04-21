using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace TurboMatterManagement.Common
{
    public class Helper
    {
        public static string FileSizeMessage
        {
            get
            {
               return string.Format("File size should not be greater than {0} MB", ConfigurationManager.AppSettings["FileUploadSizeInMB"]);
            }
        }
    }
}