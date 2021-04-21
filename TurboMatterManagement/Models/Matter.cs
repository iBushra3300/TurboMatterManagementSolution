using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using TurboMatterManagement.Areas.Admin.Models;
using TurboMatterManagement.CustomFilters;
using System.ComponentModel.DataAnnotations.Schema;

namespace TurboMatterManagement.Models
{
    public class Matter : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        [Display(Name = "Matter Number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Matter Name is required")]
        [MaxLength(200)]
        [Display(Name = "Matter Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Matter Description is required")]
        [MaxLength(6000)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Matter Status")]
        public MatterStatus MatterStatus { get; set; }

        public virtual Country Country { get; set; }

        [Required(ErrorMessage = "Country is required")]
        public int CountryId { get; set; }

        [Display(Name = "Matter Type")]
        public virtual MatterType MatterType { get; set; }

        [Required(ErrorMessage = "Matter Type is required")]
        public int MatterTypeId { get; set; }

        [Required(ErrorMessage = "Open Date is required")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        [DatePastOrPresent(ErrorMessage = "Open Date cannot be a future date")]
        [Display(Name = "Open Date")]
        public DateTime OpenDate { get; set; }

        [Display(Name = "Disposition Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy}")]
        public DateTime? DispositionDate { get; set; }
        public virtual List<Organization> Organizations { get; set; }
        public virtual List<Document> Documents { get; set; }
    }

    public enum MatterStatus
    {
        Open,
        Closed
    }

    public class MatterOrganization
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public List<Organization> Organizations { get; set; }
        public bool AddOrganization { get; set; }
        public OrganizationSearchInfo OrganizationSearchInfo { get; set; }
    }

    public class MatterDocument
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public List<Document> Documents { get; set; }
        public bool AddDocument { get; set; }
        public Document Document { get; set; }
    }
}