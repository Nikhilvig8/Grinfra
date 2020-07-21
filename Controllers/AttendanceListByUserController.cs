using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class AttendanceListByUserController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        string serverKey = "AAAASLs8D7Y:APA91bFYt4IFQRR4NPLNhX0SKzd_VQjxrvTE1mlS1rYk648fhrW3KvcejHCjwpjh9rHlLkSth7ewFN7ogmJ2mo7znvQXG1nXC0ny5cP14lz4rOM1FUOlGi-ZEdnkPW3kZxk9F15wTsPa";
        public HttpResponseMessage Post([FromBody]AttendanceListByUserRequest request)
        {
            string baseUrl = Url.Request.RequestUri.GetComponents(
    UriComponents.SchemeAndServer, UriFormat.Unescaped);

            AttendanceListByUser login = new AttendanceListByUser();

            if ((request.FromDate != "" && request.FromDate != null) && (request.ToDate != "" && request.ToDate != null))
            {
                try
                {
                    string temp = "";
                    if (request.LoginId != "" && request.LoginId != null)
                    {
                        var k = GInfraEntities.sp_Attendancelistbyuser(request.LoginId, request.FromDate, request.ToDate).ToList();
                        if (k.Count > 0)
                        {

                            foreach (var item in k)
                            {
                                AttendanceListByUserResponse LoginResponse = new AttendanceListByUserResponse();

                                LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                                LoginResponse.Designation = item.Designation;
                                LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                                LoginResponse.EmployeeId = item.EmployeeId.ToString();
                                LoginResponse.InTime = String.Format("{0:HH:mm}", Convert.ToDateTime(item.InTime));
                                LoginResponse.InImage = baseUrl + item.InImage.ToString();

                                LoginResponse.OutTime = String.Format("{0:HH:mm}", Convert.ToDateTime(item.OutTime));
                                if (item.Duration == null)
                                {
                                    LoginResponse.Duration = "-".ToString();
                                }
                                else
                                {
                                    LoginResponse.Duration = String.Format("{0:HH:mm}", Convert.ToDateTime(item.Duration));
                                }
                                if (item.AddressOut == null)
                                {
                                    LoginResponse.AddressOut = "-".ToString();
                                }
                                else
                                {
                                    LoginResponse.AddressOut = item.AddressOut.ToString();
                                }


                                LoginResponse.HeadId = item.HeadId.ToString();

                                if (item.AddressIn == null)
                                {
                                    LoginResponse.AddressIn = "null".ToString();
                                }
                                else
                                {
                                    LoginResponse.AddressIn = item.AddressIn.ToString();
                                }
                                LoginResponse.LattitudeIn = item.LattitudeIn.ToString();
                                LoginResponse.LongitudeIn = item.LongitudeIn.ToString();
                                if (item.LattitudeOut == null)
                                {
                                    LoginResponse.LattitudeOut = "null".ToString();
                                }
                                else
                                {
                                    LoginResponse.LattitudeOut = item.LattitudeOut.ToString();
                                }



                                if (item.ManagerRemarks == null)
                                {
                                    LoginResponse.Headremarks = "null".ToString();
                                }
                                else
                                {
                                    LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                                }
                                LoginResponse.MemberName = item.EmpName.ToString();
                                if (item.AttendanceStatus == "WO")
                                {
                                    LoginResponse.AttendanceStatus = "Week Off";
                                }
                                else
                                {
                                    LoginResponse.AttendanceStatus = item.AttendanceStatus.ToString();
                                }
                                if (item.MarkedByIn == item.EmployeeId)
                                {
                                    LoginResponse.MarkedByIn = "Marked by Itself";

                                }
                                else if (item.MarkedByIn == "-" || item.MarkedByIn == null)
                                {
                                    LoginResponse.MarkedByIn = "-";

                                }
                                else if (item.MarkedByIn != item.EmployeeId)
                                {
                                    LoginResponse.MarkedByIn = "Marked by Manager";

                                }
                                else
                                {
                                    LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                                }
                                //   LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                                if (item.MarkedByOut == item.EmployeeId)
                                {
                                    LoginResponse.MarkedByOut = "Marked by Itself";

                                }
                                else if (item.MarkedByOut == "-" || item.MarkedByOut == null)
                                {
                                    LoginResponse.MarkedByOut = "-";

                                }
                                else if (item.MarkedByOut != item.EmployeeId)
                                {
                                    LoginResponse.MarkedByOut = "Marked by Manager";

                                }
                                else
                                {
                                    LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                                }
                                //   LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                                LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                                LoginResponse.PolygonIn = item.PolygonIn.ToString();
                                LoginResponse.PolygonOut = item.PolygonOut.ToString();

                                
                                

                                login.Data.Add(LoginResponse);


                            }



                            login.Status = true;
                            login.Message = "Data Successfully Fetched";
                            return Request.CreateResponse(HttpStatusCode.OK, login);
                        }
                        else
                        {
                            login.Status = false;
                            login.Message = "No records Found";
                            return Request.CreateResponse(HttpStatusCode.OK, login);
                        }




                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "" + HttpStatusCode.BadRequest + "";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                }
                catch (Exception ex)
                {
                    login.Status = false;
                    login.Message = "" + ex + "";
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, login);
                }
            }


            else
            {
                try
                {
                    string temp = "";
                    if (request.LoginId != "" && request.LoginId != null)
                    {
                        var k = GInfraEntities.sp_Attendancelistbyuser(request.LoginId,"","").ToList();
                        if (k.Count > 0)
                        {

                            foreach (var item in k)
                            {
                                AttendanceListByUserResponse LoginResponse = new AttendanceListByUserResponse();
                                LoginResponse.Designation = item.Designation;
                                LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                                LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                                LoginResponse.EmployeeId = item.EmployeeId.ToString();
                                LoginResponse.InTime = String.Format("{0:HH:mm}", Convert.ToDateTime(item.InTime));
                                LoginResponse.InImage = baseUrl + item.InImage.ToString();

                                LoginResponse.OutTime = String.Format("{0:HH:mm}", Convert.ToDateTime(item.OutTime));
                                if (item.Duration == null)
                                {
                                    LoginResponse.Duration = "-".ToString();
                                }
                                else
                                {
                                    LoginResponse.Duration = String.Format("{0:HH:mm}", Convert.ToDateTime(item.Duration));
                                }
                                if (item.AddressOut == null)
                                {
                                    LoginResponse.AddressOut = "-".ToString();
                                }
                                else
                                {
                                    LoginResponse.AddressOut = item.AddressOut.ToString();
                                }


                                LoginResponse.HeadId = item.HeadId.ToString();

                                if (item.AddressIn == null)
                                {
                                    LoginResponse.AddressIn = "null".ToString();
                                }
                                else
                                {
                                    LoginResponse.AddressIn = item.AddressIn.ToString();
                                }
                                LoginResponse.LattitudeIn = item.LattitudeIn.ToString();
                                LoginResponse.LongitudeIn = item.LongitudeIn.ToString();
                                if (item.LattitudeOut == null)
                                {
                                    LoginResponse.LattitudeOut = "null".ToString();
                                }
                                else
                                {
                                    LoginResponse.LattitudeOut = item.LattitudeOut.ToString();
                                }

                              

                                
                                if (item.ManagerRemarks == null)
                                {
                                    LoginResponse.Headremarks = "null".ToString();
                                }
                                else
                                {
                                    LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                                }
                                LoginResponse.MemberName = item.EmpName.ToString();
                                if (item.AttendanceStatus == "WO")
                                {
                                    LoginResponse.AttendanceStatus = "Week Off";
                                }
                                else
                                {
                                    LoginResponse.AttendanceStatus = item.AttendanceStatus.ToString();
                                }
                                if (item.MarkedByIn == item.EmployeeId)
                                {
                                    LoginResponse.MarkedByIn = "Marked by Itself";

                                }
                                else if (item.MarkedByIn== "-" || item.MarkedByIn == null)
                                {
                                    LoginResponse.MarkedByIn = "-";

                                }
                                else if (item.MarkedByIn != item.EmployeeId)
                                {
                                    LoginResponse.MarkedByIn = "Marked by Manager";

                                }
                                else
                                {
                                    LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                                }
                                //   LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                                if (item.MarkedByOut == item.EmployeeId)
                                {
                                    LoginResponse.MarkedByOut = "Marked by Itself";

                                }
                                else if (item.MarkedByOut == "-" || item.MarkedByOut == null)
                                {
                                    LoginResponse.MarkedByOut = "-";

                                }
                                else if (item.MarkedByOut != item.EmployeeId)
                                {
                                    LoginResponse.MarkedByOut = "Marked by Manager";

                                }
                                else
                                {
                                    LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                                }
                                LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                                LoginResponse.PolygonIn = item.PolygonIn.ToString();
                                LoginResponse.PolygonOut = item.PolygonOut.ToString();
                                login.Data.Add(LoginResponse);


                            }



                            login.Status = true;
                            login.Message = "Data Successfully Fetched";
                            return Request.CreateResponse(HttpStatusCode.OK, login);
                        }
                        else
                        {
                            login.Status = false;
                            login.Message = "No records Found";
                            return Request.CreateResponse(HttpStatusCode.OK, login);
                        }




                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "" + HttpStatusCode.BadRequest + "";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
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
}