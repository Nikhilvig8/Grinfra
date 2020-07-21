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
using System.Globalization;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace AttendenceTeamWorks.Controllers
{
    [SessionExpireAttribute]
    public class MobileDeviceController : Controller
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
        public DataTable GenerateDataTable()
        {
           

           
              var  list = (from A in db.EmployeeMasters
                           join B in db.MasterPasswords on A.EmployeeId equals B.UserID
                           join e in db.MobileDetails on B.UserID equals e.Userid into ej
                           from e in ej.DefaultIfEmpty()


                           where B.AuthtokenStatus == "1" || B.AuthtokenStatus == "2"
                           select new UserCreate
                           {
                               EmployeeId = A.EmployeeId,
                               EmpName = A.EmpName,
                               Department = A.Department,
                               Designation = A.Designation,
                               Branch = A.Branch,
                               HRID = A.HRID,
                               HRName = A.HRName,
                               ReportingManger = A.MangerID,
                               ReportingMangerName = A.MangerName,
                               DateOfBirth = A.DateOfBirth,
                               DateOfJoining = A.DateOfJoining,
                               EmpMobile = A.EmpMobile,
                               EmpEmailId = A.EmpEmailId,
                               Sex = A.Sex,
                               BUCode = A.BUCode,
                               IMEI = e.IMEI == null ? "-" : e.IMEI,
                               Battery = e.Battery == null ? 0 : e.Battery,
                               LastActivity = e.LastActivity.ToString(),
                               AppVersion = e.AppVersion.ToString(),

                               LastLocation = e.LastLocation == null ? "-" : e.LastLocation,
                           }).ToList();
            string ColumnName = "EmployeeId,EmpName,Department,Designation,Branch,HRID,HRName,ReportingManger,ReportingMangerName,DateOfBirth,DateOfJoining,EmpMobile,EmpEmailId,Sex,BUCode,IMEI,Battery,LastActivity,LastLocation,AppVersion";
            string[] column = ColumnName.Split(',');
            DataTable Dt = new DataTable();
            foreach (var col in column)
            {
                Dt.Columns.Add(col, typeof(string));
            }

            try
            {
                foreach (var data in list)
                {
                    DataRow row = Dt.NewRow();
                    int i = 0;
                    foreach (var col in column)
                    {
                        if (col == "EmployeeId")
                        {
                            row[i] = data.EmployeeId;
                        }
                        if (col == "EmpName")
                        {
                            row[i] = data.EmpName;
                        }
                        if (col == "Department")
                        {
                            row[i] = data.Department;
                        }
                        if (col == "Designation")
                        {
                            row[i] = data.Designation;
                        }
                        if (col == "Branch")
                        {
                            row[i] = data.Branch;
                        }
                        
                        if (col == "HRID")
                        {
                            row[i] = data.HRID;
                        }
                        if (col == "HRName")
                        {
                            row[i] = data.HRName;
                        }
                        if (col == "ReportingManger")
                        {
                            row[i] = data.ReportingManger;
                        }
                        if (col == "ReportingMangerName")
                        {
                            row[i] = data.ReportingMangerName;
                        }
                        if (col == "DateOfBirth")
                        {
                            row[i] = data.DateOfBirth;
                        }
                        if (col == "DateOfJoining")
                        {
                            row[i] = data.DateOfJoining;
                        }
                        
                        if (col == "EmpMobile")
                        {
                            row[i] = data.EmpMobile;
                        }
                        if (col == "EmpEmailId")
                        {
                            row[i] = data.EmpEmailId;
                        }
                       
                        if (col == "Sex")
                        {
                            row[i] = data.Sex;
                        }
                        if (col == "BUCode")
                        {
                            row[i] = data.BUCode;
                        }
                        if (col == "IMEI")
                        {
                            row[i] = data.IMEI;
                        }
                        if (col == "Battery")
                        {
                            row[i] = data.Battery;
                        }
                        if (col == "LastActivity")
                        {
                            row[i] = data.LastActivity;
                        }
                        if (col == "LastLocation")
                        {
                            row[i] = data.LastLocation;
                        }
                        if (col == "AppVersion")
                        {
                            row[i] = data.AppVersion;
                        }

                        i++;
                    }
                    Dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Dt;
        }
        public ActionResult ExcelReport()
        {
            DataTable Dt = GenerateDataTable();
            var memoryStream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("General Information");
                worksheet.Cells["A1"].LoadFromDataTable(Dt, true, TableStyles.None);
                worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                worksheet.DefaultRowHeight = 18;

                Session["DownloadExcel_FileManager"] = excelPackage.GetAsByteArray();

                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Download()
        {

            if (Session["DownloadExcel_FileManager"] != null)
            {
                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                return File(data, "application/octet-stream", "MobileDeviceReport.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
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
            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role != "RL01" && Role != "RL02")
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());
            int pageSize = 10;

            int pageNumber = (page ?? 1);
            if (pagesize != null)
            {
                pageSize = Convert.ToInt32(pagesize);
            }

            ViewBag.pagesize = pagesize;
            if (search == null)
            {
                search = "";
            }
            if (empid == null)
            {
                empid = "";
            }

            ViewBag.Search = search;
            ViewBag.Employee = empid;
            List<UserCreate> tempEmployeeMaster1 = (from A in db.EmployeeMasters
                                                    join B in db.MasterPasswords on A.EmployeeId equals B.UserID
                                                    join e in db.MobileDetails on B.UserID equals e.Userid into ej
                                                    from e in ej.DefaultIfEmpty()


                                                    where B.AuthtokenStatus == "1" || B.AuthtokenStatus == "2"
                                                    select new UserCreate
                                                    {
                                                        EmployeeId = A.EmployeeId,
                                                        EmpName = A.EmpName,
                                                        Department = A.Department,
                                                        Designation = A.Designation,
                                                        Branch = A.Branch,
                                                        HRID = A.HRID,
                                                        HRName = A.HRName,
                                                        ReportingManger = A.MangerID,
                                                        ReportingMangerName = A.MangerName,
                                                        DateOfBirth = A.DateOfBirth,
                                                        DateOfJoining = A.DateOfJoining,
                                                        EmpMobile = A.EmpMobile,
                                                        EmpEmailId = A.EmpEmailId,
                                                        Sex = A.Sex,
                                                        BUCode = A.BUCode,
                                                        IMEI = e.IMEI == null ? "-" : e.IMEI,
                                                        Battery = e.Battery == null ? 0 : e.Battery,
                                                        LastActivity = e.LastActivity.ToString(),

                                                        LastLocation = e.LastLocation == null ? "-" : e.LastLocation,
                                                        AppVersion = e.AppVersion == null ? "-" : e.AppVersion,
                                                    }).ToList();

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
        public ActionResult ActiveUser(string id)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }

            GInfraEntities GInfraEntities = new GInfraEntities();
            GInfraEntities.sp_mobiledevice(id);

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
