using Hangfire;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using static GrInfra.Global;

namespace GrInfra.Models
{
    [SessionExpireAttribute]
    public class ApproveRejectAttendanceController : Controller
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
        public List<SelectListItem> ToSelectList2(List<DropDownModel> lis)
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
        public JsonResult ApproveRejectAll(string[] attendanceId, string Status, string Reason)
        {
            foreach (var id in attendanceId)
            {
                string LoginID = Session["LoginId"].ToString();
                int attendanceIdd = Convert.ToInt32(id);
                GInfraEntities GInfraEntities = new GInfraEntities();
                sp_ApproveorRejectAttendance_Result sp_ApproveorRejectAttendance_Result = new sp_ApproveorRejectAttendance_Result();
                if (Status == "Approved")
                {
                    sp_ApproveorRejectAttendance_Result = GInfraEntities.sp_ApproveorRejectAttendance(id, Reason, Status, LoginID).FirstOrDefault();
                    AttendanceLogsNewForMobile attendanceLogsNewForMobile = new AttendanceLogsNewForMobile();
                    attendanceLogsNewForMobile = GInfraEntities.AttendanceLogsNewForMobiles.Where(x => x.AttendanceLogId == attendanceIdd).FirstOrDefault();
                    EmployeeMaster employeeMaster = GInfraEntities.EmployeeMasters.Where(x => x.EmployeeId == attendanceLogsNewForMobile.EmployeeId).FirstOrDefault();
                    string userEmailSubject = "" + employeeMaster.EmpName + "(" + attendanceLogsNewForMobile.EmployeeId + ") - Attendance Approved";
                    string userEmailBody = "Dear <b>" + employeeMaster.EmpName + "(" + attendanceLogsNewForMobile.EmployeeId + "),</b><br/><br/>Your Attendance for dated i.e " + attendanceLogsNewForMobile.AttendanceDate.ToString("dd-MM-yyyy") + " has been Approved by the HR.<br/><br/><br/><br/>Wish You a Great Day Ahead!<br/><br/>Regards,<br/>Human Capital";
                    //SendMail("Nikhil.vig@teamcomputers.com", userEmailSubject, userEmailBody);
                    //SendMail(employeeMaster.EmpEmailId, userEmailSubject, userEmailBody);
                    BackgroundJob.Enqueue(() => SendMail("harvir.rathee@teamcomputers.com", userEmailSubject, userEmailBody));

                    MasterPassword master = GInfraEntities.MasterPasswords.Where(x => x.UserID == employeeMaster.EmployeeId).FirstOrDefault();
                    if (master.DeviceToken != "-")
                    {
                        BackgroundJob.Enqueue(() => SendNotificationApprove(master.DeviceToken, attendanceLogsNewForMobile.AttendanceDate, master.UserID));
                    }

                    //BackgroundJob.Enqueue(() => SendMail(employeeMaster.EmpEmailId, userEmailSubject, userEmailBody));
                }
                else
                {
                    Reason = Reason.Replace(',', '$');
                    sp_ApproveorRejectAttendance_Result = GInfraEntities.sp_ApproveorRejectAttendance(id, Reason, Status, LoginID).FirstOrDefault();
                    AttendanceLogsNewForMobile attendanceLogsNewForMobile = new AttendanceLogsNewForMobile();
                    attendanceLogsNewForMobile = GInfraEntities.AttendanceLogsNewForMobiles.Where(x => x.AttendanceLogId == attendanceIdd).FirstOrDefault();
                    EmployeeMaster employeeMaster = GInfraEntities.EmployeeMasters.Where(x => x.EmployeeId == attendanceLogsNewForMobile.EmployeeId).FirstOrDefault();
                    string userEmailSubject = "" + employeeMaster.EmpName + "(" + attendanceLogsNewForMobile.EmployeeId + ") - Attendance Rejected";
                    string userEmailBody = "Dear <b>" + employeeMaster.EmpName + "(" + attendanceLogsNewForMobile.EmployeeId + "),</b><br/><br/>Your Attendance for dated i.e " + attendanceLogsNewForMobile.AttendanceDate.ToString("dd-MM-yyyy") + " has been Rejected by the HR.<br/><br/><br/><br/>Wish You a Great Day Ahead!<br/><br/>Regards,<br/>Human Capital";
                    //SendMail("Nikhil.vig@teamcomputers.com", userEmailSubject, userEmailBody);
                    //SendMail(employeeMaster.EmpEmailId, userEmailSubject, userEmailBody);
                    BackgroundJob.Enqueue(() => SendMail("harvir.rathee@teamcomputers.com", userEmailSubject, userEmailBody));
                    //BackgroundJob.Enqueue(() => SendMail(employeeMaster.EmpEmailId, userEmailSubject, userEmailBody));

                    MasterPassword master = GInfraEntities.MasterPasswords.Where(x => x.UserID == employeeMaster.EmployeeId).FirstOrDefault();
                    if (master.DeviceToken != "-")
                    {
                        BackgroundJob.Enqueue(() => SendNotificationRejected(master.DeviceToken, attendanceLogsNewForMobile.AttendanceDate,master.UserID));
                    }
                }
            }
            return Json(JsonRequestBehavior.AllowGet);
        }

