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
    public class AttendanceStatusListController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Get()
        {

            AttendanceStatusList AttendanceStatusList = new AttendanceStatusList();

            try
            {

                var list = GInfraEntities.attendancestatusmasters.Where(m => m.isactive == true).ToList();
                if (list.Count > 0)
                {
                    AttendanceStatusListResponse AttendanceStatusListResponse = new AttendanceStatusListResponse();

                    AttendanceStatusListResponse.AttendanceStatus = "Select Status";

                    AttendanceStatusList.Data.Add(AttendanceStatusListResponse);
                    foreach (var k in list)
                    {
                         AttendanceStatusListResponse = new AttendanceStatusListResponse();

                        AttendanceStatusListResponse.AttendanceStatus = k.Status.ToString();





                        AttendanceStatusList.Data.Add(AttendanceStatusListResponse);
                    }
                    AttendanceStatusList.Status = true;
                    AttendanceStatusList.Message = "Data Fetched Successfully";
                    return Request.CreateResponse(HttpStatusCode.OK, AttendanceStatusList);
                }
                else
                {
                    AttendanceStatusList.Status = false;
                    AttendanceStatusList.Message = "No Record Found";
                    return Request.CreateResponse(HttpStatusCode.OK, AttendanceStatusList);
                }
            }
            catch (Exception ex)
            {
                AttendanceStatusList.Status = false;
                AttendanceStatusList.Message = "" + ex + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, AttendanceStatusList);
            }

        }
    }
}