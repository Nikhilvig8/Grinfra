using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class DeleteBellNotificationController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]BellNotificationAttendanceRequest request)
        {
            BellNotificationAttendance BellNotificationAttendance = new BellNotificationAttendance();
            try
            {



                if (request.id != null && request.id >0)
                {
                    
                   

                    var appversion = GInfraEntities.Notifications.Where(e => e.Id == request.id && e.isactive==true).ToList();
                    if (appversion.Count > 0)
                    {
                        foreach (var item in appversion)
                        {
                            item.isactive = false;
                            GInfraEntities.SaveChanges();





                           
                        }


                        BellNotificationAttendance.Status = true;
                        BellNotificationAttendance.Message = "Delete Successfully";


                        return Request.CreateResponse(HttpStatusCode.OK, BellNotificationAttendance);




                    }
                    else
                    {
                        BellNotificationAttendance.Status = false;
                        BellNotificationAttendance.Message = "No Records Found";
                        return Request.CreateResponse(HttpStatusCode.OK, BellNotificationAttendance);
                    }
                }
                else
                {
                    BellNotificationAttendance.Status = false;
                    BellNotificationAttendance.Message = "No Records Found";
                    return Request.CreateResponse(HttpStatusCode.OK, BellNotificationAttendance);
                }

            }
            catch (Exception ex)
            {
                BellNotificationAttendance.Status = false;
                BellNotificationAttendance.Message = "" + ex.Message + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, BellNotificationAttendance);
            }


        }
    }
}