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
using AttendenceTeamWorks;
using static GrInfra.Global;
using GrInfra.Models;

namespace AttendenceTeamWorks.Controllers
{
    [SessionExpireAttribute]
    public class TempEmployeeMastersController : Controller
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
                    Text = row.Id + " - " + row.Value
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
                    Text = row.Id
                });
            }
            return list;
        }

        public ActionResult Index(string pagesize, string search, string empid, int? page)
        {
            string Role = Session["Role"].ToString();
            string LoginId = Session["LoginId"].ToString();

            if (Role == "RL01")
            {
                ViewBag.EmployeeId = ToSelectList((from A in db.EmployeeMasters select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
            }
            else if (Role == "RL02")
            {
                ViewBag.EmployeeId = ToSelectList((from A in db.EmployeeMasters where A.HRID == LoginId select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
            }
            else
            {
                ViewBag.EmployeeId = ToSelectList((from A in db.EmployeeMasters where A.MangerID == LoginId select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
            }

            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (pagesize != null)
            {
                pageSize = Convert.ToInt32(pagesize);
            }
            if (search == null)
            {
                search = "";
            }
            if (empid == null)
            {
                empid = "";
            }

            ViewBag.pagesize = pagesize;
            ViewBag.Search = search;
            ViewBag.Employee = empid;

            List<UserCreate> tempEmployeeMaster1 = new List<UserCreate>();

            tempEmployeeMaster1 = (from A in db.EmployeeMasters
                                   join B in db.MasterPasswords on A.EmployeeId equals B.UserID
                                   select new UserCreate
                                   {
                                       EmployeeId = A.EmployeeId,
                                       EmpName = A.EmpName,
                                       Department = A.Department,
                                       Designation = A.Designation,
                                       Branch = A.Branch,
                                       ReportingManger = A.MangerID,
                                       ReportingMangerName = A.MangerName,
                                       HRID = A.HRID,
                                       HRName = A.HRName,
                                       DateOfBirth = A.DateOfBirth,
                                       DateOfJoining = A.DateOfJoining,
                                       EmpMobile = A.EmpMobile,
                                       EmpEmailId = A.EmpEmailId,
                                       Sex = A.Sex,
                                       Role = B.Role,
                                       BUCode = A.BUCode
                                   }).ToList();

            if (Role == "RL02")
            {
                tempEmployeeMaster1 = tempEmployeeMaster1.Where(x => x.HRID == LoginId).ToList();
            }
            if (Role == "RL03")
            {
                tempEmployeeMaster1 = tempEmployeeMaster1.Where(x => x.ReportingManger == LoginId).ToList();
            }

            if (search != "")
            {
                tempEmployeeMaster1 = tempEmployeeMaster1.Where(x => x.BUCode == search).ToList();
            }
            if (empid != "")
            {
                tempEmployeeMaster1 = tempEmployeeMaster1.Where(x => x.EmployeeId == empid).ToList();
            }

            return View(tempEmployeeMaster1.ToPagedList(pageNumber, pageSize));
        }
        // GET: TempEmployeeMasters/Details/5
        //public ActionResult Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EmployeeMaster EmployeeMaster = db.EmployeeMasters.Find(id);
        //    if (EmployeeMaster == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(EmployeeMaster);
        //}

        //// GET: TempEmployeeMasters/Create
        //public ActionResult Create()
        //{
        //    ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());
        //    ViewBag.ManagerIdList = ToSelectList1((from A in db.EmployeeMasters  select new DropDownModel { Id = A.ReportingManger, Value = A.ReportingManger }).Distinct().ToList());
        //    ViewBag.CostList = ToSelectList1((from A in db.EmployeeMasters select new DropDownModel { Id = A.Department, Value = A.Department }).Distinct().ToList());
        //    return View();
        //}

        //// POST: TempEmployeeMasters/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "EmployeeId,EmpName,Department,Designation,Branch,ReportingManger,DateOfJoining,DateOfBirth,EmpMobile,EmpEmailId,Sex")] UserCreate tempEmployeeMaster)
        //{
        //    ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());
        //    ViewBag.ManagerIdList = ToSelectList1((from A in db.EmployeeMasters select new DropDownModel { Id = A.ReportingManger, Value = A.ReportingManger }).Distinct().ToList());
        //    ViewBag.CostList = ToSelectList1((from A in db.EmployeeMasters select new DropDownModel { Id = A.Department, Value = A.Department }).Distinct().ToList());
        //    DateTime temp = Convert.ToDateTime(tempEmployeeMaster.DateOfJoining);
        //    SiteMaster SiteMaster = new SiteMaster();
        //    SiteMaster = db.SiteMasters.Where(x => x.BUDescription == tempEmployeeMaster.Branch).FirstOrDefault();
        //    EmployeeMaster EmployeeMaster = new EmployeeMaster();
        //    EmployeeMaster = db.EmployeeMasters.Where(x => x.EmployeeId == tempEmployeeMaster.EmployeeId).FirstOrDefault();

        //    if (EmployeeMaster != null)
        //    {
        //        if (tempEmployeeMaster.EmployeeId == EmployeeMaster.EmployeeId)
        //        {
        //            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());
        //            ViewBag.ManagerIdList = ToSelectList1((from A in db.EmployeeMasters select new DropDownModel { Id = A.ReportingManger, Value = A.ReportingManger }).Distinct().ToList());
        //            ViewBag.CostList = ToSelectList1((from A in db.EmployeeMasters select new DropDownModel { Id = A.Department, Value = A.Department }).Distinct().ToList());

        //            ViewBag.UserId = "Already Exists";
        //            return View(tempEmployeeMaster);
        //        }
        //    }


        //    if (tempEmployeeMaster.ReportingManger != null)
        //    {
        //        TempEmployeeMaster employeeMaster1 = new TempEmployeeMaster();
        //        employeeMaster1 = db.TempEmployeeMasters.Where(x => x.UserId == tempEmployeeMaster.ReportingHeadId).FirstOrDefault();
        //        tempEmployeeMaster.ReportingHeadName = employeeMaster1.MemberName;
        //    }
        //    if (tempEmployeeMaster.Branch_Code != null)
        //    {
        //        tempEmployeeMaster.Branch = branch.Location;
        //    }

        //    tempEmployeeMaster.Cl = 0;
        //    tempEmployeeMaster.SL = 0;
        //    tempEmployeeMaster.HSL = 9;
        //    tempEmployeeMaster.EL = 0;
        //    tempEmployeeMaster.RH = 2;
        //    tempEmployeeMaster.ProbationStatus = "In Probation";
        //    tempEmployeeMaster.IsActive = true;
        //    tempEmployeeMaster.ConfirmationDate = temp.AddMonths(6);
        //    tempEmployeeMaster.IsConfirmed = false;
        //    tempEmployeeMaster.Compoff = Convert.ToDecimal(0.00);
        //    tempEmployeeMaster.LOP = Convert.ToDecimal(0.00);

        //    TempEmployeeMaster tempEmployeeMaster1 = new TempEmployeeMaster();
        //    tempEmployeeMaster1 = new TempEmployeeMaster
        //    {
        //        UserId = tempEmployeeMaster.UserId,
        //        MemberName = tempEmployeeMaster.MemberName,
        //        Cost_Center = tempEmployeeMaster.Cost_Center,
        //        Designation = tempEmployeeMaster.Designation,
        //        Branch = tempEmployeeMaster.Branch,
        //        Branch_Code = tempEmployeeMaster.Branch_Code,
        //        ReportingHeadName = tempEmployeeMaster.ReportingHeadName,
        //        ReportingHeadId = tempEmployeeMaster.ReportingHeadId,
        //        DateOfBirth = tempEmployeeMaster.DateOfBirth,
        //        DateOfJoining = tempEmployeeMaster.DateOfJoining,
        //        CommAddress = tempEmployeeMaster.CommAddress,
        //        ContactNo = tempEmployeeMaster.ContactNo,
        //        CompaneyEmail = tempEmployeeMaster.CompaneyEmail,
        //        IsActive = tempEmployeeMaster.IsActive,
        //        Qualification = tempEmployeeMaster.Qualification,
        //        Gender = tempEmployeeMaster.Gender,
        //        ConfirmationDate = tempEmployeeMaster.ConfirmationDate,
        //        MaritalStatus = tempEmployeeMaster.MaritalStatus,
        //        IsConfirmed = tempEmployeeMaster.IsConfirmed,
        //        ProbationStatus = tempEmployeeMaster.ProbationStatus,
        //        Cl = tempEmployeeMaster.Cl,
        //        SL = tempEmployeeMaster.SL,
        //        HSL = tempEmployeeMaster.HSL,
        //        EL = tempEmployeeMaster.EL,
        //        lastLeaveUpdated = tempEmployeeMaster.lastLeaveUpdated,
        //        RH = tempEmployeeMaster.RH,
        //        CompanyName = tempEmployeeMaster.CompanyName,
        //        Compoff = tempEmployeeMaster.Compoff,
        //        LOP = tempEmployeeMaster.LOP
        //    };


        //    if (ModelState.IsValid)
        //    {
        //        db.TempEmployeeMasters.Add(tempEmployeeMaster1);
        //        TempEmpMasterPassword tempEmpMasterPassword = new TempEmpMasterPassword();
        //        tempEmpMasterPassword.UserID = tempEmployeeMaster1.UserId;
        //        tempEmpMasterPassword.UserTmcPassword = "password";
        //        tempEmpMasterPassword.HRRole = "-";
        //        db.TempEmpMasterPasswords.Add(tempEmpMasterPassword);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(tempEmployeeMaster);
        //}

        // GET: TempEmployeeMasters/Edit/5
        //public ActionResult Edit(string id)
        //{
        //    ViewBag.BranchList = ToSelectList1((from A in db.SiteMasters select new DropDownModel { Id = A.BUDescription.ToString(), Value = A.BUDescription }).ToList());
        //    ViewBag.ManagerIdList = ToSelectList((from A in db.EmployeeMasters select new DropDownModel { Id = A.EmployeeId, Value = A.EmpName }).Distinct().ToList());
        //    ViewBag.CostList = ToSelectList1((from A in db.EmployeeMasters select new DropDownModel { Id = A.Department, Value = A.Department }).Distinct().ToList());
        //    ViewBag.RoleList = ToSelectList1((from A in db.MasterPasswords select new DropDownModel { Id = A.Role, Value = A.Role }).Distinct().ToList());

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    EmployeeMaster EmployeeMaster = db.EmployeeMasters.Find(id);
        //    UserCreate userCreate = new UserCreate();
        //    userCreate = (from A in db.EmployeeMasters
        //                  join B in db.MasterPasswords on A.EmployeeId equals B.UserID
        //                  where A.EmployeeId == id
        //                  select new UserCreate
        //                  {
        //                      EmployeeId = A.EmployeeId,
        //                      EmpName = A.EmpName,
        //                      Department = A.Department,
        //                      Designation = A.Designation,
        //                      Branch = A.Branch,
        //                      ReportingManger = A.MangerID,
        //                      DateOfBirth = A.DateOfBirth,
        //                      DateOfJoining = A.DateOfJoining,
        //                      EmpMobile = A.EmpMobile,
        //                      EmpEmailId = A.EmpEmailId,
        //                      Sex = A.Sex,
        //                      Role = B.Role
        //                  }).FirstOrDefault();

        //    if (EmployeeMaster == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View("Edit",userCreate);
        //}

        //// POST: TempEmployeeMasters/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Branch,EmployeeId")] UserCreate tempEmployeeMaster)
        //{
        //    EmployeeMaster EmployeeMaster = db.EmployeeMasters.Find(tempEmployeeMaster.EmployeeId);
        //    EmployeeMaster.Branch = tempEmployeeMaster.Branch;

        //    if (ModelState.IsValid)
        //    {
        //        ViewBag.ErrorIsConfirmed = "";
        //        ViewBag.ErrorIsActive = "";
        //        ViewBag.ErrorReporting = "";
        //        db.Entry(EmployeeMaster).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ErrorIsConfirmed = "";
        //    ViewBag.ErrorIsActive = "";
        //    //ViewBag.BranchList = ToSelectList1((from A in db.SiteMasters select new DropDownModel { Id = A.BUDescription.ToString(), Value = A.BUDescription }).ToList());
        //    //ViewBag.ManagerIdList = ToSelectList((from A in db.EmployeeMasters select new DropDownModel { Id = A.EmployeeId, Value = A.EmpName }).Distinct().ToList());
        //    //ViewBag.CostList = ToSelectList1((from A in db.EmployeeMasters select new DropDownModel { Id = A.Department, Value = A.Department }).Distinct().ToList());
        //    return View(tempEmployeeMaster);
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
