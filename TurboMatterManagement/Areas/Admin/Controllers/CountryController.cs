using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TurboMatterManagement.Models;
using TurboMatterManagement.Areas.Admin.Models;
using System.Data.Entity;
using System.Configuration;
using PagedList;

namespace TurboMatterManagement.Areas.Admin.Controllers
{
    public class CountryController : BaseController
    {
        public ActionResult List(string sortColumn, string currentSort, string nameSearch, int? page)
        {
            sortColumn = string.IsNullOrEmpty(sortColumn) ? "Name" : sortColumn;
            ViewBag.CurrentSort = sortColumn;
            var countries = dbContext.Countries.Select(c => c);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                countries = countries.Where(c => c.Name.ToUpper().Contains(nameSearch.ToUpper()));
                ViewBag.NameSearch = nameSearch;
            }

            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
            int pageIndex = 1;
            pageIndex = page.HasValue ? page.Value : 1;

            switch(sortColumn)
            {
                case "Name":
                    if (sortColumn.Equals(currentSort))
                    {
                        countries = countries.OrderByDescending(c => c.Name);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                        countries = countries.OrderBy(c => c.Name);
                    break;
                case "Code":
                    if (sortColumn.Equals(currentSort))
                    {
                        countries = countries.OrderByDescending(c => c.Code);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                        countries = countries.OrderBy(c => c.Code);
                    break;
                case "CreateDate":
                    if (sortColumn.Equals(currentSort))
                    {
                        countries = countries.OrderByDescending(c => c.CreateDate);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                        countries = countries.OrderBy(c => c.CreateDate);
                    break;
            }
            return View(countries.ToPagedList(pageIndex, pageSize));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Country country)
        {
            if (ModelState.IsValid)
            {
                var tmpCountry = dbContext.Countries.Where(c => c.Name.ToUpper() == country.Name.ToUpper() && c.Code.ToUpper() == country.Code.ToUpper()).FirstOrDefault();
                if (tmpCountry != null)
                {
                    ModelState.AddModelError("", "Specifid country / code already exists.");
                    return View();
                }
                else
                {
                    country.CreateDate = DateTime.Now;
                    country.Active = true;
                    dbContext.Countries.Add(country);
                    dbContext.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var country = dbContext.Countries.Where(c => c.Id == id).FirstOrDefault();
            return View(country);
        }

        [HttpPost]
        public ActionResult Edit(Country country)
        {
            if (ModelState.IsValid)
            {
                var tmpCountry = dbContext.Countries.Where(c => (c.Name.ToUpper() == country.Name.ToUpper() || c.Code.ToUpper() == country.Code.ToUpper()) && c.Id != country.Id).FirstOrDefault();
                if (tmpCountry != null)
                {
                    ModelState.AddModelError("", "Specifid country / code already exists.");
                    return View();
                }
                else
                {
                    country.UpdateDate = DateTime.Now;
                    dbContext.Entry(country).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return RedirectToAction("List");
                }
            }
            return View(country);
        }

        public ActionResult Details(int id)
        {
            var country = dbContext.Countries.Where(c => c.Id == id).FirstOrDefault();
            return View(country);
        }

        public ActionResult Delete(int id)
        {
            var country = dbContext.Countries.Where(c => c.Id == id).FirstOrDefault();
            dbContext.Countries.Remove(country);
            dbContext.SaveChanges();
            return RedirectToAction("List");
        }
    }
}