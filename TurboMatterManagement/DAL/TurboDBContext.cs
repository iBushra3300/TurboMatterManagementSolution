using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using TurboMatterManagement.Areas.Admin.Models;
using TurboMatterManagement.Models;

namespace TurboMatterManagement.DAL
{
    public class TurboDBContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Matter> Matters { get; set; }
        public DbSet<MatterType> MatterTypes { get; set; }
        public DbSet<Document> Documents { get; set; }
        public TurboDBContext() : base("name=TurboDBContext") { }
    }
}