        public void SendNotificationApprove(string DeviceToken, DateTime AttendanceDate,string userid)
        {
            string serverKey = "AAAASLs8D7Y:APA91bFYt4IFQRR4NPLNhX0SKzd_VQjxrvTE1mlS1rYk648fhrW3KvcejHCjwpjh9rHlLkSth7ewFN7ogmJ2mo7znvQXG1nXC0ny5cP14lz4rOM1FUOlGi-ZEdnkPW3kZxk9F15wTsPa";
            try
            {
                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Approved Attendance by Manager" + "\",\"Description\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", AttendanceDate) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", AttendanceDate) + "\"}]}}";
                    //string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"HrAnnoucement\",\"json\":{\"Title\":\"Welcome to a new way of working!\",\"body\":\"Welcome to a new way of working!We at t:eam will be adopting Workplace by Facebook for all our internal communications. A lot like Facebook, but built for the world of work.Activate your profile today at teamcomputers.facebook.com For further details, refer to the mail sent..\",\"date\":\"9/09/2019 5:15:47 PM\",\"image_url\":\"http://111.93.123.102:8087/BannerData/1582019782Banner.png\"}}}";
                    //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"" + WithoutImage.MessageTitle + "\",\"body\":\"" + WithoutImage.Description + "\",\"date\":\"" + dateTime.ToString("dd/MM/yyyy") + "\",\"image_url\":\"" + WthImage.filepath + "\"}}}";
                    // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"body\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"date\":\"23/10/2019 12:00:00 PM\",\"image_url\":\"http://teamworksnew.teamcomputers.com:5556/Images/Final-Day-Diwali-Creative.gif\"}}}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



            }
            catch (Exception ex)
            {

                var msg = new HttpResponseMessage(HttpStatusCode.NotImplemented) { ReasonPhrase = "Error While Notification" };

            }


