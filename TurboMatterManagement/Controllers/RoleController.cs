using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using TurboMatterManagement.Models;

namespace TurboMatterManagement.Controllers
{
    public class RoleController : Controller
    {
        ApplicationDbContext dbContext;

        public RoleController()
        {
            dbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var roles = dbContext.Roles.ToList();
            return View(roles);
        }

        public ActionResult Create()
        {
            var role = new IdentityRole();
            return View(role);
        }

        [HttpPost]
        public ActionResult Create(IdentityRole role)
        {
            dbContext.Roles.Add(role);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}