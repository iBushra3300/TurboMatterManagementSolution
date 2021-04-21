using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TurboMatterManagement.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurboMatterManagement.Areas.Admin.Models
{
    public class Address : BaseEntity
    {
        [Required(ErrorMessage = "Street is required.")]
        [MaxLength(200)]
        public string Street { get; set; }
        [Required]
        [MaxLength(100)]
        public string City { get; set; }
        [Required]
        public virtual State State { get; set; }
        [Required]
        [MaxLength(10)]
        [StringLength(10, MinimumLength = 5)]
        [Display(Name = "Zip Code")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Zip Code is invalid.")]
        public string Zip { get; set; }
        [Display(Name = "Address Type")]
        public AddressType Type { get; set; }
        public virtual Organization Organization { get; set; }
        [Required]
        public int OrganizationId { get; set; }
        [NotMapped]
        public bool ForDelete { get; set; }
    }

    public enum AddressType
    {
        Physical,
        Billing,
        Shipping
    }
}