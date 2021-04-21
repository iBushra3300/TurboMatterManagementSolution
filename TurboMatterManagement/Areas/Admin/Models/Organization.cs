using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TurboMatterManagement.Models;

namespace TurboMatterManagement.Areas.Admin.Models
{
    public class Organization : BaseEntity
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [Display(Name = "Billing Region")]
        public BillingRegion BillingRegion { get; set; }
        [Required]
        [MaxLength(20)]
        [Display(Name = "Vendor Number")]
        public string VendorNumber { get; set; }
        public virtual List<Address> Addresses { get; set; }
        public virtual List<Phone> Phones { get; set; }
        [NotMapped]
        public bool IsEdit { get; set; }
        public virtual List<Matter> Matters { get; set; }
        [NotMapped]
        public bool ForDelete { get; set; }
        public Organization()
        {
            this.Addresses = new List<Address>();
            this.Phones = new List<Phone>();
        }
    }

    public enum BillingRegion
    {
        [Display(Name = "United States")]
        US, 
        [Display(Name = "Asia Pacific")]
        APAC, 
        [Display(Name = "Latin America")]
        LATAM,
        Other
    }

    public class OrganizationBasicInfo : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Display(Name = "Billing Region")]
        public BillingRegion BillingRegion { get; set; }
        [Required]
        [Display(Name = "Vendor Number")]
        public string VendorNumber { get; set; }
    }

    public class OrganizationSearchInfo
    {
        public string Name { get; set; }
        [Display(Name = "Billing Region")]
        public BillingRegion BillingRegion { get; set; }
        [Display(Name = "Vendor Number")]
        public string VendorNumber { get; set; }
    }

    public class OrganizationConactInfo 
    {
        public List<Address> Addresses { get; set; }
        public List<Phone> Phones { get; set; }
        public string AddressOperationMode { get; set; }
        public Address SelectedAddress { get; set; }
        public string PhoneOperationMode { get; set; }
        public Phone SelectedPhone { get; set; }
        public bool IsOrgBeingEdited { get; set; }
    }
}