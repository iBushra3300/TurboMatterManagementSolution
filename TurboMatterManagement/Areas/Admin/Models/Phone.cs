using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TurboMatterManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurboMatterManagement.Areas.Admin.Models
{
    public class Phone : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Phone number")]
        public string Number { get; set; }
        [Display(Name = "Phone Type")]
        public PhoneType Type { get; set; }
        public virtual Organization Organization { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        [NotMapped]
        public bool ForDelete { get; set; }
    }

    public enum PhoneType
    {
        Cell,
        Office,
        Home
    }
}