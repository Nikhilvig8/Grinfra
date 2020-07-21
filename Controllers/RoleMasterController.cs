using GrInfra.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class RoleMasterController : Controller
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

            if (Role != "RL01" && Role != "RL02")
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }

            if (Role == "RL01")
            {
                ViewBag.EmployeeId = ToSelectList1((from A in db.EmployeeMasters select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
            }
            else if (Role == "RL02")
            {
                ViewBag.EmployeeId = ToSelectList1((from A in db.EmployeeMasters where A.HRID == LoginId select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
            }
            else
            {
                ViewBag.EmployeeId = ToSelectList1((from A in db.EmployeeMasters where A.MangerID == LoginId select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
            }

            List<RoleModel> list = new List<RoleModel>();
            if (Role == "RL01")
            {
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
                        }).OrderBy(x => x.isactive).ToList();
            }
            if (Role == "RL02")
            {
                list = (from A in db.MasterPasswords
                        join C in db.EmployeeMasters on A.UserID equals C.EmployeeId
                        join B in db.RoleDefines on A.Role equals B.RoleCode
                        where C.HRID == LoginId
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
                        }).OrderBy(x => x.isactive).ToList();
            }
            if (Role == "RL03")
            {
                list = (from A in db.MasterPasswords
                        join C in db.EmployeeMasters on A.UserID equals C.EmployeeId
                        join B in db.RoleDefines on A.Role equals B.RoleCode
                        where C.MangerID == LoginId
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
                        }).OrderBy(x => x.isactive).ToList();
            }

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
            bool stat = false;
            if (status == "true")
            {
                stat = true;
            }
            MasterPassword banner = db.MasterPasswords.Where(x => x.UserID == id).FirstOrDefault();
            banner.isactive = stat;

            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}