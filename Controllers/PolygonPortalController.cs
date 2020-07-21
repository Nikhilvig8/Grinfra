using GrInfra.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class PolygonPortalController : Controller
    {
        GInfraEntities db = new GInfraEntities();
        // GET: PolygonPortal

        public List<SelectListItem> ToSelectList(List<DropDownModel> lis)
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

        public ActionResult List(string branch)
        {
            if (branch == null)
            {
                branch = "";
            }

            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());

            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role != "RL01" && Role != "RL02")
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }

            List<SiteMaster> list = new List<SiteMaster>();
            if (branch != "")
            {
                list = db.SiteMasters.Where(x => x.BUCode == branch).ToList();
            }
            else
            {
                list = db.SiteMasters.ToList();
            }
            return View(list);
        }

        public ActionResult SiteWisePolygon(int id)
        {
            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role != "RL01" && Role != "RL02")
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            ViewBag.Id = id;
            var  list = db.SiteMasters.Where(x => x.Id == id).ToList();

            foreach(var item in list)
            {
                ViewBag.lat = item.lattitude;
                ViewBag.log = item.longitute;
            }
            

            return View();
        }

        public List<SelectListItem> ToSelectList(List<SiteMaster> lis)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<string> dublicate = new List<string>();
            foreach (var row in lis)
            {
                list.Add(new SelectListItem()
                {
                    Value = row.Id.ToString(),
                    Text = row.BUCode + " - " + row.BUDescription
                });
            }
            return list;
        }

        public JsonResult SavePolygon(string polygonJSON, int SiteName,string officelat,string officelong)
        {
            ObjectParameter resultvalue = new ObjectParameter("ERROR", typeof(int));
            SiteMaster updatedCustomer = (from c in db.SiteMasters
                                          where c.Id == SiteName
                                          select c).FirstOrDefault();
            updatedCustomer.lattitude = officelat;
            updatedCustomer.longitute = officelong;




            int insertedRecords = db.SaveChanges();
            db.Sp_InsertPolygon(polygonJSON, Convert.ToInt32(SiteName), resultvalue);

            

            return Json(JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPolygon(string SiteName)
        {
            int SiteId = Convert.ToInt32(SiteName);
            List<Polygon> list = db.Polygons.Where(x => x.Siteid == SiteId).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
    }
}