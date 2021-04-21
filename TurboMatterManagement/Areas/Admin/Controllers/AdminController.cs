using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TurboMatterManagement.Models;
using TurboMatterManagement.CustomFilters;

namespace TurboMatterManagement.Areas.Admin.Controllers
{
    [AuthorizeUser(Roles = "Admin")]
    public class AdminController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}