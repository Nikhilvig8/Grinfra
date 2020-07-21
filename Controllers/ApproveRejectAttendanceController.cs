using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class ApproveRejectAttendanceController : ApiController
    {

        GInfraEntities GInfraEntities = new GInfraEntities();


        public HttpResponseMessage Post([FromBody]ApproveRejectAttendanceRequest request)
        {

            ApproveRejectAttendance ApproveRejectAttendance = new ApproveRejectAttendance();
            try
            {
                if (request.ManagerComment != "" && request.ManagerComment != null && request.AttendanceLogId != null && request.Action != null && request.Action != "")
                {

                    List<sp_ApproveorRejectAttendance_Result> sp_ApproveorRejectAttendance_Result = new List<sp_ApproveorRejectAttendance_Result>();

                    String[] idRepo = request.AttendanceLogId.Split(',');



                    for (int i = 0; i < idRepo.Length; i++)
                    {
                        int l = Convert.ToInt32(idRepo[i]);
                        sp_ApproveorRejectAttendance_Result = GInfraEntities.sp_ApproveorRejectAttendance(idRepo[i].ToString(), request.ManagerComment, request.Action, request.userid).ToList();


                        if (sp_ApproveorRejectAttendance_Result.Count > 0)
                        {
                            foreach (var item in sp_ApproveorRejectAttendance_Result)
                            {
                                if (item.ReturnCode == true)
                                {
                                    if (request.Action == "Approved")
                                    {
                                        ApproveRejectAttendance.Status = true;
                                        ApproveRejectAttendance.Message = item.ReturnMessage;
                                        var entryPoint1 = (from ep in GInfraEntities.AttendanceLogsNewForMobiles


                                                           where ep.AttendanceLogId == l
                                                           select new
                                                           {
                                                               InTime = ep.InTime,
                                                               OutTime = ep.OutTime,
                                                               AttendanceDate = ep.AttendanceDate,

                                                               Duration = ep.Duration.ToString(),
                                                               EmployeeId = ep.EmployeeId.ToString(),


                                                           }).SingleOrDefault();
                                        if (entryPoint1.EmployeeId != "")
                                        {
                                            ApproveRejectODRegurizeMail ApproveRejectODRegurizeMail1 = new ApproveRejectODRegurizeMail();



                                            var detail1 = (from ep in GInfraEntities.sp_getheaddatabyuserid(entryPoint1.EmployeeId)



                                                           select new
                                                           {
                                                               UserName = ep.UserName ?? "-",
                                                               UserNameMail = ep.UserNameMail ?? "-",
                                                               ReportingHeadName = ep.ReportingHeadName ?? "-",
                                                               ReportingHeadMail = ep.ReportingHeadMail ?? "-",



                                                           }).SingleOrDefault();






                                            int k = ApproveRejectODRegurizeMail1.ApplyLeaveMail(detail1.UserNameMail, detail1.ReportingHeadMail, detail1.UserName, String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)));
                                            if (k == 1)
                                            {
                                                

                                                var gettoken = (from e in GInfraEntities.MasterPasswords
                                                                join a in GInfraEntities.AttendanceLogsNewForMobiles
                                                                on e.UserID equals a.EmployeeId
                                                                where a.AttendanceLogId == l



                                                                select new
                                                                {
                                                                    Devicetoken = e.DeviceToken,
                                                                    UserID = e.UserID,



                                                                }).SingleOrDefault();


                                                string DeviceToken = gettoken.Devicetoken;


                                              

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

                                                        string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Approved Attendance by Manager" + "\",\"Description\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + "\"}]}}";
                                                      

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
                                                                string json = "{\"to\": \"" + DeviceToken + "\",\"content_available\": true,\"notification\": { \"title\": \"AttendanceAction\",\"body\":\" " + "Approved Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"click_action\": \"fcm.ACTION.HELLO\"},\"data\": { \"extra\": \"juice\"}}";
                                                       

                                                        

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
                                                string datee = "Approved Attendance Dated : " + entryPoint1.AttendanceDate.ToString("dd-MM-yyyy");
                                                GInfraEntities.sp_notification(gettoken.UserID, "AttendanceAction", "Approved Attendance by Manager", datee);


                                            }


                                        }

                                    }
                                    else if (request.Action == "Rejected")
                                    {
                                        ApproveRejectAttendance.Status = true;
                                        ApproveRejectAttendance.Message = item.ReturnMessage;







                                        var entryPoint1 = (from ep in GInfraEntities.AttendanceLogsNewForMobiles


                                                           where ep.AttendanceLogId == l
                                                           select new
                                                           {
                                                               InTime = ep.InTime,
                                                               OutTime = ep.OutTime,
                                                               AttendanceDate = ep.AttendanceDate,

                                                               Duration = ep.Duration.ToString(),
                                                               EmployeeId = ep.EmployeeId.ToString(),


                                                           }).SingleOrDefault();
                                        if (entryPoint1.EmployeeId != "")
                                        {
                                            ApproveRejectODRegurizeMail ApproveRejectODRegurizeMail1 = new ApproveRejectODRegurizeMail();



                                            var detail1 = (from ep in GInfraEntities.sp_getheaddatabyuserid(entryPoint1.EmployeeId)



                                                           select new
                                                           {
                                                               UserName = ep.UserName ?? "-",
                                                               UserNameMail = ep.UserNameMail ?? "-",
                                                               ReportingHeadName = ep.ReportingHeadName ?? "-",
                                                               ReportingHeadMail = ep.ReportingHeadMail ?? "-",



                                                           }).SingleOrDefault();






                                            int k = ApproveRejectODRegurizeMail1.RejectODMail(detail1.UserNameMail, detail1.ReportingHeadMail, detail1.UserName, String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)));

                                            if (k == 1)
                                            {
                                                var gettoken = (from e in GInfraEntities.MasterPasswords
                                                                join a in GInfraEntities.AttendanceLogsNewForMobiles
                                                                on e.UserID equals a.EmployeeId
                                                                where a.AttendanceLogId == l



                                                                select new
                                                                {
                                                                    Devicetoken = e.DeviceToken,
                                                                    UserID = e.UserID,



                                                                }).SingleOrDefault();


                                                string DeviceToken = gettoken.Devicetoken;

                                               
                                                //  DeviceToken = "eAwEiiBJ5Tk:APA91bEz7zIqNf-Ly8-OQnm7mdSJKZhbCiCZ2EDmLM2GxtjcWvWpimQ-8fsiIbX3aBdrqysgBVXxbM7vlYGaXZ0SN9awkzUQ5hlkwnvl1nb0o75d2rNDOOEEANKFWmPUa06zFalI1ZBw";
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

                                                        string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"AttendanceAction\",\"json\":[{\"Title\":\"" + "Reject Attendance by Manager" + "\",\"Description\":\" " + "Rejected Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + "\"}]}}";
                                                        //   string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"HR_Announcement\",\"json\":[{\"Title\":\"" + "Reject Attendance by Manager" + "\",\"Description\":\" " + "Rejected Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"CreatedOn\":\"" + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + "\"}]}}";
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
                                                        string json = "{\"to\": \"" + DeviceToken + "\",\"content_available\": true,\"notification\": { \"title\": \"AttendanceAction\",\"body\":\" " + "Rejected Attendance Dated : " + String.Format("{0:dd-MM-yyyy}", Convert.ToDateTime(entryPoint1.AttendanceDate)) + " " + " \",\"click_action\": \"fcm.ACTION.HELLO\"},\"data\": { \"extra\": \"juice\"}}";

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
                                                string datee = "Rejected Attendance Dated : " + entryPoint1.AttendanceDate.ToString("dd-MM-yyyy");
                                                GInfraEntities.sp_notification(gettoken.UserID, "AttendanceAction", "Reject Attendance by Manager", datee);
                                            }

                                        }









                                    }


                                }
                                else
                                {
                                    ApproveRejectAttendance.Status = false;
                                    ApproveRejectAttendance.Message = item.ReturnMessage;
                                }

                            }



                            //  return Request.CreateResponse(HttpStatusCode.OK, ApproveRejectAttendance);




                        }


                        else
                        {
                            ApproveRejectAttendance.Status = false;
                            ApproveRejectAttendance.Message = "" + HttpStatusCode.BadRequest + "";
                            // return Request.CreateResponse(HttpStatusCode.OK, ApproveRejectAttendance);
                        }

                    }
                    return Request.CreateResponse(HttpStatusCode.OK, ApproveRejectAttendance);
                }


                else
                {
                    ApproveRejectAttendance.Status = false;
                    ApproveRejectAttendance.Message = "" + HttpStatusCode.BadRequest + "";
                    return Request.CreateResponse(HttpStatusCode.OK, ApproveRejectAttendance);
                }

            }
            catch (Exception ex)
            {
                ApproveRejectAttendance.Status = false;
                ApproveRejectAttendance.Message = "" + ex.Message + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ApproveRejectAttendance);
            }


        }
    }
}
