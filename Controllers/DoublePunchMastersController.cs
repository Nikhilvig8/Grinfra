using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GrInfra.Models;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class DoublePunchMastersController : Controller
    {
        private GInfraEntities db = new GInfraEntities();
        public List<SelectListItem> ToSelectList(List<DropDownModel> lis)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<string> dublicate = new List<string>();
            foreach (var row in lis)
            {
                list.Add(new SelectListItem()
                {
                    Value = row.Id,
                    Text = row.Value
                });
            }
            return list;
        }


        // GET: DoublePunchMasters
        public ActionResult Index()
        {
            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUCode + " - " + A.BUDescription }).ToList());

            List<DoubleSite> list = (from A in db.DoublePunchMasters
                                     join B in db.SiteMasters on A.BUCode equals B.BUCode
                                     select new DoubleSite
                                     {
                                         Id = A.Id,
                                         BUCode = A.BUCode,
                                         BUDescription = B.BUDescription
                                     }).ToList();

            return View(list);
        }

        [HttpPost]
        public ActionResult Index(string branch)
        {
            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode, Value = A.BUCode + " - " + A.BUDescription }).ToList());

            List<DoubleSite> list = (from A in db.DoublePunchMasters
                                     join B in db.SiteMasters on A.BUCode equals B.BUCode
                                     select new DoubleSite
                                     {
                                         Id = A.Id,
                                         BUCode = A.BUCode,
                                         BUDescription = B.BUDescription
                                     }).ToList();

            DoublePunchMaster site = db.DoublePunchMasters.Where(x => x.BUCode == branch).FirstOrDefault();
            if (site == null)
            {
                DoublePunchMaster site1 = new DoublePunchMaster();
                site1.BUCode = branch;
                site1.DoublePunchMechanism = "Double";
                db.DoublePunchMasters.Add(site1);
                db.SaveChanges();
                ViewBag.Error = "";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = "You Have Already Added this Site";
                return View(list);
            }

        }

        public ActionResult Delete(int id)
        {
            DoublePunchMaster doublePunchMaster = db.DoublePunchMasters.Find(id);
            db.DoublePunchMasters.Remove(doublePunchMaster);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

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
