using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class CheckCurrentStatusController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]CheckCurrentStatusRequest request)
        {

            CheckCurrentStatus CheckCurrentStatus = new CheckCurrentStatus();
            try
            {

                if (request.authtoken != "" && request.authtoken != null)
                {




                    List<sp_checkcurrentstatus_Result> sp_checkcurrentstatus_Result = new List<sp_checkcurrentstatus_Result>();

                    sp_checkcurrentstatus_Result = GInfraEntities.sp_checkcurrentstatus(request.authtoken).ToList();
                    if (sp_checkcurrentstatus_Result.Count > 0)
                    {

                        foreach (var item in sp_checkcurrentstatus_Result)
                        {
                            string d = item.InTime.ToString();
                            CheckCurrentStatusResponse CheckCurrentStatusResponse = new CheckCurrentStatusResponse();

                            CheckCurrentStatusResponse.intime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(item.InTime));
                            CheckCurrentStatusResponse.outtime = String.Format("{0:yyyy/MM/dd HH:mm:ss}", Convert.ToDateTime(item.OutTime));
                            CheckCurrentStatusResponse.AttendanceLogId = item.AttendanceLogId.ToString();
                           // CheckCurrentStatusResponse.Status= item.att.ToString();
                            CheckCurrentStatus.Data.Add(CheckCurrentStatusResponse);
                        }
                        CheckCurrentStatus.Status = true;
                        CheckCurrentStatus.Message = "Data Fectched successfully";
                        return Request.CreateResponse(HttpStatusCode.OK, CheckCurrentStatus);
                    }
                    else
                    {
                        CheckCurrentStatus.Status = false;
                        CheckCurrentStatus.Message = "No Data Found";
                        return Request.CreateResponse(HttpStatusCode.OK, CheckCurrentStatus);
                    }

                }


                return Request.CreateResponse(HttpStatusCode.OK, CheckCurrentStatus);
            }
            catch (Exception ex)
            {
                CheckCurrentStatus.Status = false;
                CheckCurrentStatus.Message = "" + ex.Message + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, CheckCurrentStatus);
            }
        }
    }
}