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
    public class OrganizationController : BaseController
    { 
        public ActionResult List(string sortColumn, string currentSort, int? page, string nameSearch, string vendorNumberSearch, int? billingRegionSearch)
        {
            var billingRegionList = new Dictionary<int, string>();
            billingRegionList.Add(-1, " ");
            foreach (var item in Enum.GetValues(typeof(BillingRegion)))
            {
                billingRegionList.Add((int)item, Enum.GetName(typeof(BillingRegion), item));
            }
            ViewBag.BillingRegionList = new SelectList(billingRegionList, "Key", "Value");
            
            sortColumn = string.IsNullOrEmpty(sortColumn) ? "Name" : sortColumn;
            ViewBag.CurrentSort = sortColumn;
            var orgs = dbContext.Organizations.Select(o => o);
            if (!string.IsNullOrEmpty(nameSearch))
            {
                orgs = orgs.Where(o => o.Name.Contains(nameSearch));
                ViewBag.NameSearch = nameSearch;
            }
            if (!string.IsNullOrEmpty(vendorNumberSearch))
            {
                orgs = orgs.Where(o => o.VendorNumber.Contains(vendorNumberSearch));
                ViewBag.VendorNumberSearch = vendorNumberSearch;
            }
            if (billingRegionSearch.HasValue && billingRegionSearch.Value >= 0)
            {
                orgs = orgs.Where(o => o.BillingRegion == (BillingRegion)billingRegionSearch);
                ViewBag.BillingRegionSearch = billingRegionSearch;
            }

            int pageIndex;
            pageIndex = page.HasValue ? page.Value : 1;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);

            switch (sortColumn)
            {
                case "Name":
                    if (sortColumn.Equals(currentSort))
                    {
                        orgs = orgs.OrderByDescending(o => o.Name);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                    {
                        orgs = orgs.OrderBy(o => o.Name);
                    }
                    break;
                case "VendorNumber":
                    if (sortColumn.Equals(currentSort))
                    {
                        orgs = orgs.OrderByDescending(o => o.VendorNumber);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                    {
                        orgs = orgs.OrderBy(o => o.VendorNumber);
                    }
                    break;
                case "BillingRegion":
                    if (sortColumn.Equals(currentSort))
                    {
                        orgs = orgs.OrderByDescending(o => o.BillingRegion);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                    {
                        orgs = orgs.OrderBy(o => o.BillingRegion);
                    }
                    break;
            }
            return View(orgs.ToPagedList(pageIndex, pageSize));
        }
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(OrganizationBasicInfo bi, string btnSave, string btnNext)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(btnSave))
                {
                    var tmpOrg = dbContext.Organizations.Where(o => o.Name.Equals(bi.Name)).FirstOrDefault();
                    if (tmpOrg != null)
                    {
                        ModelState.AddModelError("", "Specified organization already exists");
                        return View();
                    }

                    var org = new Organization();
                    org.CreateDate = DateTime.Now;
                    org.Active = true;
                    org.Name = bi.Name;
                    org.VendorNumber = bi.VendorNumber;
                    org.BillingRegion = bi.BillingRegion;

                    dbContext.Organizations.Add(org);
                    dbContext.SaveChanges();

                    return RedirectToAction("List");
                }
                if (!string.IsNullOrEmpty(btnNext))
                {
                    var org = GetOrganization();
                    org.CreateDate = DateTime.Now;
                    org.Active = true;
                    org.Name = bi.Name;
                    org.VendorNumber = bi.VendorNumber;
                    org.BillingRegion = bi.BillingRegion;

                    var oci = new OrganizationConactInfo();
                    oci.AddressOperationMode = "Read";
                    oci.Addresses = org.Addresses;
                    oci.Phones = org.Phones;
                    return View("Contacts", oci);
                }
            }
            return View();
        }

        public ActionResult Edit(int? id)
        {
            if (id.Equals(null)) new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var org = dbContext.Organizations.Where(o => o.Id.Equals(id.Value)).FirstOrDefault();
            if (org == null) return HttpNotFound();
            org.IsEdit = true;
            Session["Organization"] = org;

            var tmp = new OrganizationBasicInfo();
            tmp.Name = org.Name;
            tmp.VendorNumber = org.VendorNumber;
            tmp.BillingRegion = org.BillingRegion;

            return View(tmp);
        }

        [HttpPost]
        public ActionResult Edit(OrganizationBasicInfo bi, string btnSave, string btnNext)
        {
            if (ModelState.IsValid)
            {
                var tmpOrg = dbContext.Organizations.Where(o => o.Name.Equals(bi.Name) && o.Id != bi.Id).FirstOrDefault();
                if (tmpOrg != null)
                {
                    ModelState.AddModelError("", "Specified organization already exists");
                    return View();
                }

                var org = GetOrganization();
                org.Name = bi.Name;
                org.VendorNumber = bi.VendorNumber;
                org.BillingRegion = bi.BillingRegion;

                if (!string.IsNullOrEmpty(btnSave))
                {
                    org.UpdateDate = DateTime.Now;
                    dbContext.Entry(org).State = EntityState.Modified;
                    dbContext.SaveChanges();

                    return RedirectToAction("List");
                }

                if (!string.IsNullOrEmpty(btnNext))
                {
                    var oci = new OrganizationConactInfo();
                    oci.AddressOperationMode = "Read";
                    oci.Addresses = org.Addresses;
                    oci.Phones = org.Phones;
                    oci.IsOrgBeingEdited = true;
                    return View("Contacts", oci);
                }
            }
            return View(bi);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id.Equals(null)) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var org = dbContext.Organizations.Where(o => o.Id == id.Value).FirstOrDefault();
            if (org == null) return new HttpNotFoundResult();
            return View(org);
        }

        public ActionResult Details(int? id)
        {
            if (id.Equals(null)) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var org = dbContext.Organizations.Where(o => o.Id == id.Value).FirstOrDefault();
            if (org == null) return new HttpNotFoundResult();
            return View(org);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id.Equals(null)) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var org = dbContext.Organizations.Where(o => o.Id == id.Value).FirstOrDefault();
            if (org == null) return new HttpNotFoundResult();
            dbContext.Organizations.Remove(org);
            dbContext.SaveChanges();
            return RedirectToAction("List");
        }
        
        [HttpPost]
        public ActionResult Contacts(OrganizationConactInfo ci, string btnPrevious, string btnSave)
        {
            var org = GetOrganization();
            string viewName = org.IsEdit ? "Edit" : "Create";

            if (!string.IsNullOrEmpty(btnPrevious))
            {
                var orgBasicInfo = new OrganizationBasicInfo();
                orgBasicInfo.Name = org.Name;
                orgBasicInfo.VendorNumber = org.VendorNumber;
                orgBasicInfo.BillingRegion = org.BillingRegion;
                orgBasicInfo.Id = org.Id;

                return View(viewName, orgBasicInfo);
            }
            if (!string.IsNullOrEmpty(btnSave))
            {
                if (ModelState.IsValid)
                {
                    if (org.IsEdit)
                    {
                        dbContext.Addresses.RemoveRange(org.Addresses.Where(a => a.ForDelete == true));
                        dbContext.Phones.RemoveRange(org.Phones.Where(p => p.ForDelete == true));
                        org.UpdateDate = DateTime.Now;
                        dbContext.Entry(org).State = EntityState.Modified;
                    }
                    else
                    {
                        var removeAddresses = org.Addresses.Where(a => a.ForDelete == true);
                        foreach (var address in removeAddresses) org.Addresses.Remove(address);
                        var removePhones = org.Phones.Where(p => p.ForDelete == true);
                        foreach (var phone in removePhones) org.Phones.Remove(phone);
                        dbContext.Organizations.Add(org);
                    }
                    dbContext.SaveChanges();

                    return RedirectToAction("List");
                }
                else
                    return View(viewName);
            }
            return View();
        }
        
        public ActionResult Cancel()
        {
            var org = GetOrganization();           
            var oci = new OrganizationConactInfo();
            oci.Addresses = org.Addresses;
            oci.Phones = org.Phones;
            oci.AddressOperationMode = "Read";
            oci.PhoneOperationMode = "Read";
            return View("Contacts", oci);
        }
        
        public ActionResult CreateAddress()
        {
            ViewData["States"] = dbContext.States.Where(s => s.Active).ToList();
            var oci = new OrganizationConactInfo();
            oci.AddressOperationMode = "Insert";
            oci.SelectedAddress = new Address();
            var org = GetOrganization();
            oci.Addresses = org.Addresses;
            return View("Contacts", oci);
        }
        
        [HttpPost]
        public ActionResult CreateAddress(Address address, FormCollection fc)
        {
            var org = GetOrganization();
            var stateId = int.Parse(fc["State"]);
            address.State = dbContext.States.Where(s => s.Id == stateId).FirstOrDefault();

            var tmpContact = org.Addresses?.Where(c => c.Street.ToUpper().Equals(address.Street.ToUpper()) && c.City.ToUpper().Equals(address.City.ToUpper()) && c.State.Code.Equals(address.State.Code) && c.Zip.Equals(address.Zip)).FirstOrDefault();
            if (tmpContact != null)
            {
                ModelState.AddModelError("", "Specified address already exists.");
                ViewData["States"] = dbContext.States.Where(s => s.Active).ToList();
                var ci = new OrganizationConactInfo();
                ci.AddressOperationMode = "Insert";
                ci.SelectedAddress = address;
                ci.Addresses = org.Addresses;
                return View("Contacts", ci);
            }
            else
            {
                address.CreateDate = DateTime.Now;
                address.Active = true;
                address.Id = 1;
                if (org.Addresses.Any())
                {
                    address.Id = org.Addresses.Select(c => c.Id).Max() + 1;
                }
                org.Addresses.Add(address);
            }

            var oci = new OrganizationConactInfo();
            oci.Addresses = org.Addresses;
            oci.Phones = org.Phones;
            oci.AddressOperationMode = "Read";
            return View("Contacts", oci);
        }
        
        public ActionResult DeleteAddress(int Id)
        {
            var org = GetOrganization();
            var address = org.Addresses.Where(c => c.Id == Id).FirstOrDefault();
            address.ForDelete = true;

            var oci = new OrganizationConactInfo();
            oci.Addresses = org.Addresses;
            oci.Phones = org.Phones;
            oci.AddressOperationMode = "Read";
            return View("Contacts", oci);
        }
        
        [HttpGet]
        public ActionResult EditAddress(int Id)
        {
            ViewData["States"] = dbContext.States.Where(s => s.Active).ToList();
            var org = GetOrganization();
            var oci = new OrganizationConactInfo();
            oci.AddressOperationMode = "Edit";
            var address = org.Addresses.Where(c => c.Id == Id).FirstOrDefault();
            oci.SelectedAddress = address;
            return View("Contacts", oci);
        }
        
        [HttpPost]
        public ActionResult EditAddress(Address address, FormCollection fc)
        {
            var org = GetOrganization();
            var stateId = int.Parse(fc["State"]);
            address.State = dbContext.States.Where(s => s.Id == stateId).FirstOrDefault();
            var orgAddress = org.Addresses.Where(a => a.Id == address.Id).FirstOrDefault();
            orgAddress.Street = address.Street;
            orgAddress.City = address.City;
            orgAddress.State = address.State;
            orgAddress.Zip = address.Zip;
            orgAddress.Type = address.Type;
            orgAddress.UpdateDate = DateTime.Now;

            var oci = new OrganizationConactInfo();
            oci.Addresses = org.Addresses;
            oci.Phones = org.Phones;
            oci.AddressOperationMode = "Read";
            return View("Contacts", oci);
        }

        public ActionResult CreatePhone()
        {
            var oci = new OrganizationConactInfo();
            oci.AddressOperationMode = "Read";
            oci.PhoneOperationMode = "Insert";
            oci.SelectedPhone = new Phone();
            var org = GetOrganization();
            oci.Phones = org.Phones;
            return View("Contacts", oci);
        }

        [HttpPost]
        public ActionResult CreatePhone(Phone phone)
        {
            var org = GetOrganization();

            var tmpContact = org.Phones?.Where(c => c.Number.Equals(phone.Number)).FirstOrDefault();
            if (tmpContact != null)
            {
                ModelState.AddModelError("", "Specified phone already exists.");
                var ci = new OrganizationConactInfo();
                ci.AddressOperationMode = "Read";
                ci.PhoneOperationMode = "Insert";
                ci.SelectedPhone = phone;
                ci.Phones = org.Phones;
                return View("Contacts", ci);
            }
            else
            {
                phone.CreateDate = DateTime.Now;
                phone.Active = true;
                phone.Id = 1;
                if (org.Phones.Any())
                {
                    phone.Id = org.Phones.Select(c => c.Id).Max() + 1;
                }
                org.Phones.Add(phone);
            }

            var oci = new OrganizationConactInfo();
            oci.Phones = org.Phones;
            oci.Addresses = org.Addresses;
            oci.AddressOperationMode = "Read";
            oci.PhoneOperationMode = "Read";
            return View("Contacts", oci);
        }

        [HttpGet]
        public ActionResult EditPhone(int Id)
        {
            var org = GetOrganization();
            var oci = new OrganizationConactInfo();
            oci.AddressOperationMode = "Read";
            oci.PhoneOperationMode = "Edit";
            var phone = org.Phones?.Where(c => c.Id == Id).FirstOrDefault();
            oci.SelectedPhone = phone;
            oci.Addresses = org.Addresses;
            oci.Phones = org.Phones;
            return View("Contacts", oci);
        }

        [HttpPost]
        public ActionResult EditPhone(Phone phone)
        {
            var org = GetOrganization();
            var orgPhone = org.Phones?.Where(c => c.Id == phone.Id).FirstOrDefault();
            orgPhone.Number = phone.Number;
            orgPhone.Type = phone.Type;
            orgPhone.UpdateDate = DateTime.Now;

            var oci = new OrganizationConactInfo();
            oci.Addresses = org.Addresses;
            oci.Phones = org.Phones;
            oci.AddressOperationMode = "Read";
            oci.PhoneOperationMode = "Read";
            return View("Contacts", oci);
        }

        public ActionResult DeletePhone(int Id)
        {
            var org = GetOrganization();
            var phone = org.Phones.Where(p => p.Id == Id).FirstOrDefault();
            phone.ForDelete = true;

            var oci = new OrganizationConactInfo();
            oci.Addresses = org.Addresses;
            oci.Phones = org.Phones;
            oci.AddressOperationMode = "Read";
            oci.PhoneOperationMode = "Read";
            return View("Contacts", oci);
        }

        private Organization GetOrganization()
        {
            Organization org = null;
            if (Session["Organization"] == null)
            {
                org = new Organization();
                Session["Organization"] = org;
            }
            else
            {
                org = Session["Organization"] as Organization;
            }
            return org;
        }
    }
}