            try
            {

                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"to\": \"" + DeviceToken + "\",\"content_available\": true,\"notification\": { \"title\": \"AttendanceAction\",\"body\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(AttendanceDate)) + " " + " \",\"click_action\": \"fcm.ACTION.HELLO\"},\"data\": { \"extra\": \"juice\"}}";




                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



            }
            catch (Exception ex)
            {

                var msg = new HttpResponseMessage(HttpStatusCode.NotImplemented) { ReasonPhrase = "Error While Notification" };

            }
            string datee = "Approved Attendance Dated : " + AttendanceDate.ToString("dd-MM-yyyy");
            db.sp_notification(userid, "AttendanceAction", "Approved Attendance by Manager", datee);
        }

        public void SendNotificationRejected(string DeviceToken, DateTime AttendanceDate, string userid)
        {
            string serverKey = "AAAASLs8D7Y:APA91bFYt4IFQRR4NPLNhX0SKzd_VQjxrvTE1mlS1rYk648fhrW3KvcejHCjwpjh9rHlLkSth7ewFN7ogmJ2mo7znvQXG1nXC0ny5cP14lz4rOM1FUOlGi-ZEdnkPW3kZxk9F15wTsPa";
            try
            {
                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Reject Attendance by Manager" + "\",\"Description\":\" " + "Rejected Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", AttendanceDate) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", AttendanceDate) + "\"}]}}";
                    // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"Welcome to a new way of working!\",\"body\":\"Welcome to a new way of working!We at t:eam will be adopting Workplace by Facebook for all our internal communications. A lot like Facebook, but built for the world of work.Activate your profile today at teamcomputers.facebook.com For further details, refer to the mail sent..\",\"date\":\"9/09/2019 5:15:47 PM\",\"image_url\":\"https://teamcomputers.facebook.com/\"}}}";
                    //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"" + WithoutImage.MessageTitle + "\",\"body\":\"" + WithoutImage.Description + "\",\"date\":\"" + dateTime.ToString("dd/MM/yyyy") + "\",\"image_url\":\"" + WthImage.filepath + "\"}}}";
                    // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"body\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"date\":\"23/10/2019 12:00:00 PM\",\"image_url\":\"http://teamworksnew.teamcomputers.com:5556/Images/Final-Day-Diwali-Creative.gif\"}}}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



            }
            catch (Exception ex)
            {

                var msg = new HttpResponseMessage(HttpStatusCode.NotImplemented) { ReasonPhrase = "Error While Notification" };

            }


            try
            {

                var result = "-1";
                var webAddr = "https://fcm.googleapis.com/fcm/send";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"to\": \"" + DeviceToken + "\",\"content_available\": true,\"notification\": { \"title\": \"AttendanceAction\",\"body\":\" " + "Rejected Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(AttendanceDate)) + " " + " \",\"click_action\": \"fcm.ACTION.HELLO\"},\"data\": { \"extra\": \"juice\"}}";

                    // string json = "{\"to\": \"" + DeviceToken + "\",\"notification\": {\"type\": \"HrAnnoucement\",\"json\":{\"Title\":\"Welcome to a new way of working!\",\"body\":\"Welcome to a new way of working!We at t:eam will be adopting Workplace by Facebook for all our internal communications. A lot like Facebook, but built for the world of work.Activate your profile today at teamcomputers.facebook.com For further details, refer to the mail sent..\",\"date\":\"9/09/2019 5:15:47 PM\",\"image_url\":\"http://111.93.123.102:8087/BannerData/1582019782Banner.png\"}}}";
                    //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"" + WithoutImage.MessageTitle + "\",\"body\":\"" + WithoutImage.Description + "\",\"date\":\"" + dateTime.ToString("dd/MM/yyyy") + "\",\"image_url\":\"" + WthImage.filepath + "\"}}}";
                    // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"body\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"date\":\"23/10/2019 12:00:00 PM\",\"image_url\":\"http://teamworksnew.teamcomputers.com:5556/Images/Final-Day-Diwali-Creative.gif\"}}}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



            }
            catch (Exception ex)
            {

                var msg = new HttpResponseMessage(HttpStatusCode.NotImplemented) { ReasonPhrase = "Error While Notification" };

            }
            string datee = "Rejected Attendance Dated : " + AttendanceDate.ToString("dd-MM-yyyy");
            db.sp_notification(datee, "AttendanceAction", "Reject Attendance by Manager", datee);
        }
        // GET: ApproveRejectAttendance
        public ActionResult Attendance(string pagesize, string branch, string emp, string head, string fromdate, string todate, int? page)
        {
            ViewBag.RejectList = ToSelectList2((from A in db.ReasonMasters where A.isactive == true select new DropDownModel { Id = A.id.ToString(), Value = A.Reasons }).ToList());

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

            string LoginID = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();
            if (Role == "RL01")
            {
                ViewBag.EmployeeList = ToSelectList((from A in db.EmployeeMasters select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
            }
            if (Role == "RL02")
            {
                ViewBag.EmployeeList = ToSelectList((from A in db.EmployeeMasters where A.HRID == LoginID select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
            }
            if (Role == "RL03")
            {
                ViewBag.EmployeeList = ToSelectList((from A in db.EmployeeMasters where A.MangerID == LoginID select new DropDownModel { Id = A.EmployeeId.ToString(), Value = A.EmpName }).ToList());
            }

            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());
            ViewBag.HeadList = ToSelectList((from A in db.EmployeeMasters join B in db.EmployeeMasters on A.MangerID equals B.EmployeeId select new DropDownModel { Id = B.EmployeeId, Value = B.EmpName }).Distinct().ToList());
            ViewBag.AttendanceStatusList = ToSelectList1((from A in db.attendancestatusmasters select new DropDownModel { Id = A.Status, Value = A.Status }).ToList());

            List<AttendanceReport> list = new List<AttendanceReport>();

            if (fromdate != "" && todate != "")
            {
                DateTime fromd = Convert.ToDateTime(fromdate);
                DateTime tod = Convert.ToDateTime(todate);
                list = (from a in db.AttendanceLogsNewForMobiles
                        join b in db.EmployeeMasters on a.EmployeeId equals b.EmployeeId
                        join c in db.SiteMasters on b.Branch equals c.BUDescription
                        where a.AttendanceStatus == "Pending" && a.AttendanceDate >= fromd && a.AttendanceDate <= tod
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
                            //BioMetricInTime = a.BioMetricIn,
                            //BioMetricOutTime = a.BioMetricOut
                        }).OrderByDescending(x => x.AttendanceDate).ToList();
            }
            else
            {
                DateTime date = DateTime.Today;
                DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
                DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                list = (from a in db.AttendanceLogsNewForMobiles
                        join b in db.EmployeeMasters on a.EmployeeId equals b.EmployeeId
                        join c in db.SiteMasters on b.Branch equals c.BUDescription
                        where a.AttendanceStatus == "Pending" && a.AttendanceDate >= firstDayOfMonth && a.AttendanceDate <= lastDayOfMonth
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
                            //BioMetricInTime = a.BioMetricIn,
                            //BioMetricOutTime = a.BioMetricOut
                        }).OrderByDescending(x => x.AttendanceDate).ToList();
            }

            if (Role == "RL02")
            {
                list = list.Where(x => x.HRId == LoginID).ToList();
            }
            if (Role == "RL03")
            {
                list = list.Where(x => x.ManagerId == LoginID).ToList();
            }

            ViewBag.Branch = branch;
            ViewBag.Employee = emp;
            ViewBag.Head = head;
            ViewBag.FromDate = fromdate;
            ViewBag.ToDate = todate;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            if (pagesize != null)
            {
                pageSize = Convert.ToInt32(pagesize);
            }
            if (branch != "")
            {
                list = list.Where(x => x.BUCode == branch).ToList();
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

        public static bool SendMail(string toMail, string subject, string emailBody)
        {
            try
            {
                string senderEmail = System.Configuration.ConfigurationManager.AppSettings["SenderEmail"].ToString();
                string senderPassword = System.Configuration.ConfigurationManager.AppSettings["SenderPassword"].ToString();

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.EnableSsl = true;
                client.Timeout = 100000;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(senderEmail, senderPassword);
                MailMessage mailMessage = new MailMessage(senderEmail, toMail, subject, emailBody);
                mailMessage.CC.Add("nikhil.vig@teamcomputers.com");
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8;
                client.Send(mailMessage);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}