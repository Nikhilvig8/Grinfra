using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using GrInfra.Models;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class SiteMasterController : Controller
    {
        private GInfraEntities db = new GInfraEntities();

        public List<SelectListItem> ToSelectList1(List<DropDownModel> lis)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<string> dublicate = new List<string>();
            foreach (var row in lis)
            {
                list.Add(new SelectListItem()
                {
                    Value = row.Id,
                    Text = row.Id + " - " + row.Value
                });
            }
            return list;
        }
        // GET: SiteMaster
        public ActionResult Index(string pagesize, string branch, int? page)
        {
            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role != "RL01" && Role != "RL02")
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            List<SiteMaster> list = db.SiteMasters.ToList();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (pagesize != null)
            {
                pageSize = Convert.ToInt32(pagesize);
            }
            ViewBag.BranchList = ToSelectList1((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());

            if (branch == null)
            {
                branch = "";
            }
            if (branch != "")
            {
                list = list.Where(x => x.BUCode == branch).ToList();
            }
            ViewBag.Branch = branch;
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        //// GET: SiteMaster/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SiteMaster siteMaster = db.SiteMasters.Find(id);
        //    if (siteMaster == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(siteMaster);
        //}

        //// GET: SiteMaster/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: SiteMaster/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,BUCode,BUDescription,WorkingHour,GracePeriod")] SiteMaster siteMaster)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.SiteMasters.Add(siteMaster);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(siteMaster);
        //}

        // GET: SiteMaster/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteMaster siteMaster = db.SiteMasters.Find(id);
            if (siteMaster == null)
            {
                return HttpNotFound();
            }
            return View(siteMaster);
        }

        // POST: SiteMaster/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,BUCode,BUDescription,Workinghours,GracePeriod")] SiteMaster siteMaster)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            SiteMaster site = db.SiteMasters.Find(siteMaster.Id);

            site.Workinghours = siteMaster.Workinghours;
            site.GracePeriod = siteMaster.GracePeriod;

            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(siteMaster);
        }

        //// GET: SiteMaster/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    SiteMaster siteMaster = db.SiteMasters.Find(id);
        //    if (siteMaster == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(siteMaster);
        //}

        //// POST: SiteMaster/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    SiteMaster siteMaster = db.SiteMasters.Find(id);
        //    db.SiteMasters.Remove(siteMaster);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
