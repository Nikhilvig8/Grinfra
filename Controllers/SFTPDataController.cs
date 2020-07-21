using GrInfra.Models;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Net.Mail;
using System.Text;
using System.Net;
using System.Web.Hosting;
using System.Data;
using OfficeOpenXml.Table;
using System.Net.Http;

namespace GrInfra.Controllers
{
    public class SFTPDataController : Controller
    {
        
        public ConnectionInfo SFTPHost { get; private set; }

        public static void Main()
        {
             // RoasterData();
            //BiometricData();
            EmployeeData();
            //Upload();

        }
        public static void Push()
        {

            Upload();

        }

        public static bool ExportToExcel()
        {
            DataTable dt = GenerateDataTable();
            StreamWriter wr = new StreamWriter(HostingEnvironment.MapPath("~/UploadFiles/AttendanceDaily.csv"));
            try
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    wr.Write(dt.Columns[i].ToString().ToUpper() + "\t");
                }

                wr.WriteLine();

                //write rows to excel file
                for (int i = 0; i < (dt.Rows.Count); i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            wr.Write(Convert.ToString(dt.Rows[i][j]) + "\t");
                        }
                        else
                        {
                            wr.Write("\t");
                        }
                    }
                    //go to next line
                    wr.WriteLine();
                }
                //close file
                wr.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            return false; ;
        }

        public static DataTable GenerateDataTable()
        {
            GInfraEntities db = new GInfraEntities();
            List<AttendanceReport> list = new List<AttendanceReport>();
            DateTime today = DateTime.Today;
            today = today.AddDays(-1);

            list = (from a in db.AttendanceLogsNewForMobiles
                    join b in db.EmployeeMasters on a.EmployeeId equals b.EmployeeId
                    join c in db.SiteMasters on b.Branch equals c.BUDescription
                    where a.AttendanceDate == today
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
                        Branch = b.Branch
                    }).OrderByDescending(x => x.AttendanceDate).ToList();


            DataTable Dt = new DataTable();
            Dt.Columns.Add("Attendance Date,", typeof(string));
            Dt.Columns.Add("Employee ID,", typeof(string));
            Dt.Columns.Add("Employee Name,", typeof(string));
            Dt.Columns.Add("In Time,", typeof(string));
            Dt.Columns.Add("Out Time,", typeof(string));


            try
            {
                foreach (var data in list)
                {
                    DataRow row = Dt.NewRow();
                    row[0] = data.AttendanceDate.ToString("yyyy-MM-dd") + ",";
                    row[1] = data.EmployeeId + ",";
                    row[2] = data.EmployeeName + ",";
                    DateTime indate = Convert.ToDateTime(data.InTime);
                    row[3] = indate.Hour.ToString() + ":" + indate.Minute.ToString() + ":" + indate.Second.ToString() + ",";
                    DateTime outdate = Convert.ToDateTime(data.InTime);
                    row[4] = outdate.Hour.ToString() + ":" + outdate.Minute.ToString() + ":" + outdate.Second.ToString() + ",";

                    Dt.Rows.Add(row);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return Dt;
        }
        public static void SendHrannoucement()
        {
            string dateString2 = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime king = Convert.ToDateTime(dateString2);
            GInfraEntities GInfraEntities = new GInfraEntities();

            var BUCode = (from res in GInfraEntities.EmployeeMasters


                            select new
                            {
                                BUCode = res.BUCode ?? "-"



                            }).Distinct().ToList();
            foreach (var k in BUCode)
            {
                var count = (from c in GInfraEntities.sp_hrnotification(k.BUCode).Where(x => x.FromDate.Value.Year == king.Year
                            && x.FromDate.Value.Month == king.Month
                            && x.FromDate.Value.Day == king.Day)
                                             select c.id).Count();


                if (count > 0)
                {
                    var Employeeid = (from res in GInfraEntities.EmployeeMasters.Where(m=>m.BUCode==k.BUCode)


                                  select new
                                  {
                                      BUCode = res.BUCode ?? "-",
                                      Employeeid = res.EmployeeId ?? "-"



                                  }).Distinct().ToList();
                    foreach(var l in Employeeid)
                    {
                        //var hrdetails = (from c in GInfraEntities.sp_hrnotification(k.BUCode).Where(m =>  m.ToDate > king)
                        var hrdetails = (from c in GInfraEntities.sp_hrnotification(k.BUCode).Where(x => x.FromDate.Value.Year == king.Year
                            && x.FromDate.Value.Month == king.Month
                            && x.FromDate.Value.Day == king.Day)
                                         select new
                                         {
                                             Title = c.Title ?? "-",
                                             Description = c.Description ?? "-"



                                         }).ToList();
                        if (hrdetails.Count > 0)
                        {


                            foreach (var n in hrdetails)
                            {
                                var gettoken = (from e in GInfraEntities.MasterPasswords

                                                where e.UserID == l.Employeeid



                                                select new
                                                {
                                                    Devicetoken = e.DeviceToken,



                                                }).SingleOrDefault();
                                string DeviceToken = "-";
                                try
                                {
                                     DeviceToken = gettoken.Devicetoken;
                                }
                                catch(Exception ex)
                                {
                                    DeviceToken = "-";

                                }

                                if (DeviceToken != "-")
                                {


                                    // DeviceToken = "eNkwJf36cSI:APA91bFeOCPQNRznvSk3lEqJZpCySl14Tvkafh2zjKa4zBF7cBquMZkMsD4L8OKLAt8PgsuDlztI8NfozEmUXGUigUjkdYFQDycIvN1UiG0LyM2b_D1VteRQ-0AVhIlVPT_aZG-nnXnx";
                                    string serverKey = "AAAASLs8D7Y:APA91bFYt4IFQRR4NPLNhX0SKzd_VQjxrvTE1mlS1rYk648fhrW3KvcejHCjwpjh9rHlLkSth7ewFN7ogmJ2mo7znvQXG1nXC0ny5cP14lz4rOM1FUOlGi-ZEdnkPW3kZxk9F15wTsPa";
                                    try
                                    {
                                        var result1 = "-1";
                                        var webAddr = "https://fcm.googleapis.com/fcm/send";

                                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                                        httpWebRequest.ContentType = "application/json";
                                        httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                                        httpWebRequest.Method = "POST";

                                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                        {
                                            string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"HrAnnoucement\",\"json\":{\"Title\":\" " + n.Title + " \",\"body\":\"" + n.Description + "\",\"date\":\"\"}}}";
                                            //string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Approved Attendance by Manager" + "\",\"Description\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + "\"}]}}";
                                            //  string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"HrAnnoucement\",\"json\":{\"Title\":\"Welcome to a new way of working!\",\"body\":\"Welcome to a new way of working!We at t:eam will be adopting Workplace by Facebook for all our internal communications. A lot like Facebook, but built for the world of work.Activate your profile today at teamcomputers.facebook.com For further details, refer to the mail sent..\",\"date\":\"9/09/2019 5:15:47 PM\",\"image_url\":\"http://111.93.123.102:8087/BannerData/1582019782Banner.png\"}}}";
                                            //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"" + WithoutImage.MessageTitle + "\",\"body\":\"" + WithoutImage.Description + "\",\"date\":\"" + dateTime.ToString("dd/MM/yyyy") + "\",\"image_url\":\"" + WthImage.filepath + "\"}}}";
                                            // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"body\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"date\":\"23/10/2019 12:00:00 PM\",\"image_url\":\"http://teamworksnew.teamcomputers.com:5556/Images/Final-Day-Diwali-Creative.gif\"}}}";

                                            streamWriter.Write(json);
                                            streamWriter.Flush();
                                        }

                                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                        {
                                            result1 = streamReader.ReadToEnd();
                                        }
                                        var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



                                    }
                                    catch (Exception e)
                                    {

                                    }

                                    try
                                    {
                                        var result1 = "-1";
                                        var webAddr = "https://fcm.googleapis.com/fcm/send";

                                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                                        httpWebRequest.ContentType = "application/json";
                                        httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                                        httpWebRequest.Method = "POST";

                                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                                        {
                                            string json = "{\"to\": \"" + DeviceToken + "\",\"content_available\": true,\"notification\": { \"title\": \"HrAnnoucement\",\"body\": \"" + "" + n.Title + "" + "\",\"click_action\": \"fcm.ACTION.HELLO\"},\"data\": { \"extra\": \"juice\"}}";

                                            //string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Approved Attendance by Manager" + "\",\"Description\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + "\"}]}}";
                                            //  string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"HrAnnoucement\",\"json\":{\"Title\":\"Welcome to a new way of working!\",\"body\":\"Welcome to a new way of working!We at t:eam will be adopting Workplace by Facebook for all our internal communications. A lot like Facebook, but built for the world of work.Activate your profile today at teamcomputers.facebook.com For further details, refer to the mail sent..\",\"date\":\"9/09/2019 5:15:47 PM\",\"image_url\":\"http://111.93.123.102:8087/BannerData/1582019782Banner.png\"}}}";
                                            //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"" + WithoutImage.MessageTitle + "\",\"body\":\"" + WithoutImage.Description + "\",\"date\":\"" + dateTime.ToString("dd/MM/yyyy") + "\",\"image_url\":\"" + WthImage.filepath + "\"}}}";
                                            // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"body\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"date\":\"23/10/2019 12:00:00 PM\",\"image_url\":\"http://teamworksnew.teamcomputers.com:5556/Images/Final-Day-Diwali-Creative.gif\"}}}";

                                            streamWriter.Write(json);
                                            streamWriter.Flush();
                                        }

                                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                        {
                                            result1 = streamReader.ReadToEnd();
                                        }
                                        var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



                                    }
                                    catch (Exception e)
                                    {

                                    }
                                    GInfraEntities.sp_notification(l.Employeeid, "HrAnnoucement", n.Title,n.Description );
                                }
                            }
                        }
                    }





                }
            }

        }
        public static void SendPendingRequestMail()
        {
            GInfraEntities GInfraEntities = new GInfraEntities();

            string dateString2 = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime king = Convert.ToDateTime(dateString2);
            var headlist = (from res in GInfraEntities.EmployeeMasters


                            select new
                            {
                                Headid = res.MangerID ?? "-"



                            }).Distinct().ToList();


            foreach (var k in headlist)
            {
                var count = (from c in GInfraEntities.AttendanceLogsNewForMobiles.Where(m => m.AttendanceDate == king && m.AttendanceStatus == "Pending" && m.HeadId == k.Headid) select c.AttendanceStatus).Count();


                if (count > 0)
                {


                    try
                    {


                        var detail1 = (from ep in GInfraEntities.sp_getheaddatabyuserid(k.Headid)



                                       select new
                                       {
                                           UserName = ep.UserName ?? "-",
                                           UserNameMail = ep.UserNameMail ?? "-",
                                           ReportingHeadName = ep.ReportingHeadName ?? "-",
                                           ReportingHeadMail = ep.ReportingHeadMail ?? "-",
                                           EmployeeId = ep.EmployeeId ?? "-",
                                           ReportingHeadId = ep.ReportingHeadId ?? "-",



                                       }).SingleOrDefault();
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        //mail.To.Add(usermailid);

                        mail.To.Add("nikhil.vig@teamcomputers.com");
                        mail.CC.Add("nikhil.vig@teamcomputers.com");
                        mail.Subject = "Pending - Attendance ";
                        mail.IsBodyHtml = true;
                        mail.Priority = MailPriority.High;
                        mail.Body = "Dear <b>" + detail1.UserName + ",</b><br/><br/>Your " + count + " Attendance Pending Request is left.<br/><br/>Please login into Mobile Application <br/><br/><br/><br/>Wish You a Great Day Ahead!<br/><br/>Regards,<br/>Human Capital";
                       // SmtpServer.Send(mail);




                        var gettoken = (from e in GInfraEntities.MasterPasswords

                                        where e.UserID == k.Headid



                                        select new
                                        {
                                            Devicetoken = e.DeviceToken,



                                        }).SingleOrDefault();

                        string DeviceToken = gettoken.Devicetoken;


                        // DeviceToken = "eNkwJf36cSI:APA91bFeOCPQNRznvSk3lEqJZpCySl14Tvkafh2zjKa4zBF7cBquMZkMsD4L8OKLAt8PgsuDlztI8NfozEmUXGUigUjkdYFQDycIvN1UiG0LyM2b_D1VteRQ-0AVhIlVPT_aZG-nnXnx";
                        string serverKey = "AAAASLs8D7Y:APA91bFYt4IFQRR4NPLNhX0SKzd_VQjxrvTE1mlS1rYk648fhrW3KvcejHCjwpjh9rHlLkSth7ewFN7ogmJ2mo7znvQXG1nXC0ny5cP14lz4rOM1FUOlGi-ZEdnkPW3kZxk9F15wTsPa";
                        try
                        {
                            var result1 = "-1";
                            var webAddr = "https://fcm.googleapis.com/fcm/send";

                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                            httpWebRequest.Method = "POST";

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"PolygonManager\",\"json\":{\"Title\":\"Your " + count + " Attendance Pending Request is left.\",\"body\":\"Your " + count + " Attendance Pending Request is left.\",\"date\":\"\"}}}";
                                //string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Approved Attendance by Manager" + "\",\"Description\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + "\"}]}}";
                                //  string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"HrAnnoucement\",\"json\":{\"Title\":\"Welcome to a new way of working!\",\"body\":\"Welcome to a new way of working!We at t:eam will be adopting Workplace by Facebook for all our internal communications. A lot like Facebook, but built for the world of work.Activate your profile today at teamcomputers.facebook.com For further details, refer to the mail sent..\",\"date\":\"9/09/2019 5:15:47 PM\",\"image_url\":\"http://111.93.123.102:8087/BannerData/1582019782Banner.png\"}}}";
                                //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"" + WithoutImage.MessageTitle + "\",\"body\":\"" + WithoutImage.Description + "\",\"date\":\"" + dateTime.ToString("dd/MM/yyyy") + "\",\"image_url\":\"" + WthImage.filepath + "\"}}}";
                                // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"body\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"date\":\"23/10/2019 12:00:00 PM\",\"image_url\":\"http://teamworksnew.teamcomputers.com:5556/Images/Final-Day-Diwali-Creative.gif\"}}}";

                                streamWriter.Write(json);
                                streamWriter.Flush();
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result1 = streamReader.ReadToEnd();
                            }
                            var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



                        }
                        catch (Exception e)
                        {

                        }

                        try
                        {
                            var result1 = "-1";
                            var webAddr = "https://fcm.googleapis.com/fcm/send";

                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                            httpWebRequest.Method = "POST";

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {

                                string json = "{\"to\": \"" + DeviceToken + "\",\"content_available\": true,\"notification\": { \"title\": \"PolygonManager\",\"body\": \"Your " + count + " Attendance Pending Request is left.\",\"click_action\": \"fcm.ACTION.HELLO\"},\"data\": { \"extra\": \"juice\"}}";
                                //string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Approved Attendance by Manager" + "\",\"Description\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + "\"}]}}";
                                //  string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"HrAnnoucement\",\"json\":{\"Title\":\"Welcome to a new way of working!\",\"body\":\"Welcome to a new way of working!We at t:eam will be adopting Workplace by Facebook for all our internal communications. A lot like Facebook, but built for the world of work.Activate your profile today at teamcomputers.facebook.com For further details, refer to the mail sent..\",\"date\":\"9/09/2019 5:15:47 PM\",\"image_url\":\"http://111.93.123.102:8087/BannerData/1582019782Banner.png\"}}}";
                                //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"" + WithoutImage.MessageTitle + "\",\"body\":\"" + WithoutImage.Description + "\",\"date\":\"" + dateTime.ToString("dd/MM/yyyy") + "\",\"image_url\":\"" + WthImage.filepath + "\"}}}";
                                // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"body\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"date\":\"23/10/2019 12:00:00 PM\",\"image_url\":\"http://teamworksnew.teamcomputers.com:5556/Images/Final-Day-Diwali-Creative.gif\"}}}";

                                streamWriter.Write(json);
                                streamWriter.Flush();
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result1 = streamReader.ReadToEnd();
                            }
                            var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



                        }
                        catch (Exception e)
                        {

                        }


                        GInfraEntities.sp_notification(k.Headid, "PolygonManager", "Your " + count + " Attendance Pending Request is left.", "Your " + count + " Attendance Pending Request is left.");

                    }
                    catch(Exception e)
                    {

                    }




                       
                    }
            }
            


        }

        public static void HRSendPendingRequestMail()
        {
            GInfraEntities GInfraEntities = new GInfraEntities();

            string dateString2 = DateTime.Now.ToString("yyyy-MM-dd");
            DateTime king = Convert.ToDateTime(dateString2);
            var headlist = (from res in GInfraEntities.EmployeeMasters


                            select new
                            {
                                Headid = res.HRID ?? "-"



                            }).Distinct().ToList();


            foreach (var k in headlist)
            {
                var count = (from lst1 in GInfraEntities.EmployeeMasters
                             join lst2 in GInfraEntities.AttendanceLogsNewForMobiles on lst1.EmployeeId equals lst2.EmployeeId into yG
                             from y1 in yG.DefaultIfEmpty().Where(m => m.AttendanceDate == king && m.AttendanceStatus == "Pending" && m.HeadId == k.Headid)
                             select new { X = lst1, Y = y1 }).Count();
              //  var count = (from c in GInfraEntities.AttendanceLogsNewForMobiles.Where(m => m.AttendanceDate == king && m.AttendanceStatus == "Pending" && m.HeadId == k.Headid) select c.AttendanceStatus).Count();


                if (count > 0)
                {


                    try
                    {


                        var detail1 = (from ep in GInfraEntities.sp_getheaddatabyuserid(k.Headid)



                                       select new
                                       {
                                           UserName = ep.UserName ?? "-",
                                           UserNameMail = ep.UserNameMail ?? "-",
                                           ReportingHeadName = ep.ReportingHeadName ?? "-",
                                           ReportingHeadMail = ep.ReportingHeadMail ?? "-",
                                           EmployeeId = ep.EmployeeId ?? "-",
                                           ReportingHeadId = ep.ReportingHeadId ?? "-",



                                       }).SingleOrDefault();
                        MailMessage mail = new MailMessage();
                        SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        //mail.To.Add(usermailid);

                        mail.To.Add("nikhil.vig@teamcomputers.com");
                        mail.CC.Add("nikhil.vig@teamcomputers.com");
                        mail.Subject = "Pending - Attendance ";
                        mail.IsBodyHtml = true;
                        mail.Priority = MailPriority.High;
                        mail.Body = "Dear <b>" + detail1.UserName + ",</b><br/><br/>Your " + count + " Attendance Pending Request is left.<br/><br/>Please login into Mobile Application <br/><br/><br/><br/>Wish You a Great Day Ahead!<br/><br/>Regards,<br/>Human Capital";
                   //   SmtpServer.Send(mail);




                        var gettoken = (from e in GInfraEntities.MasterPasswords

                                        where e.UserID == k.Headid



                                        select new
                                        {
                                            Devicetoken = e.DeviceToken,



                                        }).SingleOrDefault();

                        string DeviceToken = gettoken.Devicetoken;


                        // DeviceToken = "eNkwJf36cSI:APA91bFeOCPQNRznvSk3lEqJZpCySl14Tvkafh2zjKa4zBF7cBquMZkMsD4L8OKLAt8PgsuDlztI8NfozEmUXGUigUjkdYFQDycIvN1UiG0LyM2b_D1VteRQ-0AVhIlVPT_aZG-nnXnx";
                        string serverKey = "AAAASLs8D7Y:APA91bFYt4IFQRR4NPLNhX0SKzd_VQjxrvTE1mlS1rYk648fhrW3KvcejHCjwpjh9rHlLkSth7ewFN7ogmJ2mo7znvQXG1nXC0ny5cP14lz4rOM1FUOlGi-ZEdnkPW3kZxk9F15wTsPa";
                        try
                        {
                            var result1 = "-1";
                            var webAddr = "https://fcm.googleapis.com/fcm/send";

                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                            httpWebRequest.Method = "POST";

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"PolygonStatus\",\"json\":{\"Title\":\"Your " + count + " Attendance Pending Request is left.\",\"body\":\"" + count + " punchin from outside the polygon\",\"date\":\"\"}}}";
                                //string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Approved Attendance by Manager" + "\",\"Description\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + "\"}]}}";
                                //  string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"HrAnnoucement\",\"json\":{\"Title\":\"Welcome to a new way of working!\",\"body\":\"Welcome to a new way of working!We at t:eam will be adopting Workplace by Facebook for all our internal communications. A lot like Facebook, but built for the world of work.Activate your profile today at teamcomputers.facebook.com For further details, refer to the mail sent..\",\"date\":\"9/09/2019 5:15:47 PM\",\"image_url\":\"http://111.93.123.102:8087/BannerData/1582019782Banner.png\"}}}";
                                //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"" + WithoutImage.MessageTitle + "\",\"body\":\"" + WithoutImage.Description + "\",\"date\":\"" + dateTime.ToString("dd/MM/yyyy") + "\",\"image_url\":\"" + WthImage.filepath + "\"}}}";
                                // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"body\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"date\":\"23/10/2019 12:00:00 PM\",\"image_url\":\"http://teamworksnew.teamcomputers.com:5556/Images/Final-Day-Diwali-Creative.gif\"}}}";

                                streamWriter.Write(json);
                                streamWriter.Flush();
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result1 = streamReader.ReadToEnd();
                            }
                            var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



                        }
                        catch (Exception e)
                        {

                        }

                        try
                        {
                            var result1 = "-1";
                            var webAddr = "https://fcm.googleapis.com/fcm/send";

                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Headers.Add("Authorization:key=" + serverKey);
                            httpWebRequest.Method = "POST";

                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                string json = "{\"to\": \"" + DeviceToken + "\",\"content_available\": true,\"notification\": { \"title\": \"PolygonStatus\",\"body\": \"Your " + count + " Attendance Pending Request is left.\",\"click_action\": \"fcm.ACTION.HELLO\"},\"data\": { \"extra\": \"juice\"}}";
                                //string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Approved Attendance by Manager" + "\",\"Description\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + "\"}]}}";
                                //  string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"HrAnnoucement\",\"json\":{\"Title\":\"Welcome to a new way of working!\",\"body\":\"Welcome to a new way of working!We at t:eam will be adopting Workplace by Facebook for all our internal communications. A lot like Facebook, but built for the world of work.Activate your profile today at teamcomputers.facebook.com For further details, refer to the mail sent..\",\"date\":\"9/09/2019 5:15:47 PM\",\"image_url\":\"http://111.93.123.102:8087/BannerData/1582019782Banner.png\"}}}";
                                //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"" + WithoutImage.MessageTitle + "\",\"body\":\"" + WithoutImage.Description + "\",\"date\":\"" + dateTime.ToString("dd/MM/yyyy") + "\",\"image_url\":\"" + WthImage.filepath + "\"}}}";
                                // string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"Team Creative\",\"json\":{\"Title\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"body\":\"A Rangoli Competition. Win & take away a Rs 2500 voucher(One Team, every Location) \",\"date\":\"23/10/2019 12:00:00 PM\",\"image_url\":\"http://teamworksnew.teamcomputers.com:5556/Images/Final-Day-Diwali-Creative.gif\"}}}";

                                streamWriter.Write(json);
                                streamWriter.Flush();
                            }

                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                result1 = streamReader.ReadToEnd();
                            }
                            var msg = new HttpResponseMessage(HttpStatusCode.OK) { ReasonPhrase = "Notification sended" };



                        }
                        catch (Exception e)
                        {

                        }

                        GInfraEntities.sp_notification(k.Headid, "PolygonStatus", "Your " + count + " Attendance Pending Request is left.", "Your " + count + " Attendance Pending Request is left.");
                    }
                    catch (Exception e)
                    {

                    }





                }
            }



        }

        private static void Upload()
        {
            bool resu = ExportToExcel();
            if (resu == true)
            {
                string SFTPFolderDest = HostingEnvironment.MapPath("~/UploadFiles") + "/";
                bool res = CopyFileFromLocalToRemote("sftp44.sapsf.com", "11640128P", "2mkPQa7UA731", SFTPFolderDest, "/Work_Schedule");
                if (res)
                {
                    string userEmailBody;
                    string userEmailSubject = "Upload Data Daily Status - " + DateTime.Now.ToString("dddd, dd MMMM yyyy"); ;
                    bool ress = ImportRoasterData();
                    if (ress)
                    {
                        userEmailBody = "<div><div>Hello Sir,</div><div>Upload is <b>Successful</b>.</div></div>";
                    }
                    else
                    {
                        userEmailBody = "<div><div>Hello Sir,</div><div>Upload is <b>UnSuccessful</b>.</div></div>";
                    }
                    SendMail("Nikhil.vig@teamcomputers.com", userEmailSubject, userEmailBody);
                }
            }
            else
            {
                string userEmailSubject = "Export Attendance Data Daily Status - " + DateTime.Now.ToString("dddd, dd MMMM yyyy");
                string userEmailBody = "<div><div>Hello Sir,</div><div>Export Daily Attendance is <b>UnSuccessful</b>.</div></div>";
                SendMail("Nikhil.vig@teamcomputers.com", userEmailSubject, userEmailBody);
            }

        }

        public static bool CopyFileFromLocalToRemote(string host, string user, string password, string localPath, string remotePath)
        {
            try
            {
                using (SftpClient client = new SftpClient(host, user, password))
                {
                    client.KeepAliveInterval = TimeSpan.FromSeconds(60);
                    client.ConnectionInfo.Timeout = TimeSpan.FromMinutes(180);
                    client.OperationTimeout = TimeSpan.FromMinutes(180);
                    client.Connect();
                    bool connected = client.IsConnected;

                    string fileName = "AttendanceDaily.csv";

                    var files = client.ListDirectory(remotePath);
                    client.ChangeDirectory("/Work_Schedule/");
                    //client.DownloadFile(remotePath, file);
                    using (var uplfileStream = System.IO.File.OpenRead(Path.Combine(localPath, fileName)))
                    {
                        client.UploadFile(uplfileStream, fileName, true);
                    }
                    //file.Close();
                    client.Disconnect();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        // GET: SFTPData
        public static void RoasterData()
        {
            string SFTPFolderDest = HostingEnvironment.MapPath("~/UploadFiles") + "/";
            bool res = CopyFileFromRemoteToLocal("sftp44.sapsf.com", "11640128P", "2mkPQa7UA731", SFTPFolderDest, "/Work_Schedule");
            if (res)
            {
                string userEmailBody;
                string userEmailSubject = "Roaster Data Daily Status - " + DateTime.Now.ToString("dddd, dd MMMM yyyy"); ;
                bool ress = ImportRoasterData();
                if (ress)
                {
                    userEmailBody = "<div><div>Hello Sir,</div><div>Automatic Roaster data fetching and saving to the database is <b>Successful</b>.</div></div>";
                }
                else
                {
                    userEmailBody = "<div><div>Hello Sir,</div><div>Automatic Roaster data fetching and saving to the database is <b>UnSuccessful</b>.</div></div>";
                }
                SendMail("Nikhil.vig@teamcomputers.com", userEmailSubject, userEmailBody);
            }
        }
        public static void EmployeeData()
        {
            string SFTPFolderDest = HostingEnvironment.MapPath("~/UploadFiles") + "/";
            bool res = CopyFileFromRemoteToLocal("sftp44.sapsf.com", "11640128P", "2mkPQa7UA731", SFTPFolderDest, "/Work_Schedule");
            if (res)
            {
                string userEmailBody;
                string userEmailSubject = "Employee Data Daily Status - " + DateTime.Now.ToString("dddd, dd MMMM yyyy"); ;
                bool ress = ImportEmployeeData();
                if (ress)
                {
                    userEmailBody = "<div><div>hello sir,</div><div>automatic employee data fetching and saving to the database is <b>successful</b>.</div></div>";
                }
                else
                {
                    userEmailBody = "<div><div>hello sir,</div><div>automatic employee data fetching and saving to the database is <b>unsuccessful</b>.</div></div>";
                }
                SendMail("Nikhil.vig@teamcomputers.com", userEmailSubject, userEmailBody);
            }
        }
        public static bool ImportEmployeeData()
        {
            try
            {
                string path = HostingEnvironment.MapPath("~/UploadFiles") + "\\Mobile_App_Report-Component1.csv";

                GInfraEntities db = new GInfraEntities();
                db.Database.ExecuteSqlCommand("Truncate Table EmployeeMaster");

                int j = 0;
                string[] data = System.IO.File.ReadAllLines(path);
                foreach (string row in data)
                {
                    if (j > 2)
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            EmployeeMaster employee = new EmployeeMaster();
                            int i = 0;
                            foreach (string cell in row.Split(','))
                            {
                                string tempdata = cell.Replace("\"", "");
                                if (i == 0)
                                {
                                    employee.EmployeeId = tempdata.ToString();
                                }
                                else if (i == 1)
                                {
                                    //tempdata = "2014-12-12";
                                    employee.DateOfJoining = tempdata;
                                }
                                else if (i == 2)
                                {
                                    employee.EmpName = tempdata.ToString();
                                }
                                else if (i == 3)
                                {
                                    employee.EmpEmailId = tempdata.ToString();
                                }
                                else if (i == 4)
                                {
                                    employee.EmpMobile = tempdata.ToString();
                                }
                                else if (i == 5)
                                {
                                    employee.MangerID = tempdata.ToString();
                                }
                                else if (i == 6)
                                {
                                    employee.MangerName = tempdata.ToString();
                                }
                                else if (i == 7)
                                {
                                    employee.ManagerMobile = tempdata.ToString();
                                }
                                else if (i == 8)
                                {
                                    employee.MangerEmail = tempdata.ToString();
                                }
                                else if (i == 9)
                                {
                                    employee.HRName = tempdata.ToString();
                                }
                                else if (i == 10)
                                {
                                    employee.HRManagerEmail = tempdata.ToString();
                                }
                                else if (i == 11)
                                {
                                    employee.HRManagerMobile = tempdata.ToString();
                                }
                                else if (i == 12)
                                {
                                    employee.HRID = tempdata.ToString();
                                }
                                else if (i == 13)
                                {
                                    employee.Branch = tempdata.ToString();
                                }
                                else if (i == 14)
                                {
                                    employee.Company = tempdata.ToString();
                                }
                                else if (i == 15)
                                {
                                    employee.Department = tempdata.ToString();
                                }
                                else if (i == 16)
                                {
                                    employee.Designation = tempdata.ToString();
                                }
                                else if (i == 17)
                                {
                                    employee.GradeCode = tempdata.ToString();
                                }
                                else if (i == 18)
                                {
                                    //tempdata = "2014-12-12";
                                    employee.DateOfBirth = tempdata; ;
                                }
                                else if (i == 19)
                                {
                                    employee.Sex = tempdata.ToString();
                                }
                                else if (i == 20)
                                {
                                    employee.BUCode = tempdata.ToString();
                                }
                                i++;
                            }
                            db.EmployeeMasters.Add(employee);
                            db.SaveChanges();


                        }
                    }
                    j++;
                }
                db.Sftpemployeepassword();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool ImportRoasterData()
        {
            try
            {
                string path = HostingEnvironment.MapPath("~/UploadFiles") + "\\WS.csv";

                GInfraEntities db = new GInfraEntities();
                db.Database.ExecuteSqlCommand("Truncate Table Roaster");

                string[] data = System.IO.File.ReadAllLines(path);
                foreach (string row in data)
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        Roaster roaster = new Roaster();
                        int i = 0;
                        foreach (string cell in row.Split(','))
                        {
                            if (i == 0)
                            {
                                
                                roaster.date = cell;
                               // roaster.date = Convert.ToDateTime("2014-12-12");
                            }
                            else if (i == 1)
                            {
                                roaster.EmployeeId = cell.ToString();
                            }
                            else if (i == 2)
                            {
                                roaster.Location = cell.ToString();
                            }
                            else if (i == 3)
                            {
                                roaster.ShiftTimming = cell.ToString();
                            }
                            i++;
                        }
                        db.Roasters.Add(roaster);
                        db.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static void BiometricData()
        {
            string SFTPFolderDest = HostingEnvironment.MapPath("~/UploadFiles") + "/";
            bool res = CopyFileFromRemoteToLocal("sftp44.sapsf.com", "11640128P", "2mkPQa7UA731", SFTPFolderDest, "/Attendance");
            if (res)
            {
                string userEmailBody;
                string userEmailSubject = "Biometric Data Daily Status - " + DateTime.Now.ToString("dddd, dd MMMM yyyy"); ;
                bool ress = ImportBiometricData();
                if (ress)
                {
                    userEmailBody = "<div><div>Hello Sir,</div><div>Automatic Biometric data fetching and saving to the database is <b>Successful</b>.</div></div>";
                }
                else
                {
                    userEmailBody = "<div><div>Hello Sir,</div><div>Automatic Biometric data fetching and saving to the database is <b>UnSuccessful</b>.</div></div>";
                }
                SendMail("Nikhil.vig@teamcomputers.com", userEmailSubject, userEmailBody);
            }
        }
        public static bool ImportBiometricData()
        {
            try
            {
                string path = HostingEnvironment.MapPath("~/UploadFiles") + "\\attendance.csv";

                GInfraEntities db = new GInfraEntities();

                int j = 0;
                string[] data = System.IO.File.ReadAllLines(path);
                foreach (string row in data)
                {
                    if (j == 0 || j == 1)
                    {

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            string[] cells = row.Split(',');
                            BiometricAttendance biometricAttendanceCheck = new BiometricAttendance();
                            string EmployeeId = cells[1].ToString();
                            DateTime startdate = Convert.ToDateTime(cells[2]);
                            biometricAttendanceCheck = db.BiometricAttendances.Where(x => x.EmployeeId == EmployeeId && x.StartDate == startdate).FirstOrDefault();
                            if (biometricAttendanceCheck == null)
                            {
                                int i = 0;

                                BiometricAttendance biometricAttendance = new BiometricAttendance();
                                foreach (var cell in cells)
                                {
                                    if (i == 1)
                                    {
                                        biometricAttendance.EmployeeId = cell.ToString();
                                    }
                                    if (i == 2)
                                    {
                                        biometricAttendance.StartDate = Convert.ToDateTime(cell);
                                    }
                                    if (i == 3)
                                    {
                                        if (cell == null || cell == "")
                                        {
                                            biometricAttendance.PunchInDate = null;
                                        }
                                        else
                                        {
                                            biometricAttendance.PunchInDate = Convert.ToDateTime(cell);
                                        }
                                    }
                                    if (i == 4)
                                    {
                                        if (cell == null || cell == "")
                                        {
                                            biometricAttendance.PunchInTime = null;
                                        }
                                        else
                                        {
                                            DateTime dateTime = Convert.ToDateTime(cell);
                                            biometricAttendance.PunchInTime = dateTime.TimeOfDay;
                                        }
                                    }
                                    if (i == 5)
                                    {
                                        if (cell == null || cell == "")
                                        {
                                            biometricAttendance.PunchOutDate = null;
                                        }
                                        else
                                        {
                                            biometricAttendance.PunchOutDate = Convert.ToDateTime(cell);
                                        }
                                    }
                                    if (i == 6)
                                    {
                                        if (cell == null || cell == "")
                                        {
                                            biometricAttendance.PunchOutTime = null;
                                        }
                                        else
                                        {
                                            DateTime dateTime = Convert.ToDateTime(cell);
                                            biometricAttendance.PunchOutTime = dateTime.TimeOfDay;
                                        }
                                    }
                                    if (i == 7)
                                    {
                                        biometricAttendance.CustRegularized = cell.ToString();
                                    }
                                    i++;
                                }
                                db.BiometricAttendances.Add(biometricAttendance);
                                db.SaveChanges();
                            }
                            else
                            {
                                int i = 0;

                                foreach (var cell in cells)
                                {
                                    //if (i == 1)
                                    //{
                                    //    biometricAttendanceCheck.EmployeeId = cell.ToString();
                                    //}
                                    //if (i == 2)
                                    //{
                                    //    biometricAttendanceCheck.StartDate = Convert.ToDateTime(cell);
                                    //}
                                    if (i == 3)
                                    {
                                        if (cell == null || cell == "")
                                        {
                                            biometricAttendanceCheck.PunchInDate = null;
                                        }
                                        else
                                        {
                                            biometricAttendanceCheck.PunchInDate = Convert.ToDateTime(cell);
                                        }
                                    }
                                    if (i == 4)
                                    {
                                        if (cell == null || cell == "")
                                        {
                                            biometricAttendanceCheck.PunchInTime = null;
                                        }
                                        else
                                        {
                                            DateTime dateTime = Convert.ToDateTime(cell);
                                            biometricAttendanceCheck.PunchInTime = dateTime.TimeOfDay;
                                        }
                                    }
                                    if (i == 5)
                                    {
                                        if (cell == null || cell == "")
                                        {
                                            biometricAttendanceCheck.PunchOutDate = null;
                                        }
                                        else
                                        {
                                            biometricAttendanceCheck.PunchOutDate = Convert.ToDateTime(cell);
                                        }
                                    }
                                    if (i == 6)
                                    {
                                        if (cell == null || cell == "")
                                        {
                                            biometricAttendanceCheck.PunchOutTime = null;
                                        }
                                        else
                                        {
                                            DateTime dateTime = Convert.ToDateTime(cell);
                                            biometricAttendanceCheck.PunchOutTime = dateTime.TimeOfDay;
                                        }
                                    }
                                    if (i == 7)
                                    {
                                        biometricAttendanceCheck.CustRegularized = cell.ToString();
                                    }
                                    i++;
                                }
                                db.Entry(biometricAttendanceCheck).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                    }
                    j++;
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static bool CopyFileFromRemoteToLocal(string host, string user, string password, string localPath, string remotePath)
        {
            try
            {
                using (SftpClient client = new SftpClient(host, user, password))
                {
                    client.KeepAliveInterval = TimeSpan.FromSeconds(60);
                    client.ConnectionInfo.Timeout = TimeSpan.FromMinutes(180);
                    client.OperationTimeout = TimeSpan.FromMinutes(180);
                    client.Connect();
                    bool connected = client.IsConnected;

                    var files = client.ListDirectory(remotePath);
                    //client.DownloadFile(remotePath, file);
                    foreach (var file in files)
                    {
                        if (file.Name == "WS.csv")
                        {
                            string fileName = "WS.csv";
                            using (Stream fileStream = System.IO.File.OpenWrite(Path.Combine(localPath, fileName)))
                            {
                                client.DownloadFile(file.FullName, fileStream);
                            }
                        }
                        if (file.Name == "attendance.csv")
                        {
                            using (Stream fileStream = System.IO.File.OpenWrite(Path.Combine(localPath, file.Name)))
                            {
                                client.DownloadFile(file.FullName, fileStream);
                            }
                        }
                        if (file.Name == "Mobile_App_Report-Component1.csv")
                        {
                            using (Stream fileStream = System.IO.File.OpenWrite(Path.Combine(localPath, file.Name)))
                            {
                                client.DownloadFile(file.FullName, fileStream);
                            }
                        }
                    }
                    //file.Close();
                    client.Disconnect();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

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