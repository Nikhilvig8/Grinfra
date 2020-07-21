using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class AttendenceInController : ApiController
    {
       



        [HttpPost]
        public async Task<HttpResponseMessage> PostFormData()
        {
            string foldername = DateTime.Now.ToString("dd-MM-yyyy");
            GInfraEntities GInfraEntities = new GInfraEntities();
            double Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            AttendenceInRequest request = new AttendenceInRequest();
            AttendenceIn login = new AttendenceIn();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());

            try
            {

                //access form data  
                NameValueCollection formData = provider.FormData;




                //access files  
                IList<HttpContent> files = provider.Files;
                if (files.Count > 0)
                {
                    HttpContent file1 = files[0];
                    var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');
                    string extension = Path.GetExtension(thisFileName);
                    thisFileName = "";

                    thisFileName = formData["UserId"] + "_PunchIN_" + DateTime.Now.ToString("yyyy-MMM-dd") + extension;
                    string filename = String.Empty;
                    Stream input = await file1.ReadAsStreamAsync();
                    string directoryName = String.Empty;
                    string URL = String.Empty;
                    string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

                    var path = HttpRuntime.AppDomainAppPath;
                    directoryName = System.IO.Path.Combine(path, "ClientDocument\\" + foldername + "\\");
                    filename = System.IO.Path.Combine(directoryName, thisFileName);
                    string filepath1 = "/" + "ClientDocument" + "/" + foldername + "/" + thisFileName;
                    //Deletion exists file  
                    //if (File.Exists(filename))
                    //{
                    //    File.Delete(filename);
                    //}

                    string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/" + foldername + "/";
                    URL = DocsPath + thisFileName;



                    //   formData["Name"];
                    // await Request.Content.ReadAsMultipartAsync(provider);



                    request.AddressIn = formData["AddressIn"];
                    request.UserId = formData["UserId"];
                    request.HeadId = formData["HeadId"];
                    request.InTime = formData["InTime"];
                    request.AddressIn = formData["AddressIn"];
                    request.LattitudeIn = formData["LattitudeIn"];
                    request.LongitudeIn = formData["LongitudeIn"];
                    request.mode = formData["mode"];
                    request.PolygonIn = formData["PolygonIn"];
                    request.Filee = filepath1.ToString();
                    request.action = formData["action"].ToString();
                    if (request.InTime != "" && request.InTime != null && request.UserId != "" && request.UserId != null && request.PolygonIn != "" && request.PolygonIn != null
                  && request.LattitudeIn != "" && request.LattitudeIn != null && request.LongitudeIn != "" && request.LongitudeIn != null
                   )
                    //if (request.InTime != "" && request.InTime != null)
                    {





                        DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        var k = GInfraEntities.InsertAttendanceIn(request.UserId, request.InTime, request.HeadId, request.AddressIn, request.LattitudeIn, request.LongitudeIn, "Pending", request.mode, request.Filee, request.PolygonIn,request.action).ToList();
                        foreach (var item in k)
                        {
                            if (item.returncode == "true")
                            {

                                Directory.CreateDirectory(@directoryName);
                                using (Stream file = File.OpenWrite(filename))
                                {
                                    input.CopyTo(file);
                                    //close file  
                                    file.Close();
                                }

                                var result = (from p in GInfraEntities.AttendanceLogsNewForMobiles
                                              where p.EmployeeId == request.UserId && p.AttendanceDate == dateTime
                                              select new
                                              {
                                                  p.AttendanceLogId,
                                                  p.InTime,
                                                  p.AttendanceStatus
                                              }).FirstOrDefault();
                                login.AttendanceLogId = result.AttendanceLogId;
                                login.InTime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(result.InTime));
                                login.OutTime = "";
                                login.AttendanceStatus = result.AttendanceStatus;
                                login.AttendanceLogId = result.AttendanceLogId;
                                //login.Data.Add(AttendenceInResponse);
                                login.Status = true;
                                login.Message = "Attendance Punch in Successfully";
                                string usermessage = "You have punched In outside the polygon.";
                                string headmessage = "Your team member " + request.UserId + " has punched In outside the polygon.";
                                try
                                {
                                    if (request.PolygonIn == "Out")
                                    {
                                        var headid = (from a in GInfraEntities.EmployeeMasters
                                                      join b in GInfraEntities.EmployeeMasters
                                                      on a.MangerID equals b.EmployeeId
                                                      where a.EmployeeId == request.UserId



                                                      select new
                                                      {
                                                          headid = b.EmployeeId,




                                                      }).SingleOrDefault();
                                       // string managerid = headid.ToString();
                                        var gettoken = (from e in GInfraEntities.MasterPasswords
                                                        
                                                        where e.UserID == headid.headid



                                                        select new
                                                        {
                                                            Devicetoken = e.DeviceToken,



                                                        }).SingleOrDefault();
                                        var getusertoken = (from e in GInfraEntities.MasterPasswords

                                                        where e.UserID == request.UserId



                                                        select new
                                                        {
                                                            Devicetoken = e.DeviceToken,



                                                        }).SingleOrDefault();

                                        string DeviceToken = gettoken.Devicetoken;
                                        string DeviceUserToken= getusertoken.Devicetoken;

                                        // DeviceToken = "eNkwJf36cSI:APA91bFeOCPQNRznvSk3lEqJZpCySl14Tvkafh2zjKa4zBF7cBquMZkMsD4L8OKLAt8PgsuDlztI8NfozEmUXGUigUjkdYFQDycIvN1UiG0LyM2b_D1VteRQ-0AVhIlVPT_aZG-nnXnx";
                                        string serverKey = "AAAASLs8D7Y:APA91bFYt4IFQRR4NPLNhX0SKzd_VQjxrvTE1mlS1rYk648fhrW3KvcejHCjwpjh9rHlLkSth7ewFN7ogmJ2mo7znvQXG1nXC0ny5cP14lz4rOM1FUOlGi-ZEdnkPW3kZxk9F15wTsPa";
                                        //string serverKey = "AAAA9-eygYA:APA91bE55q_BzvUTXY4cjjtaePYvI8-U1aq0Vqe8MnVZhsa4l1s0K7bYXyMa7puQNHP9OA8H6LzvpS3yXHWEjPbEnR8FH75TyE5Qcqi9n56cPN9yrkogUWlIoQHicelhlVpheMbu21-I";

                                        //head
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
                                                string json = "{\"to\": \"" + DeviceToken + "\",\"data\": {\"type\": \"PolygonManager\",\"json\":{\"Title\":\""+headmessage+"\",\"body\":\""+headmessage+"\",\"date\":\"\"}}}";
                                                

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
                                        catch (Exception ex)
                                        {

                                            var msg = new HttpResponseMessage(HttpStatusCode.NotImplemented) { ReasonPhrase = "Error While Notification" };

                                        }
                                        //user
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
                                                
                                                   
                                                string json = "{\"to\": \"" + DeviceUserToken + "\",\"data\": {\"type\": \"PolygonStatus\",\"json\":{\"Title\":\""+usermessage+"\",\"body\":\""+usermessage+"\",\"date\":\"\"}}}";
                                              

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
                                        catch (Exception ex)
                                        {

                                            var msg = new HttpResponseMessage(HttpStatusCode.NotImplemented) { ReasonPhrase = "Error While Notification" };

                                        }
                                        //head
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
                                                string json = "{\"to\": \"" + DeviceToken + "\",\"content_available\": true,\"notification\": { \"title\": \"PolygonManager\",\"body\": \""+headmessage+"\",\"click_action\": \"fcm.ACTION.HELLO\"},\"data\": { \"extra\": \"juice\"}}";
                                                

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
                                        catch (Exception ex)
                                        {

                                            var msg = new HttpResponseMessage(HttpStatusCode.NotImplemented) { ReasonPhrase = "Error While Notification" };

                                        }
                                        ////user
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
                                                string json = "{\"to\": \"" + DeviceUserToken + "\",\"content_available\": true,\"notification\": { \"title\": \"PolygonStatus\",\"body\": \""+usermessage+"\",\"click_action\": \"fcm.ACTION.HELLO\"},\"data\": { \"extra\": \"juice\"}}";
                                               

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
                                        catch (Exception ex)
                                        {

                                            var msg = new HttpResponseMessage(HttpStatusCode.NotImplemented) { ReasonPhrase = "Error While Notification" };

                                        }



                                        
                                       
                                        GInfraEntities.sp_notification(headid.headid, "PolygonManager", headmessage, headmessage);
                                        GInfraEntities.sp_notification(request.UserId, "PolygonStatus", usermessage, usermessage);
                                    }
                                }
                                catch(Exception e)
                                {

                                }
                            }
                            if (item.returncode == "false")
                            {
                                var result = (from p in GInfraEntities.AttendanceLogsNewForMobiles
                                              where p.EmployeeId == request.UserId && p.AttendanceDate == dateTime
                                              select new
                                              {
                                                  p.AttendanceLogId,
                                                  p.InTime,
                                                  p.AttendanceStatus
                                              }).FirstOrDefault();
                                login.AttendanceLogId = result.AttendanceLogId;
                                login.InTime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(result.InTime));
                                login.OutTime = "";
                                login.AttendanceStatus = result.AttendanceStatus;
                                login.AttendanceLogId = result.AttendanceLogId;
                                //login.Data.Add(AttendenceInResponse);
                                login.Status = false;
                                login.Message = "User already Punch in";


                            }
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, login);

                    }
                    else
                    {
                        //File.Delete(filename);
                        login.Status = false;
                        login.Message = "" + HttpStatusCode.BadRequest + "";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    // return Request.CreateResponse(HttpStatusCode.InternalServerError, login);
                }
                else
                {

                    login.Status = false;
                    login.Message = "Image File is missing";
                    return Request.CreateResponse(HttpStatusCode.OK, login);
                }
            }



            catch (Exception ex)
            {
                login.Status = false;
                login.Message = "Can't punch in, as your shift hasn't started yet";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, login);
            }


        }






    }
}