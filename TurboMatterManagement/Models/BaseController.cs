using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using TurboMatterManagement.DAL;

namespace TurboMatterManagement.Models
{
    [Authorize]
    public class BaseController : Controller
    {
        private static TurboDBContext _dbContext;
        public static TurboDBContext dbContext
        {
            get
            {
                if (_dbContext == null)
                {
                    _dbContext = new TurboDBContext();
                }
                return _dbContext;
            }
        }
    }
}