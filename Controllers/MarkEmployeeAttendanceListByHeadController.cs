using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class MarkEmployeeAttendanceListByHeadController : ApiController
    {

        GInfraEntities GInfraEntities = new GInfraEntities();


        public HttpResponseMessage Post([FromBody]MarkEmployeeAttendanceListByHeadRequest request)
        {
            MarkEmployeeAttendanceListByHead login = new MarkEmployeeAttendanceListByHead();

            string baseUrl = Url.Request.RequestUri.GetComponents(
   UriComponents.SchemeAndServer, UriFormat.Unescaped);

            try
            {
                if (request.HeadID != null && request.HeadID != "")
                {


                    var list = GInfraEntities.getuserattendancebyhead(request.HeadID).ToList();
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            MarkEmployeeAttendanceListByHeadResponse AppversionResponse = new MarkEmployeeAttendanceListByHeadResponse();
                            if(item.InImage!="-")
                            {
                                AppversionResponse.InImage = baseUrl + item.InImage.ToString();
                            }
                            else
                            {
                                AppversionResponse.InImage = item.InImage.ToString();
                            }
                            
                            AppversionResponse.employeename = item.EmpName.ToString();
                            AppversionResponse.employeeid = item.EmployeeId.ToString();
                            AppversionResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                            AppversionResponse.AttendanceDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.AttendanceDate)); 
                            AppversionResponse.Designation = item.Designation.ToString();
                            AppversionResponse.InTime = String.Format("{0:HH:mm}", Convert.ToDateTime(item.InTime));
                            AppversionResponse.OutTime = String.Format("{0:HH:mm}", Convert.ToDateTime(item.OutTime));
                            AppversionResponse.Duration = String.Format("{0:HH:mm}", Convert.ToDateTime(item.Duration));
                            AppversionResponse.HeadId = item.HeadId.ToString();
                            AppversionResponse.AddressIn = item.AddressIn.ToString();
                            AppversionResponse.LattitudeIn = item.LattitudeIn.ToString();
                            AppversionResponse.LongitudeIn = item.LongitudeIn.ToString();
                            AppversionResponse.AddressOut = item.AddressOut.ToString();
                            AppversionResponse.LattitudeOut = item.LattitudeOut.ToString();
                            AppversionResponse.LongitudeOut = item.LongitudeOut.ToString();
                            AppversionResponse.AttendanceStatus = item.AttendanceStatus.ToString();
                            AppversionResponse.Modeofpunchin = item.Modeofpunchin.ToString();
                            AppversionResponse.ModeofpunchOut = item.ModeofpunchOut.ToString();


                            login.Data.Add(AppversionResponse);

                        }
                        login.Status = true;
                        login.Message = "Data Fetched Successfully";
                        
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }
                    else
                    {
                        login.Status = false;
                        login.Message = "No Record Found";
                        return Request.CreateResponse(HttpStatusCode.OK, login);
                    }

                }
                else
                {
                    login.Status = false;
                    login.Message = "Bad Request";
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
