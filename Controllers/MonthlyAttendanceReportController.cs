using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;
using static GrInfra.Global;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using GrInfra.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class MonthlyAttendanceReportController : Controller
    {
        private GInfraEntities db = new GInfraEntities();
        // GET: AttendanceReport
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

        // GET: AttendanceReport
        public ActionResult Index(int? page, string branch, string emp, string head, string hr)
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
            if (hr == null)
            {
                hr = "";
            }
            ViewBag.Branch = branch;
            ViewBag.Employee = emp;
            ViewBag.Head = head;
            ViewBag.HR = hr;

            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role == "RL01")
            {
                ViewBag.EmployeeList = ToSelectList((from A in db.EmployeeMasters select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
                ViewBag.HeadList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.MangerID equals B.EmployeeId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
                ViewBag.HRList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.HRID equals B.EmployeeId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
            }
            else if (Role == "RL02")
            {
                ViewBag.EmployeeList = ToSelectList((from A in db.EmployeeMasters where A.HRID == LoginId select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
                ViewBag.HeadList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.MangerID equals B.EmployeeId where A.HRID == LoginId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
                ViewBag.HRList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.HRID equals B.EmployeeId where A.HRID == LoginId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
            }
            else
            {
                ViewBag.EmployeeList = ToSelectList((from A in db.EmployeeMasters where A.MangerID == LoginId select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
                ViewBag.HeadList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.MangerID equals B.EmployeeId where A.MangerID == LoginId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
                ViewBag.HRList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.HRID equals B.EmployeeId where A.MangerID == LoginId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
            }
            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            List<Sp_MonthlyAttendanceReports_Result> list = db.Sp_MonthlyAttendanceReports(Role, LoginId, emp, head, hr, branch).ToList();

            return View(list.ToPagedList(pageNumber, pageSize));
        }

        public DataTable GenerateDataTable(string branch, string emp, string head, string hr)
        {
            int month = DateTime.Now.Month;
            int year = DateTime.Now.Year;
            var date = year + "-" + month + "-" + 1;
            DateTime day = DateTime.Parse(date);
            string weekday = day.DayOfWeek.ToString();


            DataTable Dt = new DataTable();
            Dt.Columns.Add("Employee Id", typeof(string));
            Dt.Columns.Add("Employee Name", typeof(string));
            Dt.Columns.Add("Manager Id", typeof(string));
            Dt.Columns.Add("Manager Name", typeof(string));
            Dt.Columns.Add("HR Id", typeof(string));
            Dt.Columns.Add("HR Name", typeof(string));
            Dt.Columns.Add("Department", typeof(string));
            Dt.Columns.Add("Designation", typeof(string));
            Dt.Columns.Add("Branch", typeof(string));
            Dt.Columns.Add("Email ID", typeof(string));
            Dt.Columns.Add("Mobile No", typeof(string));
            Dt.Columns.Add("Date Of Joining", typeof(DateTime));

            Dt.Columns.Add("Absent", typeof(int));

            Dt.Columns.Add("Present", typeof(int));
            Dt.Columns.Add("Week Off", typeof(int));
            Dt.Columns.Add("TDP", typeof(decimal));
            for (int i = 1; i <= 31; i++)
            {
                string name = i + " - " + weekday;
                if (i == 1)
                {
                    Dt.Columns.Add(name, typeof(string));
                }
                else
                {
                    day = day.AddDays(1);
                    weekday = day.DayOfWeek.ToString();
                    name = i + " - " + weekday;
                    Dt.Columns.Add(name, typeof(string));
                }
            }
            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            List<Sp_MonthlyAttendanceReports_Result> FileData = new List<Sp_MonthlyAttendanceReports_Result>();
            FileData = db.Sp_MonthlyAttendanceReports(Role, LoginId, emp, head, hr, branch).ToList();

            try
            {
                foreach (var data in FileData)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = data.EmployeeId;
                    row[1] = data.EmpName;
                    row[2] = data.MangerID;
                    row[3] = data.MangerName;
                    row[4] = data.HRID;
                    row[5] = data.HRName;
                    row[6] = data.Department;
                    row[7] = data.Designation;
                    row[8] = data.Branch;
                    row[9] = data.EmpEmailId;
                    row[10] = data.EmpMobile;
                    row[11] = data.DateOfJoining;
                    row[12] = data.Absent;
                    row[13] = data.Present;
                    row[14] = data.WO;
                    row[15] = data.TDP;
                    row[16] = data.C1;
                    row[17] = data.C2;
                    row[18] = data.C3;
                    row[19] = data.C4;
                    row[20] = data.C5;
                    row[21] = data.C6;
                    row[22] = data.C7;
                    row[23] = data.C8;
                    row[24] = data.C9;
                    row[25] = data.C10;
                    row[26] = data.C11;
                    row[27] = data.C12;
                    row[28] = data.C13;
                    row[29] = data.C14;
                    row[30] = data.C15;
                    row[31] = data.C16;
                    row[32] = data.C17;
                    row[33] = data.C18;
                    row[34] = data.C19;
                    row[35] = data.C20;
                    row[36] = data.C21;
                    row[37] = data.C22;
                    row[38] = data.C23;
                    row[39] = data.C24;
                    row[40] = data.C25;
                    row[41] = data.C26;
                    row[42] = data.C27;
                    row[43] = data.C28;
                    row[44] = data.C29;
                    row[45] = data.C30;
                    row[46] = data.C31;
                    Dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return Dt;
        }

        public ActionResult ExcelReport(string branch, string emp, string head, string hr)
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
            if (hr == null)
            {
                hr = "";
            }

            DataTable Dt = GenerateDataTable(emp, head, hr, branch);
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

        public ActionResult ExportPDF(string branch, string emp, string head, string hr)
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
            if (hr == null)
            {
                hr = "";
            }

            DataTable Dt = GenerateDataTable(emp, head, hr, branch);

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