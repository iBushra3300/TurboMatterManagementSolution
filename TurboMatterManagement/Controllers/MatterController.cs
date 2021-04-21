using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using TurboMatterManagement.Models;
using TurboMatterManagement.Areas.Admin.Models;
using System.Configuration;
using PagedList;
using TurboMatterManagement.CustomFilters;

namespace TurboMatterManagement.Controllers
{
    [AuthorizeUser(Roles = "Attorney, Admin")]
    public class MatterController : BaseController
    {
        public ActionResult List(string sortColumn, string currentSort, int? page, string numberSearch, string nameSearch, string matterStatusSearch, string openDateSearch)
        {
            sortColumn = string.IsNullOrEmpty(sortColumn) ? "Number" : sortColumn;
            ViewBag.CurrentSort = sortColumn;
            int pageIndex = page.HasValue ? page.Value : 1;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
            var matters = dbContext.Matters.Select(m => m);

            if (!string.IsNullOrEmpty(numberSearch)) 
            {
                matters = matters.Where(m => m.Number.Contains(numberSearch));
                ViewBag.NumberSearch = numberSearch;
            }

            if (!string.IsNullOrEmpty(nameSearch))
            {
                matters = matters.Where(m => m.Name.ToUpper().Contains(nameSearch.ToUpper()));
                ViewBag.NameSearch = nameSearch;
            }

            if (!string.IsNullOrEmpty(matterStatusSearch))
            {
                var msSearch = (MatterStatus)int.Parse(matterStatusSearch);
                matters = matters.Where(m => m.MatterStatus == msSearch);
                ViewBag.MatterStatusSearch = matterStatusSearch;
            }

            if (!string.IsNullOrEmpty(openDateSearch))
            {
                var openDate = DateTime.Parse(openDateSearch);
                matters = matters.Where(m => m.OpenDate == openDate);
                ViewBag.OpenDateSearch = openDateSearch;
            }

            switch (sortColumn)
            {
                case "Number":
                    if (sortColumn.Equals(currentSort))
                    {
                        matters = matters.OrderByDescending(m => m.Number);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                    {
                        matters = matters.OrderBy(m => m.Number);
                    }
                    break;
                case "Name":
                    if (sortColumn.Equals(currentSort))
                    {
                        matters = matters.OrderByDescending(m => m.Name);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                    {
                        matters = matters.OrderBy(m => m.Name);
                    }
                    break;
                case "MatterStatus":
                    if (sortColumn.Equals(currentSort))
                    {
                        matters = matters.OrderByDescending(m => m.MatterStatus);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                    {
                        matters = matters.OrderBy(m => m.MatterStatus);
                    }
                    break;
                case "OpenDate":
                    if (sortColumn.Equals(currentSort))
                    {
                        matters = matters.OrderByDescending(m => m.OpenDate);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                    {
                        matters = matters.OrderBy(m => m.OpenDate);
                    }
                    break;
            }

            return View(matters.ToPagedList(pageIndex, pageSize));
        }

        public ActionResult Create()
        {
            PopulateDropDowns();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Matter matter, string btnSave, string btnSaveEdit, string btnCancel)
        {
            if (!string.IsNullOrEmpty(btnCancel))
            {
                return RedirectToAction("List");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var tmpMatter = dbContext.Matters.Where(m => m.Name.ToUpper().Equals(matter.Name.ToUpper()) && m.MatterTypeId == matter.MatterTypeId && m.CountryId == matter.CountryId).FirstOrDefault();
                    if (tmpMatter != null)
                    {
                        ModelState.AddModelError("", "Specified matter already exists");
                        PopulateDropDowns();
                        return View(matter);
                    }
                    else
                    {
                        int matterId = 1;
                        if (dbContext.Matters.Any())
                        {
                            matterId = dbContext.Matters.Select(m => m.Id).Max() + 1;
                        }
                        matter.Number = string.Format("TM{0}", matterId.ToString().PadLeft(10, '0'));
                        matter.Active = true;
                        matter.CreateDate = DateTime.Now;
                        dbContext.Matters.Add(matter);
                        dbContext.SaveChanges();
                    }
                    string actionMethodName = string.IsNullOrEmpty("btnSave") ? "Edit" : "List";
                    return RedirectToAction(actionMethodName);
                }
                PopulateDropDowns();
            }
            return View(matter);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var matter = dbContext.Matters.Where(m => m.Id == id.Value).FirstOrDefault();
            if (matter == null)
            {
                return HttpNotFound();
            }

            Session["Matter"] = matter;
            PopulateDropDowns();
            return View(matter);
        }

        [HttpPost]
        public ActionResult Edit(Matter matter, string btnSave, string btnNext, string btnCancel)
        {
            if (!string.IsNullOrEmpty(btnCancel))
            {
                return RedirectToAction("List");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var tmpMatter = dbContext.Matters.Where(m => m.Name.ToUpper().Equals(matter.Name.ToUpper()) && m.MatterTypeId == matter.MatterTypeId && m.CountryId == matter.CountryId && m.Id != matter.Id).FirstOrDefault();
                    if (tmpMatter != null)
                    {
                        ModelState.AddModelError("", "Specified matter already exists");
                        PopulateDropDowns();
                        return View(matter);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(btnSave))
                        {
                            // Find solution to remove these 2 lines
                            var local = dbContext.Set<Matter>().Local.FirstOrDefault(m => m.Id == matter.Id);
                            if (local != null) dbContext.Entry(local).State = EntityState.Detached;

                            matter.UpdateDate = DateTime.Now;
                            dbContext.Entry(matter).State = EntityState.Modified;
                            dbContext.SaveChanges();
                            return RedirectToAction("List");
                        }
                        if (!string.IsNullOrEmpty(btnNext))
                        {
                            var myMatter = MyMatter;
                            myMatter.UpdateDate = DateTime.Now;
                            myMatter.Name = matter.Name;
                            myMatter.MatterStatus = matter.MatterStatus;
                            myMatter.Description = matter.Description;
                            myMatter.MatterTypeId = matter.MatterTypeId;
                            myMatter.CountryId = matter.CountryId;
                            myMatter.OpenDate = matter.OpenDate;
                            myMatter.DispositionDate = matter.DispositionDate;
                            return RedirectToAction("MatterOrganizations");
                        }
                    }
                }
                PopulateDropDowns();
            }
            return View(matter);
        }

        public ActionResult MatterOrganizations()
        {
            var matterOrg = new MatterOrganization();
            matterOrg.Number = MyMatter.Number;
            matterOrg.Organizations = MyMatter.Organizations;
            matterOrg.AddOrganization = false;
            return View("MatterOrganizations", matterOrg);
        }

        [HttpPost]
        public ActionResult SearchOrganizations(MatterOrganization mo)
        {
            var orgs = dbContext.Organizations.Where(o => o.BillingRegion == mo.OrganizationSearchInfo.BillingRegion);
            if (!string.IsNullOrEmpty(mo.OrganizationSearchInfo.Name)) orgs = orgs.Where(o => o.Name.ToUpper().Contains(mo.OrganizationSearchInfo.Name.ToUpper()));
            if (!string.IsNullOrEmpty(mo.OrganizationSearchInfo.VendorNumber)) orgs = orgs.Where(o => o.VendorNumber.ToUpper().Contains(mo.OrganizationSearchInfo.VendorNumber.ToUpper()));
            
            var matterOrg = new MatterOrganization();
            matterOrg.Number = MyMatter.Number;
            matterOrg.AddOrganization = true;
            matterOrg.Organizations = orgs.ToList();
            return View("MatterOrganizations", matterOrg);
        }

        [HttpPost]
        public ActionResult Organizations(string btnSave, string btnNext, string btnPrevious)
        {
            if (!string.IsNullOrEmpty(btnSave))
            {
                var matter = MyMatter;
                var matterOrgs = matter.Organizations.Where(o => o.ForDelete == true).ToList();
                foreach(var org in matterOrgs)
                {
                    matter.Organizations.Remove(org);
                }

                dbContext.Entry(matter).State = EntityState.Modified;
                dbContext.SaveChanges();
                PopulateDropDowns();
                return View("Edit", matter);
            }
            else
            if (!string.IsNullOrEmpty(btnNext))
            {
                return RedirectToAction("MatterDocuments");
            }
            else
            if (!string.IsNullOrEmpty(btnPrevious))
            {
                PopulateDropDowns();
                return View("Edit", MyMatter);
            }

            return View();
        }

        public ActionResult AddMatterOrganization()
        {
            var matterOrg = new MatterOrganization();
            matterOrg.Number = MyMatter.Number;
            matterOrg.AddOrganization = true;
            return View("MatterOrganizations", matterOrg);
        }

        [ActionName("SelectThisOrganization")]
        public ActionResult AddMatterOrganization(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var org = dbContext.Organizations.Where(o => o.Id == id.Value).FirstOrDefault();
            if (org == null) return HttpNotFound();

            bool shouldAddOrg = true;
            if (MyMatter.Organizations != null && MyMatter.Organizations.Any())
            {
                var tmpOrg = MyMatter.Organizations.Where(o => o.Id == id).FirstOrDefault();
                if (tmpOrg != null) shouldAddOrg = false;
            }
            if (shouldAddOrg)
            {
                org.ForDelete = false;
                MyMatter.Organizations.Add(org);
            }
            return RedirectToAction("MatterOrganizations");
        }
        
        public ActionResult DeleteMatterOrganization(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var org = dbContext.Organizations.Where(o => o.Id == id.Value).FirstOrDefault();
            if (org == null) return HttpNotFound();

            org.ForDelete = true;
            return RedirectToAction("MatterOrganizations");
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var matter = dbContext.Matters.Where(m => m.Id == id.Value).FirstOrDefault();
            if (matter == null)
            {
                return HttpNotFound();
            }

            Session["Matter"] = matter;
            return View(matter);
        }

        public ActionResult MatterOganizationDetails()
        {
            var matterOrg = new MatterOrganization();
            matterOrg.Id = MyMatter.Id;
            matterOrg.Number = MyMatter.Number;
            matterOrg.Organizations = MyMatter.Organizations;
            matterOrg.AddOrganization = false;
            return View("MatterOrganizationDetails", matterOrg);
        }

        public ActionResult MatterDocumentsDetails()
        {
            var matterDoc = new MatterDocument();
            matterDoc.Id = MyMatter.Id;
            matterDoc.Number = MyMatter.Number;
            matterDoc.Documents = MyMatter.Documents;
            matterDoc.AddDocument = false;
            return View("MatterDocumentsDetails", matterDoc);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var matter = dbContext.Matters.Where(m => m.Id == id.Value).FirstOrDefault();
            if (matter == null)
            {
                return HttpNotFound();
            }
            return View(matter);
        }

        [HttpPost]
        [ActionName(name: "Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var matter = dbContext.Matters.Where(m => m.Id == id.Value).FirstOrDefault();
            if (matter == null)
            {
                return HttpNotFound();
            }

            dbContext.Matters.Remove(matter);
            dbContext.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult MatterDocuments()
        {
            var matterDoc = new MatterDocument();
            matterDoc.Number = MyMatter.Number;
            matterDoc.Documents = MyMatter.Documents;
            matterDoc.AddDocument = false;
            return View("MatterDocuments", matterDoc);
        }

        [HttpPost]
        public ActionResult Documents(string btnSave, string btnPrevious)
        {
            if (!string.IsNullOrEmpty(btnSave))
            {
                var matter = MyMatter;
                matter.UpdateDate = DateTime.Now;
                dbContext.Entry(matter).State = EntityState.Modified;
                dbContext.SaveChanges();
                return RedirectToAction("List");
            }
            else
            if (!string.IsNullOrEmpty(btnPrevious))
            {
                return RedirectToAction("MatterOrganizations");
            }

            return View();
        }

        public ActionResult AddMatterDocument()
        {
            var matterDoc = new MatterDocument();
            matterDoc.Number = MyMatter.Number;
            matterDoc.AddDocument = true;
            return View("MatterDocuments", matterDoc);
        }

        [HttpPost]
        public ActionResult UploadMatterDocument(DocumentBasicInfo doc)
        {
            if (ModelState.IsValid)
            {
                if (doc != null && doc.FileAttach.ContentLength > 0)
                {
                    var document = new Document();
                    document.FileName = System.IO.Path.GetFileName(doc.FileAttach.FileName);
                    document.ContentType = doc.FileAttach.ContentType;
                    document.CreateDate = DateTime.Now;
                    document.Active = true;
                    document.Matter = MyMatter;

                    using (var reader = new BinaryReader(doc.FileAttach.InputStream))
                    {
                        document.Content = reader.ReadBytes(doc.FileAttach.ContentLength);
                    }

                    dbContext.Documents.Add(document);
                    dbContext.SaveChanges();

                    return RedirectToAction("MatterDocuments");
                }
            }
            else
            {
                var matterDoc = new MatterDocument();
                matterDoc.Number = MyMatter.Number;
                matterDoc.AddDocument = true;
                return View("MatterDocuments", matterDoc);
            }

            return View();
        }

        public ActionResult DeleteMatterDocument(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var document = dbContext.Documents.Where(d => d.Id == id.Value).FirstOrDefault();
            if (document == null)
            {
                return HttpNotFound();
            }

            dbContext.Documents.Remove(document);
            dbContext.SaveChanges();

            return RedirectToAction("MatterDocuments");
        }

        public ActionResult OpenMatterDocument(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var document = dbContext.Documents.Where(d => d.Id == id.Value).FirstOrDefault();
            if (document == null)
            {
                return HttpNotFound();
            }

            Response.AppendHeader("Content-Disposition", "inline; filename=" + document.FileName);
            return new FileStreamResult(new MemoryStream(document.Content), document.ContentType);
        }

        private void PopulateDropDowns()
        {
            ViewData["MatterTypeId"] = new SelectList(dbContext.MatterTypes.Where(m => m.Active == true), "Id", "Name");
            ViewData["CountryId"] = new SelectList(dbContext.Countries.Where(m => m.Active == true).OrderBy(c => c.Name), "Id", "Name");
        }

        private Matter MyMatter
        {
            get
            {
                Matter matter = null;
                if (Session["Matter"] == null)
                {
                    matter = new Matter();
                    Session["Matter"] = matter;
                }
                else
                {
                    matter = Session["Matter"] as Matter;
                }
                return matter;
            }
        }
    }
}