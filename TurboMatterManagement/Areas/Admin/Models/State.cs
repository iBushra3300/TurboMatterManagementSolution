using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TurboMatterManagement.Models;

namespace TurboMatterManagement.Areas.Admin.Models
{
    public class State : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public virtual List<Address> Addresses { get; set; }
    }
}