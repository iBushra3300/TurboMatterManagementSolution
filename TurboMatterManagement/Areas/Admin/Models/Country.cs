using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TurboMatterManagement.Models;

namespace TurboMatterManagement.Areas.Admin.Models
{
    public class Country : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        [Display(Name = "Country Name")]
        public string Name { get; set; }
        [Required]
        [MaxLength(2)]
        [StringLength(2, ErrorMessage = "The {0} must be only {2} characters long.", MinimumLength = 2 )]
        public string Code { get; set; }
        public virtual List<Matter> Matters { get; set; }
    }
}