using GrInfra.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class SuperMasterController : Controller
    {
        GInfraEntities db = new GInfraEntities();
        // GET: RoleMaster
        public List<SelectListItem> ToSelectList(List<DropDownModel> lis)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<string> dublicate = new List<string>();
            foreach (var row in lis)
            {
                list.Add(new SelectListItem()
                {
                    Value = row.Id,
                    Text = row.Id
                });
            }
            return list;
        }

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

        public ActionResult Index(string pagesize, string emp, int? page)
        {
            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role != "RL01")
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            List<RoleModel> list = new List<RoleModel>();
            list = (from A in db.MasterPasswords
                    join B in db.RoleDefines on A.Role equals B.RoleCode
                    join C in db.EmployeeMasters on A.UserID equals C.EmployeeId
                    select new RoleModel
                    {
                        Role = A.Role,
                        RoleName = B.RoleName,
                        UserId = A.UserID,
                        isactive = A.isactive,
                        Super = A.Super,
                        EmployeeName = C.EmpName,
                        ManagerId = C.MangerID,
                        ManagerName = C.MangerName,
                        HRId = C.HRID,
                        HRName = C.HRName,
                        Department = C.Department,
                        Designation = C.Designation,
                        BUCode = C.BUCode,
                        Branch = C.Branch
                    }).OrderByDescending(x => x.Super).ToList();
            ViewBag.EmployeeId = ToSelectList1((from A in db.EmployeeMasters select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (pagesize != null)
            {
                pageSize = Convert.ToInt32(pagesize);
            }
            if (emp == null)
            {
                emp = "";
            }
            ViewBag.Employee = emp;
            if (emp != "")
            {
                list = list.Where(x => x.UserId == emp).ToList();
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ChangeStatus(string id, string status)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            bool stat = false;
            if (status == "true")
            {
                stat = true;
            }
            MasterPassword banner = db.MasterPasswords.Where(x => x.UserID == id).FirstOrDefault();
            banner.Super = stat;

            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}