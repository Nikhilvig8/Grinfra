using System;
using System.Collections.Generic;
using System.Data;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GrInfra.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using PagedList;
using PagedList.Mvc;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class AttendanceReportController : Controller
    {
        private GInfraEntities db = new GInfraEntities();
        public static double DistanceT(string lat1,string long2,string blat1,string blong1)
        {
            try
            {


                var sCoord = new GeoCoordinate(Convert.ToDouble(lat1), Convert.ToDouble(long2));
                var eCoord = new GeoCoordinate(Convert.ToDouble(blat1), Convert.ToDouble(blong1));
                return sCoord.GetDistanceTo(eCoord);
            }
            catch
            {
                return 0;
            }
            //var sCoord = new GeoCoordinate(28.650768, 77.134531);
            //var eCoord = new GeoCoordinate(28.650768, 77.134531);

            
        }
        //public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'M')
        //{
        //    double rlat1 = Math.PI * lat1 / 180;
        //    double rlat2 = Math.PI * lat2 / 180;
        //    double theta = lon1 - lon2;
        //    double rtheta = Math.PI * theta / 180;
        //    double dist =
        //        Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
        //        Math.Cos(rlat2) * Math.Cos(rtheta);
        //    dist = Math.Acos(dist);
        //    dist = dist * 180 / Math.PI;
        //    dist = dist * 60 * 1.1515;

        //    switch (unit)
        //    {
        //        case 'K': //Kilometers -> default
        //            return dist * 1.609344;
        //        case 'N': //Nautical Miles 
        //            return dist * 0.8684;
        //        case 'M': //Miles
        //            return dist;
        //    }

        //    return dist;
        //}
        //public double GetDistanceBetweenPoints(string lat11, string long11, double lat2, double long2)
        //{
        //    double lat1 = Convert.ToDouble(lat11);
        //    double long1 = Convert.ToDouble(long11);
        //    double distance = 0;

        //    double dLat = (lat2 - lat1) / 180 * Math.PI;
        //    double dLong = (long2 - long1) / 180 * Math.PI;

        //    double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
        //                + Math.Cos(lat2) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);
        //    double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        //    //Calculate radius of earth
        //    // For this you can assume any of the two points.
        //    double radiusE = 6378135; // Equatorial radius, in metres
        //    double radiusP = 6356750; // Polar Radius

        //    //Numerator part of function
        //    double nr = Math.Pow(radiusE * radiusP * Math.Cos(lat1 / 180 * Math.PI), 2);
        //    //Denominator part of the function
        //    double dr = Math.Pow(radiusE * Math.Cos(lat1 / 180 * Math.PI), 2)
        //                    + Math.Pow(radiusP * Math.Sin(lat1 / 180 * Math.PI), 2);
        //    double radius = Math.Sqrt(nr / dr);

        //    //Calaculate distance in metres.
        //    distance = radius * c;
        //    return distance;
        //}

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

        public ActionResult Report(string pagesize, string attendancestatus, string branch, string emp, string head, string fromdate, string todate, int? page)
        {
            
            if (branch == null)
            {
                branch = "";
            }
            if (emp == null)
            {
                emp = "";
            }
            if (head == null)
            {
                head = "";
            }
            if (fromdate == null)
            {
                fromdate = "";
            }
            if (todate == null)
            {
                todate = "";
            }
            if (attendancestatus == null)
            {
                attendancestatus = "";
            }

            ViewBag.Branch = branch;
            ViewBag.Employee = emp;
            ViewBag.Head = head;
            ViewBag.FromDate = fromdate;
            ViewBag.ToDate = todate;
            ViewBag.attendancestatus = attendancestatus;

            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role == "RL01")
            {
                ViewBag.EmployeeList = ToSelectList((from A in db.EmployeeMasters select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
                ViewBag.HeadList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.MangerID equals B.EmployeeId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
            }
            else if (Role == "RL02")
            {
                ViewBag.EmployeeList = ToSelectList((from A in db.EmployeeMasters where A.HRID == LoginId select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
                ViewBag.HeadList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.MangerID equals B.EmployeeId where A.HRID == LoginId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
            }
            else
            {
                ViewBag.EmployeeList = ToSelectList((from A in db.EmployeeMasters where A.MangerID == LoginId select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
                ViewBag.HeadList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.MangerID equals B.EmployeeId where A.MangerID == LoginId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
            }

            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());
            ViewBag.AttendanceStatusList = ToSelectList1((from A in db.attendancestatusmasters select new DropDownModel { Id = A.Status, Value = A.Status }).ToList());

            List<AttendanceReport> list = new List<AttendanceReport>();

            if (fromdate != "" && todate != "")
            {
                DateTime fromd = Convert.ToDateTime(fromdate);
                DateTime tod = Convert.ToDateTime(todate);
                list = (from a in db.AttendanceLogsNewForMobiles
                        join b in db.EmployeeMasters on a.EmployeeId equals b.EmployeeId
                        join c in db.SiteMasters on b.Branch equals c.BUDescription
                        where a.AttendanceDate >= fromd && a.AttendanceDate <= tod
                        select new AttendanceReport
                        {
                            AttendanceLogId = a.AttendanceLogId,
                            AttendanceDate = a.AttendanceDate,
                            EmployeeId = a.EmployeeId,
                            EmployeeName = b.EmpName,
                            InTime = a.InTime,
                            OutTime = a.OutTime,
                            Duration = a.Duration,
                            ManagerId = a.HeadId,
                            ManagerName = b.MangerName,
                            HRId = b.HRID,
                            HRName = b.HRName,
                            Department = b.Department,
                            Designation = b.Designation,
                            BUCode = c.BUCode,
                            AddressIn = a.AddressIn,
                            AddressOut = a.AddressOut,
                            AttendanceStatus = a.AttendanceStatus,
                            Branch = b.Branch,
                            Inimage = a.InImage,
                            lat1 = a.LattitudeIn,
                            long1 = a.LongitudeIn,
                            lat2 = a.LattitudeOut,
                            long2 = a.LongitudeOut,
                            blat1 = db.Polygons.Where(a => a.Siteid == c.Id).Take(1).Select(b => b.Lattitude).DefaultIfEmpty("0.00").FirstOrDefault().ToString(),
                            blong1 = db.Polygons.Where(a => a.Siteid == c.Id).Take(1).Select(b => b.Longitude).DefaultIfEmpty("0.00").FirstOrDefault().ToString(),
                            distancepunchin = "",
                            distancepunchout = "",
                            Officelat = c.lattitude,
                            officelong = c.longitute,

                            // distancepunchin= GetDistanceBetweenPoints("28.650768", "77.134531", 28.650768, 77.134531),
                            //Inimage = a.InImage== null ? "-" : a.InImage,

                            //BioMetricInTime = a.BioMetricIn,
                            //BioMetricOutTime = a.BioMetricOut
                        }).OrderByDescending(x => x.AttendanceDate).ToList();
                foreach (var mc in list)
                {
                    if (mc.InTime != null)
                    {
                        mc.distancepunchin = string.Format("{0:0.00}", DistanceT(mc.lat1, mc.long1, mc.Officelat, mc.officelong));
                        //mc.distancepunchin = string.Format("{0:0.00}", DistanceT(mc.lat1, mc.long1, mc.blat1, mc.blong1));
                    }
                    if (mc.OutTime != null)
                    {
                        mc.distancepunchout = string.Format("{0:0.00}", DistanceT(mc.lat2, mc.long2, mc.Officelat, mc.officelong));
                        // mc.distancepunchout = string.Format("{0:0.00}", DistanceT(mc.lat2, mc.long2, mc.blat1, mc.blong1));
                    }

                    if (mc.Officelat != null && mc.officelong != null)
                    {
                        var list1 = db.sp_polygon(mc.BUCode).ToList();
                        decimal value1 = 0;
                        foreach(var k in list1)
                        {
                            mc.bufferdistance = string.Format("{0:0.00}", DistanceT(mc.Officelat, mc.officelong, k.lattitude, k.longitude));
                            if (Convert.ToDecimal(mc.bufferdistance) > value1)
                            {
                                value1 = Convert.ToDecimal(mc.bufferdistance);
                            }
                           
                        }
                        mc.bufferdistance = value1.ToString();

                        //  mc.bufferdistance = string.Format("{0:0.00}", DistanceT(mc.Officelat, mc.officelong, mc.blat1, mc.blong1));

                    }

                }
            }
            else
            {
             
                list = (from a in db.AttendanceLogsNewForMobiles
                        join b in db.EmployeeMasters on a.EmployeeId equals b.EmployeeId
                        join c in db.SiteMasters on b.Branch equals c.BUDescription
                        where a.AttendanceDate == DateTime.Today
                        select new AttendanceReport
                        {
                            AttendanceLogId = a.AttendanceLogId,
                            AttendanceDate = a.AttendanceDate,
                            EmployeeId = a.EmployeeId,
                            EmployeeName = b.EmpName,
                            InTime = a.InTime,
                            OutTime = a.OutTime,
                            Duration = a.Duration,
                            ManagerId = a.HeadId,
                            ManagerName = b.MangerName,
                            HRId = b.HRID,
                            HRName = b.HRName,
                            Department = b.Department,
                            Designation = b.Designation,
                            BUCode = c.BUCode,
                            AddressIn = a.AddressIn,
                            AddressOut = a.AddressOut,
                            AttendanceStatus = a.AttendanceStatus,
                            Branch = b.Branch,
                            Inimage = a.InImage,
                            lat1 = a.LattitudeIn,
                            long1 = a.LongitudeIn,
                            lat2 = a.LattitudeOut,
                            long2 = a.LongitudeOut,
                            blat1 = db.Polygons.Where(a => a.Siteid == c.Id).Take(1).Select(b => b.Lattitude).DefaultIfEmpty("0.00").FirstOrDefault().ToString(),
                            blong1 = db.Polygons.Where(a => a.Siteid == c.Id).Take(1).Select(b => b.Longitude).DefaultIfEmpty("0.00").FirstOrDefault().ToString(),
                            distancepunchin = "",
                            distancepunchout = "",
                            Officelat = c.lattitude,
                            officelong = c.longitute,
                            //BioMetricInTime = a.BioMetricIn,
                            //BioMetricOutTime = a.BioMetricOut
                        }).OrderByDescending(x => x.AttendanceDate).ToList();



                foreach (var mc in list)
                {
                    if (mc.InTime != null)
                    {
                        mc.distancepunchin = string.Format("{0:0.00}", DistanceT(mc.lat1, mc.long1, mc.Officelat, mc.officelong));
                        //mc.distancepunchin = string.Format("{0:0.00}", DistanceT(mc.lat1, mc.long1, mc.blat1, mc.blong1));
                    }
                    if (mc.OutTime != null)
                    {
                        //mc.distancepunchout = string.Format("{0:0.00}",DistanceT(mc.lat2, mc.long2, mc.blat1, mc.blong1));
                        mc.distancepunchout = string.Format("{0:0.00}", DistanceT(mc.lat2, mc.long2, mc.Officelat, mc.officelong));
                    }
                    if (mc.Officelat != null && mc.officelong != null)
                    {

                        var list1 = db.sp_polygon(mc.BUCode).ToList();
                        decimal value1 = 0;
                        foreach (var k in list1)
                        {
                            mc.bufferdistance = string.Format("{0:0.00}", DistanceT(mc.Officelat, mc.officelong, k.lattitude, k.longitude));
                            if (Convert.ToDecimal(mc.bufferdistance) > value1)
                            {
                                value1 = Convert.ToDecimal(mc.bufferdistance);
                            }

                        }
                        mc.bufferdistance = value1.ToString();

                        //mc.bufferdistance = string.Format("{0:0.00}", DistanceT( mc.blat1, mc.blong1,mc.Officelat, mc.officelong));
                    }
                }




            }


            if (Role == "RL02")
            {
                list = list.Where(x => x.HRId == LoginId).ToList();
            }
            if (Role == "RL03")
            {
                list = list.Where(x => x.ManagerId == LoginId).ToList();
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (pagesize != null)
            {
                pageSize = Convert.ToInt32(pagesize);
            }
            ViewBag.pagesize = pagesize;
            if (branch != "")
            {
                list = list.Where(x => x.BUCode == branch).ToList();
            }
            if (attendancestatus != "")
            {
                list = list.Where(x => x.AttendanceStatus == attendancestatus).ToList();
            }
            if (emp != "")
            {
                list = list.Where(x => x.EmployeeId == emp).ToList();
            }
            if (head != "")
            {
                list = list.Where(x => x.ManagerId == head).ToList();
            }

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public DataTable GenerateDataTable(string attendancestatus, string branch, string emp, string head, string fromdate, string todate, string ColumnName)
        {
            if (branch == null)
            {
                branch = "";
            }
            if (emp == null)
            {
                emp = "";
            }
            if (head == null)
            {
                head = "";
            }
            if (fromdate == null)
            {
                fromdate = "";
            }
            if (todate == null)
            {
                todate = "";
            }
            if (attendancestatus == null)
            {
                attendancestatus = "";
            }

            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();
            List<AttendanceReport> list = new List<AttendanceReport>();

            if (fromdate != "" && todate != "")
            {
                DateTime fromd = Convert.ToDateTime(fromdate);
                DateTime tod = Convert.ToDateTime(todate);
                list = (from a in db.AttendanceLogsNewForMobiles
                        join b in db.EmployeeMasters on a.EmployeeId equals b.EmployeeId
                        join c in db.SiteMasters on b.Branch equals c.BUDescription
                        where a.AttendanceDate >= fromd && a.AttendanceDate <= tod
                        select new AttendanceReport
                        {
                            AttendanceLogId = a.AttendanceLogId,
                            AttendanceDate = a.AttendanceDate,
                            EmployeeId = a.EmployeeId,
                            EmployeeName = b.EmpName,
                            InTime = a.InTime,
                            OutTime = a.OutTime,
                            Duration = a.Duration,
                            ManagerId = a.HeadId,
                            ManagerName = b.MangerName,
                            HRId = b.HRID,
                            HRName = b.HRName,
                            Department = b.Department,
                            Designation = b.Designation,
                            BUCode = c.BUCode,
                            AddressIn = a.AddressIn,
                            AddressOut = a.AddressOut,
                            AttendanceStatus = a.AttendanceStatus,
                            Branch = b.BUCode,
                            //BioMetricInTime = a.BioMetricIn,
                            //BioMetricOutTime = a.BioMetricOut
                        }).OrderByDescending(x => x.AttendanceDate).ToList();
            }
            else
            {
                list = (from a in db.AttendanceLogsNewForMobiles
                        join b in db.EmployeeMasters on a.EmployeeId equals b.EmployeeId
                        join c in db.SiteMasters on b.Branch equals c.BUDescription
                        where a.AttendanceDate == DateTime.Today
                        select new AttendanceReport
                        {
                            AttendanceLogId = a.AttendanceLogId,
                            AttendanceDate = a.AttendanceDate,
                            EmployeeId = a.EmployeeId,
                            EmployeeName = b.EmpName,
                            InTime = a.InTime,
                            OutTime = a.OutTime,
                            Duration = a.Duration,
                            ManagerId = a.HeadId,
                            ManagerName = b.MangerName,
                            HRId = b.HRID,
                            HRName = b.HRName,
                            Department = b.Department,
                            Designation = b.Designation,
                            BUCode = c.BUCode,
                            AddressIn = a.AddressIn,
                            AddressOut = a.AddressOut,
                            AttendanceStatus = a.AttendanceStatus,
                            Branch = b.BUCode,
                            //BioMetricInTime = a.BioMetricIn,
                            //BioMetricOutTime = a.BioMetricOut
                        }).OrderByDescending(x => x.AttendanceDate).ToList();
            }


            if (Role == "RL02")
            {
                list = list.Where(x => x.HRId == LoginId).ToList();
            }
            if (Role == "RL03")
            {
                list = list.Where(x => x.ManagerId == LoginId).ToList();
            }
            string[] column = ColumnName.Split(',');

            if (branch != "")
            {
                list = list.Where(x => x.Branch == branch).ToList();
            }
            if (attendancestatus != "")
            {
                list = list.Where(x => x.AttendanceStatus == attendancestatus).ToList();
            }
            if (emp != "")
            {
                list = list.Where(x => x.EmployeeId == emp).ToList();
            }
            if (head != "")
            {
                list = list.Where(x => x.ManagerId == head).ToList();
            }
            if (fromdate != "" && todate != "")
            {
                DateTime fromd = Convert.ToDateTime(fromdate);
                DateTime tod = Convert.ToDateTime(todate);
                list = list.Where(x => x.AttendanceDate >= fromd && x.AttendanceDate <= tod).ToList();
            }

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
                        if (col == "AttendanceDate")
                        {
                            row[i] = data.AttendanceDate.ToString("yyyy-MM-dd");
                        }
                        if (col == "EmployeeId")
                        {
                            row[i] = data.EmployeeId;
                        }
                        if (col == "EmployeeName")
                        {
                            row[i] = data.EmployeeName;
                        }
                        if (col == "ManagerId")
                        {
                            row[i] = data.ManagerId;
                        }
                        if (col == "ManagerName")
                        {
                            row[i] = data.ManagerName;
                        }
                        if (col == "HRId")
                        {
                            row[i] = data.HRId;
                        }
                        if (col == "HRName")
                        {
                            row[i] = data.HRName;
                        }
                        if (col == "Department")
                        {
                            row[i] = data.Department;
                        }
                        if (col == "Designation")
                        {
                            row[i] = data.Designation;
                        }
                        if (col == "BranchCode")
                        {
                            row[i] = data.BUCode;
                        }
                        if (col == "BranchDescription")
                        {
                            row[i] = data.Branch;
                        }
                        if (col == "InTime")
                        {
                            //TimeSpan InTime;
                            //if (data.InTime != null)
                            //{
                            //    InTime = Convert.ToDateTime(data.InTime).TimeOfDay;
                            //    TimeSpan BioMetricInTime = Convert.ToDateTime(data.BioMetricInTime).TimeOfDay;
                            //    if (InTime > BioMetricInTime)
                            //    {
                            //        row[i] = BioMetricInTime.ToString();
                            //    }
                            //    else
                            //    {
                            //        row[i] = InTime.ToString();

                            //    }
                            //}
                            //else
                            //{
                            //    TimeSpan BioMetricInTime = Convert.ToDateTime(data.BioMetricInTime).TimeOfDay;
                            //    row[i] = BioMetricInTime.ToString();
                            //}
                            DateTime date = Convert.ToDateTime(data.InTime);
                            row[i] = date.Hour.ToString() + ":" + date.Minute.ToString() + ":" + date.Second.ToString();

                        }
                        if (col == "OutTime")
                        {
                            //TimeSpan OutTime = Convert.ToDateTime(data.OutTime).TimeOfDay;
                            //TimeSpan BioMetricOutTime = Convert.ToDateTime(data.BioMetricOutTime).TimeOfDay;
                            //if (OutTime < BioMetricOutTime)
                            //{
                            //    row[i] = BioMetricOutTime.ToString();
                            //}
                            //else
                            //{
                            //    row[i] = OutTime.ToString();

                            //}
                            DateTime date = Convert.ToDateTime(data.OutTime);
                            row[i] = date.Hour.ToString() + ":" + date.Minute.ToString() + ":" + date.Second.ToString();
                        }
                        if (col == "Duration")
                        {
                            row[i] = data.Duration;
                        }
                        if (col == "AddressIn")
                        {
                            row[i] = data.AddressIn;
                        }
                        if (col == "AddressOut")
                        {
                            row[i] = data.AddressOut;
                        }
                        if (col == "AttendanceStatus")
                        {
                            row[i] = data.AttendanceStatus;
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

        public ActionResult ExcelReport(string attendancestatus, string branch, string emp, string head, string fromdate, string todate, string ColumnName)
        {
            DataTable Dt = GenerateDataTable(attendancestatus, branch, emp, head, fromdate, todate, ColumnName);
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
                return File(data, "application/octet-stream", "Report.xlsx");
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult DownloadCsv()
        {

            if (Session["DownloadExcel_FileManager"] != null)
            {
                byte[] data = Session["DownloadExcel_FileManager"] as byte[];
                return File(data, "application/octet-stream", "Report.csv");
            }
            else
            {
                return new EmptyResult();
            }
        }

        public ActionResult ExportPDF(string attendancestatus, string branch, string emp, string head, string fromdate, string todate, string ColumnName)
        {
            DataTable Dt = GenerateDataTable(attendancestatus, branch, emp, head, fromdate, todate, ColumnName);

            byte[] filecontent = exportpdf(Dt);
            Session["DownloadPdf_FileManager"] = filecontent;
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadPdf()
        {

            if (Session["DownloadPdf_FileManager"] != null)
            {
                byte[] data = Session["DownloadPdf_FileManager"] as byte[];
                return File(data, "application/octet-stream", "Report.pdf");
            }
            else
            {
                return new EmptyResult();
            }
        }

        private byte[] exportpdf(DataTable dtEmployee)
        {
            int totalColumns = dtEmployee.Columns.Count;
            // creating document object  
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            iTextSharp.text.Rectangle rec = new iTextSharp.text.Rectangle(PageSize.A4);
            rec.BackgroundColor = new BaseColor(System.Drawing.Color.Olive);
            Document doc = new Document(rec);
            if (totalColumns <= 3)
            {
                doc.SetPageSize(iTextSharp.text.PageSize.A4);
            }
            else if (totalColumns <= 5 && totalColumns > 3)
            {
                doc.SetPageSize(iTextSharp.text.PageSize.A2);
            }
            else
            {
                doc.SetPageSize(iTextSharp.text.PageSize.A0);
            }
            PdfWriter writer = PdfWriter.GetInstance(doc, ms);
            doc.Open();

            //Creating paragraph for header  
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fntHead = new iTextSharp.text.Font(bfntHead, 16, 1, iTextSharp.text.BaseColor.BLUE);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_LEFT;
            prgHeading.Add(new Chunk("Attendance Report".ToUpper(), fntHead));
            doc.Add(prgHeading);

            //Adding paragraph for report generated by  
            Paragraph prgGeneratedBY = new Paragraph();
            BaseFont btnAuthor = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            iTextSharp.text.Font fntAuthor = new iTextSharp.text.Font(btnAuthor, 8, 2, iTextSharp.text.BaseColor.BLUE);
            prgGeneratedBY.Alignment = Element.ALIGN_RIGHT;
            //prgGeneratedBY.Add(new Chunk("Report Generated by : ASPArticles", fntAuthor));  
            //prgGeneratedBY.Add(new Chunk("\nGenerated Date : " + DateTime.Now.ToShortDateString(), fntAuthor));  
            doc.Add(prgGeneratedBY);

            //Adding a line  
            Paragraph p = new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.0F, 100.0F, iTextSharp.text.BaseColor.BLACK, Element.ALIGN_LEFT, 1)));
            doc.Add(p);

            //Adding line break  
            doc.Add(new Chunk("\n", fntHead));

            //Adding  PdfPTable  
            PdfPTable table = new PdfPTable(dtEmployee.Columns.Count);

            for (int i = 0; i < dtEmployee.Columns.Count; i++)
            {
                string cellText = Server.HtmlDecode(dtEmployee.Columns[i].ColumnName);
                PdfPCell cell = new PdfPCell();
                cell.Phrase = new Phrase(cellText, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 10, 1, new BaseColor(System.Drawing.ColorTranslator.FromHtml("#000000"))));
                cell.BackgroundColor = new BaseColor(System.Drawing.ColorTranslator.FromHtml("#C8C8C8"));
                //cell.Phrase = new Phrase(cellText, new Font(Font.FontFamily.TIMES_ROMAN, 10, 1, new BaseColor(grdStudent.HeaderStyle.ForeColor)));  
                //cell.BackgroundColor = new BaseColor(grdStudent.HeaderStyle.BackColor);  
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.PaddingBottom = 5;
                table.AddCell(cell);
            }

            //writing table Data  
            for (int i = 0; i < dtEmployee.Rows.Count; i++)
            {
                for (int j = 0; j < dtEmployee.Columns.Count; j++)
                {
                    table.AddCell(dtEmployee.Rows[i][j].ToString());
                }
            }

            doc.Add(table);
            doc.Close();

            byte[] result = ms.ToArray();
            return result;

        }
    }
}