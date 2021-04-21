using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TurboMatterManagement.Areas.Admin.Models;
using System.ComponentModel.DataAnnotations;
using TurboMatterManagement.CustomFilters;

namespace TurboMatterManagement.Models
{
    public class Document : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        public string FileName { get; set; }
        [Required]
        [StringLength(100)]
        public string ContentType { get; set; }
        [Required]
        public byte[] Content { get; set; }
        [Required]
        public virtual Matter Matter { get; set; }
    }

    public class DocumentBasicInfo
    {   
        [Required]
        [AllowFileSize()]
        public HttpPostedFileBase FileAttach { get; set; }
    }
}