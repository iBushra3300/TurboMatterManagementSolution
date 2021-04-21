using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;
using TurboMatterManagement.Common;

namespace TurboMatterManagement.CustomFilters
{
    public class AllowFileSizeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool result = false;
            HttpPostedFileBase upload = value as HttpPostedFileBase;
            if (upload != null)
            {
                result = upload.ContentLength <= this.FileSizeInBytes;
            }
            return result;
        }

        public AllowFileSizeAttribute() : base(Helper.FileSizeMessage) { }
        private int FileSizeInBytes
        {
            get
            {
                int fileSize = int.Parse(WebConfigurationManager.AppSettings["FileUploadSizeInMB"]);
                if (fileSize > 0)
                {
                    fileSize = fileSize * 1024 * 1024;
                }

                return fileSize;
            }
        }
    }
}