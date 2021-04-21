using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TurboMatterManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace TurboMatterManagement.Areas.Admin.Models
{
    public class MatterType : BaseEntity
    {
        [Required]
        [MaxLength(200)]
        [Display(Name = "Matter Type")]
        public string Name { get; set; }
        public virtual List<Matter> Matters { get; set; }
    }
}