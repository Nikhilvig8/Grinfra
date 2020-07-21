using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class AttendanceListByHeadController : ApiController
    {

        GInfraEntities GInfraEntities = new GInfraEntities();
        string serverKey = "AAAASLs8D7Y:APA91bFYt4IFQRR4NPLNhX0SKzd_VQjxrvTE1mlS1rYk648fhrW3KvcejHCjwpjh9rHlLkSth7ewFN7ogmJ2mo7znvQXG1nXC0ny5cP14lz4rOM1FUOlGi-ZEdnkPW3kZxk9F15wTsPa";

        public HttpResponseMessage Post([FromBody]AttendanceListByHeadRequest request)
        {
            AttendanceListByHead login = new AttendanceListByHead();
            string baseUrl = Url.Request.RequestUri.GetComponents(
    UriComponents.SchemeAndServer, UriFormat.Unescaped);

            if (request.Status != null && request.Status != "" && (request.Employee == "" || request.Employee == null) && (request.FromDate == "" || request.FromDate == null) && (request.ToDate == "" || request.ToDate == null))
            {
                try
                {

                    string temp = "";
                    AttendanceListByHead AttendanceListByHead = new AttendanceListByHead();
                    var list = GInfraEntities.getAttendanceListByReportingHead(request.HeadId, "", "", "", "" + request.Status + "").ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            AttendanceListByHeadResponse LoginResponse = new AttendanceListByHeadResponse();
                            LoginResponse.Designation = item.Designation;
                            LoginResponse.InImage = baseUrl + item.InImage.ToString();
                            LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                            LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.EmployeeId = item.EmployeeId.ToString();
                            LoginResponse.InTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.InTime));
                            LoginResponse.DateName = String.Format("{0:MMM}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.OutTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.OutTime));
                            if (item.Duration == null)
                            {
                                LoginResponse.Duration = "-".ToString();
                            }
                            else
                            {
                                LoginResponse.Duration = String.Format("{0: HH:mm}", Convert.ToDateTime(item.Duration));
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
                            LoginResponse.AddressIn = item.AddressIn.ToString();
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


                            if (item.AttendanceStatus == "P")
                            {
                                temp = "Present";
                            }
                            else if (item.AttendanceStatus == "A")
                            {
                                temp = "Absent";
                            }
                            else if (item.AttendanceStatus == "R")
                            {
                                temp = "Rejected";
                            }
                            else
                            {
                                temp = item.AttendanceStatus;
                            }




                            LoginResponse.AttendanceStatus = temp.ToString();
                            if (item.ManagerRemarks == null)
                            {
                                LoginResponse.Headremarks = "null".ToString();
                            }
                            else
                            {
                                LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                            }
                            LoginResponse.MemberName = item.EmpName.ToString();


                            LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                            LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                            LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                            LoginResponse.PolygonIn = item.PolygonIn.ToString();
                            LoginResponse.PolygonOut = item.PolygonOut.ToString();


                            login.Data.Add(LoginResponse);
                        }
                        login.Status = true;
                        login.Message = "Data Fetched Successfully";
                        login.Search = "Only Status";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "No Record Found";
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


            else if (request.Status != null && request.Status != "" && (request.Employee != "" && request.Employee != null) && (request.FromDate == "" || request.FromDate == null) && (request.ToDate == "" || request.ToDate == null))
            {
                try
                {

                    string temp = "";
                    AttendanceListByHead AttendanceListByHead = new AttendanceListByHead();
                    var list = GInfraEntities.getAttendanceListByReportingHead(request.HeadId, ""+request.Employee+"", "", "", "" + request.Status + "").ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            AttendanceListByHeadResponse LoginResponse = new AttendanceListByHeadResponse();
                            LoginResponse.Designation = item.Designation;
                            LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                            LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                            LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                            LoginResponse.PolygonIn = item.PolygonIn.ToString();
                            LoginResponse.PolygonOut = item.PolygonOut.ToString();
                            LoginResponse.InImage = baseUrl + item.InImage.ToString();
                            LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                            LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.EmployeeId = item.EmployeeId.ToString();
                            LoginResponse.InTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.InTime));
                            LoginResponse.DateName = String.Format("{0:MMM}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.OutTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.OutTime));
                            if (item.Duration == null)
                            {
                                LoginResponse.Duration = "-".ToString();
                            }
                            else
                            {
                                LoginResponse.Duration = String.Format("{0: HH:mm}", Convert.ToDateTime(item.Duration));
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
                            LoginResponse.AddressIn = item.AddressIn.ToString();
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


                            if (item.AttendanceStatus == "P")
                            {
                                temp = "Present";
                            }
                            else if (item.AttendanceStatus == "A")
                            {
                                temp = "Absent";
                            }
                            else if (item.AttendanceStatus == "R")
                            {
                                temp = "Rejected";
                            }
                            else
                            {
                                temp = item.AttendanceStatus;
                            }




                            LoginResponse.AttendanceStatus = temp.ToString();
                            if (item.ManagerRemarks == null)
                            {
                                LoginResponse.Headremarks = "null".ToString();
                            }
                            else
                            {
                                LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                            }
                            LoginResponse.MemberName = item.EmpName.ToString();

                            login.Data.Add(LoginResponse);
                        }
                        login.Status = true;
                        login.Message = "Data Fetched Successfully";
                        login.Search = "Status with Employee";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "No Record Found";
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

            else if (request.Status != null && request.Status != "" && (request.Employee == "" || request.Employee == null) && (request.FromDate != "" && request.FromDate != null) && (request.ToDate != "" && request.ToDate != null))
            {
                try
                {

                    string temp = "";
                    AttendanceListByHead AttendanceListByHead = new AttendanceListByHead();
                    var list = GInfraEntities.getAttendanceListByReportingHead(request.HeadId, "", "" + request.FromDate + "", "" + request.ToDate + "", "" + request.Status + "").ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            AttendanceListByHeadResponse LoginResponse = new AttendanceListByHeadResponse();
                            LoginResponse.Designation = item.Designation;
                            LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                            LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                            LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                            LoginResponse.PolygonIn = item.PolygonIn.ToString();
                            LoginResponse.PolygonOut = item.PolygonOut.ToString();
                            LoginResponse.InImage = baseUrl + item.InImage.ToString();
                            LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                            LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.EmployeeId = item.EmployeeId.ToString();
                            LoginResponse.InTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.InTime));
                            LoginResponse.DateName = String.Format("{0:MMM}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.OutTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.OutTime));
                            if (item.Duration == null)
                            {
                                LoginResponse.Duration = "-".ToString();
                            }
                            else
                            {
                                LoginResponse.Duration = String.Format("{0: HH:mm}", Convert.ToDateTime(item.Duration));
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
                            LoginResponse.AddressIn = item.AddressIn.ToString();
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


                            if (item.AttendanceStatus == "P")
                            {
                                temp = "Present";
                            }
                            else if (item.AttendanceStatus == "A")
                            {
                                temp = "Absent";
                            }
                            else if (item.AttendanceStatus == "R")
                            {
                                temp = "Rejected";
                            }
                            else
                            {
                                temp = item.AttendanceStatus;
                            }




                            LoginResponse.AttendanceStatus = temp.ToString();
                            if (item.ManagerRemarks == null)
                            {
                                LoginResponse.Headremarks = "null".ToString();
                            }
                            else
                            {
                                LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                            }
                            LoginResponse.MemberName = item.EmpName.ToString();

                            login.Data.Add(LoginResponse);
                        }
                        login.Status = true;
                        login.Message = "Data Fetched Successfully";
                        login.Search = "Status with calender";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "No Record Found";
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

            else if (request.Status != null && request.Status != "" && (request.Employee != "" && request.Employee != null) && (request.FromDate != "" && request.FromDate != null) && (request.ToDate != "" && request.ToDate != null))
            {
                try
                {

                    string temp = "";
                    AttendanceListByHead AttendanceListByHead = new AttendanceListByHead();
                    var list = GInfraEntities.getAttendanceListByReportingHead(request.HeadId, "" + request.Employee + "", "" + request.FromDate + "", "" + request.ToDate + "", "" + request.Status + "").ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            AttendanceListByHeadResponse LoginResponse = new AttendanceListByHeadResponse();
                            LoginResponse.Designation = item.Designation;
                            LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                            LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                            LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                            LoginResponse.PolygonIn = item.PolygonIn.ToString();
                            LoginResponse.PolygonOut = item.PolygonOut.ToString();
                            LoginResponse.InImage = baseUrl + item.InImage.ToString();
                            LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                            LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.EmployeeId = item.EmployeeId.ToString();
                            LoginResponse.InTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.InTime));
                            LoginResponse.DateName = String.Format("{0:MMM}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.OutTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.OutTime));
                            if (item.Duration == null)
                            {
                                LoginResponse.Duration = "-".ToString();
                            }
                            else
                            {
                                LoginResponse.Duration = String.Format("{0: HH:mm}", Convert.ToDateTime(item.Duration));
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
                            LoginResponse.AddressIn = item.AddressIn.ToString();
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


                            if (item.AttendanceStatus == "P")
                            {
                                temp = "Present";
                            }
                            else if (item.AttendanceStatus == "A")
                            {
                                temp = "Absent";
                            }
                            else if (item.AttendanceStatus == "R")
                            {
                                temp = "Rejected";
                            }
                            else
                            {
                                temp = item.AttendanceStatus;
                            }




                            LoginResponse.AttendanceStatus = temp.ToString();
                            if (item.ManagerRemarks == null)
                            {
                                LoginResponse.Headremarks = "null".ToString();
                            }
                            else
                            {
                                LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                            }
                            LoginResponse.MemberName = item.EmpName.ToString();

                            login.Data.Add(LoginResponse);
                        }
                        login.Status = true;
                        login.Message = "Data Fetched Successfully";
                        login.Search = "All";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "No Record Found";
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



            else if (request.Employee != null && request.Employee != "" && (request.Status == "" || request.Status == null) && (request.FromDate == "" || request.FromDate == null) && (request.ToDate == "" || request.ToDate == null))
            {
                try
                {

                    string temp = "";
                    AttendanceListByHead AttendanceListByHead = new AttendanceListByHead();
                    var list = GInfraEntities.getAttendanceListByReportingHead(request.HeadId, "" + request.Employee + "", "", "", "").ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            AttendanceListByHeadResponse LoginResponse = new AttendanceListByHeadResponse();
                            LoginResponse.Designation = item.Designation;
                            LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                            LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                            LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                            LoginResponse.PolygonIn = item.PolygonIn.ToString();
                            LoginResponse.PolygonOut = item.PolygonOut.ToString();
                            LoginResponse.InImage = baseUrl + item.InImage.ToString();
                            LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                            LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.EmployeeId = item.EmployeeId.ToString();
                            LoginResponse.InTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.InTime));
                            LoginResponse.DateName = String.Format("{0:MMM}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.OutTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.OutTime));
                            if (item.Duration == null)
                            {
                                LoginResponse.Duration = "-".ToString();
                            }
                            else
                            {
                                LoginResponse.Duration = String.Format("{0: HH:mm}", Convert.ToDateTime(item.Duration));
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
                            LoginResponse.AddressIn = item.AddressIn.ToString();
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


                            if (item.AttendanceStatus == "P")
                            {
                                temp = "Present";
                            }
                            else if (item.AttendanceStatus == "A")
                            {
                                temp = "Absent";
                            }
                            else if (item.AttendanceStatus == "R")
                            {
                                temp = "Rejected";
                            }
                            else
                            {
                                temp = item.AttendanceStatus;
                            }




                            LoginResponse.AttendanceStatus = temp.ToString();
                            if (item.ManagerRemarks == null)
                            {
                                LoginResponse.Headremarks = "null".ToString();
                            }
                            else
                            {
                                LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                            }
                            LoginResponse.MemberName = item.EmpName.ToString();

                            login.Data.Add(LoginResponse);
                        }
                        login.Status = true;
                        login.Message = "Data Fetched Successfully";
                        login.Search = "Only Employee";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "No Record Found";
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
            else if ((request.Employee == null || request.Employee == "") && (request.Status == "" || request.Status == null) && (request.FromDate != "" && request.FromDate != null) && (request.ToDate != "" && request.ToDate != null))
            {
                try
                {
                    string temp = "";
                    AttendanceListByHead AttendanceListByHead = new AttendanceListByHead();
                    var list = GInfraEntities.getAttendanceListByReportingHead(request.HeadId, "", "" + request.FromDate + "", "" + request.ToDate + "", "").ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            AttendanceListByHeadResponse LoginResponse = new AttendanceListByHeadResponse();
                            LoginResponse.Designation = item.Designation;
                            LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                            LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                            LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                            LoginResponse.PolygonIn = item.PolygonIn.ToString();
                            LoginResponse.PolygonOut = item.PolygonOut.ToString();
                            LoginResponse.InImage = baseUrl + item.InImage.ToString();
                            LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                            LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.EmployeeId = item.EmployeeId.ToString();
                            LoginResponse.InTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.InTime));
                            LoginResponse.DateName = String.Format("{0:MMM}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.OutTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.OutTime));
                            if (item.Duration == null)
                            {
                                LoginResponse.Duration = "-".ToString();
                            }
                            else
                            {
                                LoginResponse.Duration = String.Format("{0: HH:mm}", Convert.ToDateTime(item.Duration));
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
                            LoginResponse.AddressIn = item.AddressIn.ToString();
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


                            if (item.AttendanceStatus == "P")
                            {
                                temp = "Present";
                            }
                            else if (item.AttendanceStatus == "A")
                            {
                                temp = "Absent";
                            }
                            else if (item.AttendanceStatus == "R")
                            {
                                temp = "Rejected";
                            }
                            else
                            {
                                temp = item.AttendanceStatus;
                            }




                            LoginResponse.AttendanceStatus = temp.ToString();
                            if (item.ManagerRemarks == null)
                            {
                                LoginResponse.Headremarks = "null".ToString();
                            }
                            else
                            {
                                LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                            }
                            LoginResponse.MemberName = item.EmpName.ToString();

                            login.Data.Add(LoginResponse);
                        }
                        login.Status = true;
                        login.Message = "Data Fetched Successfully";
                        login.Search = "Only Calender";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "No Record Found";
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
            else if ((request.Employee != null && request.Employee != "") && (request.Status == "" || request.Status == null) && (request.FromDate != "" && request.FromDate != null) && (request.ToDate != "" && request.ToDate != null))
            {
                try
                {
                    string temp = "";
                    AttendanceListByHead AttendanceListByHead = new AttendanceListByHead();
                    var list = GInfraEntities.getAttendanceListByReportingHead(request.HeadId, "" + request.Employee + "", "" + request.FromDate + "", "" + request.ToDate + "", "").ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            AttendanceListByHeadResponse LoginResponse = new AttendanceListByHeadResponse();
                            LoginResponse.Designation = item.Designation;
                            LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                            LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                            LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                            LoginResponse.PolygonIn = item.PolygonIn.ToString();
                            LoginResponse.PolygonOut = item.PolygonOut.ToString();
                            LoginResponse.InImage = baseUrl + item.InImage.ToString();
                            LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                            LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.EmployeeId = item.EmployeeId.ToString();
                            LoginResponse.InTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.InTime));
                            LoginResponse.DateName = String.Format("{0:MMM}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.OutTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.OutTime));
                            if (item.Duration == null)
                            {
                                LoginResponse.Duration = "-".ToString();
                            }
                            else
                            {
                                LoginResponse.Duration = String.Format("{0: HH:mm}", Convert.ToDateTime(item.Duration));
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
                            LoginResponse.AddressIn = item.AddressIn.ToString();
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


                            if (item.AttendanceStatus == "P")
                            {
                                temp = "Present";
                            }
                            else if (item.AttendanceStatus == "A")
                            {
                                temp = "Absent";
                            }
                            else if (item.AttendanceStatus == "R")
                            {
                                temp = "Rejected";
                            }
                            else
                            {
                                temp = item.AttendanceStatus;
                            }




                            LoginResponse.AttendanceStatus = temp.ToString();
                            if (item.ManagerRemarks == null)
                            {
                                LoginResponse.Headremarks = "null".ToString();
                            }
                            else
                            {
                                LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                            }
                            LoginResponse.MemberName = item.EmpName.ToString();

                            login.Data.Add(LoginResponse);
                        }
                        login.Status = true;
                        login.Message = "Data Fetched Successfully";
                        login.Search = "Employee With Calender";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "No Record Found";
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
                    AttendanceListByHead AttendanceListByHead = new AttendanceListByHead();
                    var list = GInfraEntities.getAttendanceListByReportingHead(request.HeadId, "", "", "", "").ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            AttendanceListByHeadResponse LoginResponse = new AttendanceListByHeadResponse();
                            LoginResponse.Designation = item.Designation;
                            LoginResponse.MarkedByIn = item.MarkedByIn.ToString();
                            LoginResponse.MarkedByOut = item.MarkedByOut.ToString();
                            LoginResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();
                            LoginResponse.PolygonIn = item.PolygonIn.ToString();
                            LoginResponse.PolygonOut = item.PolygonOut.ToString();
                            LoginResponse.InImage = baseUrl + item.InImage.ToString();
                            LoginResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                            LoginResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.EmployeeId = item.EmployeeId.ToString();
                            LoginResponse.InTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.InTime));
                            LoginResponse.DateName = String.Format("{0:MMM}", Convert.ToDateTime(item.AttendanceDate));
                            LoginResponse.OutTime = String.Format("{0: HH:mm}", Convert.ToDateTime(item.OutTime));
                            if (item.Duration == null)
                            {
                                LoginResponse.Duration = "-".ToString();
                            }
                            else
                            {
                                LoginResponse.Duration = String.Format("{0: HH:mm}", Convert.ToDateTime(item.Duration));
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
                            LoginResponse.AddressIn = item.AddressIn.ToString();
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


                            if (item.AttendanceStatus == "P")
                            {
                                temp = "Present";
                            }
                            else if (item.AttendanceStatus == "A")
                            {
                                temp = "Absent";
                            }
                            else if (item.AttendanceStatus == "A1")
                            {
                                temp = "First Half";
                            }
                            else if (item.AttendanceStatus == "A2")
                            {
                                temp = "Second Half";
                            }
                            else if (item.AttendanceStatus == "L")
                            {
                                temp = "Leave";
                            }
                            else if (item.AttendanceStatus == "R")
                            {
                                temp = "Rejected OD";
                            }
                            else
                            {
                                temp = item.AttendanceStatus;
                            }




                            LoginResponse.AttendanceStatus = temp.ToString();
                            if (item.ManagerRemarks == null)
                            {
                                LoginResponse.Headremarks = "null".ToString();
                            }
                            else
                            {
                                LoginResponse.Headremarks = item.ManagerRemarks.ToString();
                            }
                            LoginResponse.MemberName = item.EmpName.ToString();

                            login.Data.Add(LoginResponse);
                        }
                        login.Status = true;
                        login.Message = "Data Fetched Successfully";
                        login.Search = "Main";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "No Record Found";
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