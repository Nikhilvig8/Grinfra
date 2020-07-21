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
    public class AttendenceOutController : ApiController
    {

       

        // GET: api/Login

        // POST: api/Login
        [HttpPost]
        public async Task<HttpResponseMessage> PostFormData()
        {
            GInfraEntities GInfraEntities = new GInfraEntities();
            double Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            AttendenceOutRequest request = new AttendenceOutRequest();
            AttendenceIn login = new AttendenceIn();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

          //  string root = HttpContext.Current.Server.MapPath("~/App_Data");
            //var provider = new MultipartFormDataStreamProvider(root);
            var provider = await Request.Content.ReadAsMultipartAsync<InMemoryMultipartFormDataStreamProvider>(new InMemoryMultipartFormDataStreamProvider());

            try
            {
                
                //access form data  
                NameValueCollection formData = provider.FormData;
                //access files  
                IList<HttpContent> files = provider.Files;

                //HttpContent file1 = files[0];
                //var thisFileName = file1.Headers.ContentDisposition.FileName.Trim('\"');
                //string extension = Path.GetExtension(thisFileName);
                //thisFileName = ""; 
                
                //thisFileName = formData["UserId"] + "_PunchOut_" + DateTime.Now.ToString("yyyy-MMM-dd") + extension;
                //string filename = String.Empty;
                //Stream input = await file1.ReadAsStreamAsync();
                //string directoryName = String.Empty;
                //string URL = String.Empty;
                //string tempDocUrl = WebConfigurationManager.AppSettings["DocsUrl"];

                //var path = HttpRuntime.AppDomainAppPath;
                //directoryName = System.IO.Path.Combine(path, "ClientDocument");
                //filename = System.IO.Path.Combine(directoryName, thisFileName);
                //string filepath1 = "/" + "ClientDocument" + "/" + thisFileName;
                ////Deletion exists file  
                ////if (File.Exists(filename))
                ////{
                ////    File.Delete(filename);
                ////}

                //string DocsPath = tempDocUrl + "/" + "ClientDocument" + "/";
                //URL = DocsPath + thisFileName;


               
                //   formData["Name"];
                // await Request.Content.ReadAsMultipartAsync(provider);
                


                request.UserId = formData["UserId"];
                request.OutTime = formData["OutTime"];
                request.AttendanceStatus = formData["AttendanceStatus"];
                request.AddressOut = formData["AddressOut"];
                request.LattitudeOut = formData["LattitudeOut"];
                request.LongitudeOut = formData["LongitudeOut"];
                request.AttendanceLogId = formData["AttendanceLogId"];
                request.mode = formData["mode"];
                request.PolygonOut = formData["PolygonOut"];
                request.action = formData["action"];
                //   request.Filee = filepath1.ToString();
                if (request.LattitudeOut != "" && request.LattitudeOut != null)
                {





                   DateTime dateTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                    var k = GInfraEntities.InsertAttendanceOut(request.UserId, request.OutTime, request.AttendanceStatus, request.AddressOut, request.LattitudeOut, request.LongitudeOut, request.AttendanceLogId.ToString(), request.mode,request.PolygonOut,request.action).ToList();
                    foreach (var item in k)
                    {
                        if (item.returncode == "true")
                        {
                            //Directory.CreateDirectory(@directoryName);  
                            //using (Stream file = File.OpenWrite(filename))
                            //{
                            //    input.CopyTo(file);
                            //    //close file  
                            //    file.Close();
                            //}

                          
                            var result = (from p in GInfraEntities.AttendanceLogsNewForMobiles
                                          where p.EmployeeId == request.UserId && p.AttendanceDate == dateTime
                                          select new
                                          {
                                              p.AttendanceLogId,
                                              p.InTime,
                                              p.AttendanceStatus,
                                              p.OutTime,

                                          }).FirstOrDefault();
                            login.Status = true;

                            //   string outtime = result.InTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            login.InTime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(result.InTime));
                            login.OutTime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(result.OutTime));
                            login.AttendanceLogId = result.AttendanceLogId;
                            login.AttendanceStatus = result.AttendanceStatus;
                            login.Message = "Attendance Punch Out Successfully";
                            try
                            {
                                if (request.PolygonOut == "Out")
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

                                    string DeviceUserToken = getusertoken.Devicetoken;
                                    string DeviceToken = gettoken.Devicetoken;
                                    string usermessage = "You have punched Out outside the polygon.";
                                    string headmessage = "Your team member " + request.UserId + " has punched Out outside the polygon.";

                                    // DeviceToken = "eNkwJf36cSI:APA91bFeOCPQNRznvSk3lEqJZpCySl14Tvkafh2zjKa4zBF7cBquMZkMsD4L8OKLAt8PgsuDlztI8NfozEmUXGUigUjkdYFQDycIvN1UiG0LyM2b_D1VteRQ-0AVhIlVPT_aZG-nnXnx";
                                    string serverKey = "AAAASLs8D7Y:APA91bFYt4IFQRR4NPLNhX0SKzd_VQjxrvTE1mlS1rYk648fhrW3KvcejHCjwpjh9rHlLkSth7ewFN7ogmJ2mo7znvQXG1nXC0ny5cP14lz4rOM1FUOlGi-ZEdnkPW3kZxk9F15wTsPa";
                                    // string serverKey = "AAAA9-eygYA:APA91bE55q_BzvUTXY4cjjtaePYvI8-U1aq0Vqe8MnVZhsa4l1s0K7bYXyMa7puQNHP9OA8H6LzvpS3yXHWEjPbEnR8FH75TyE5Qcqi9n56cPN9yrkogUWlIoQHicelhlVpheMbu21-I";
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



                                    //   head
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
                                    //  user
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
                                    GInfraEntities.sp_notification(request.UserId, "PolygonStatus", usermessage,usermessage);
                                }
                            }
                            catch (Exception e)
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
                                              p.AttendanceStatus,
                                              p.OutTime,

                                          }).FirstOrDefault();
                            login.Status = false;


                            login.InTime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(result.InTime));
                            login.OutTime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(result.OutTime));
                            login.AttendanceLogId = result.AttendanceLogId;
                            login.AttendanceStatus = result.AttendanceStatus;
                            login.Message = "User already punch out";


                        }
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, login);

                }
                else
                {
                   // File.Delete(filename);
                    login.Status = false;
                    login.Message = "" + HttpStatusCode.OK + "";
                    return Request.CreateResponse(HttpStatusCode.OK, login);
                }
                // return Request.CreateResponse(HttpStatusCode.InternalServerError, login);
            }
            catch (Exception ex)
            {
                login.Status = false;
                login.Message = "" + ex + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, login);
            }

            
        }
       





    }
}