using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TurboMatterManagement.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public bool Active { get; set; }
        [Display(Name = "Create Date")]
        public DateTime CreateDate { get; set; }
        [Display(Name = "Update Date")]
        public DateTime? UpdateDate { get; set; }
    }
}