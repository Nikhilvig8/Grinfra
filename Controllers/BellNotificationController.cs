using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class BellNotificationController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]BellNotificationAttendanceRequest request)
        {
            BellNotificationAttendance BellNotificationAttendance = new BellNotificationAttendance();
            try
            {



                if (request.userid != null && request.userid != "")
                {

                    var appversion = GInfraEntities.Notifications.Where(e => e.isactive == true && e.Userid == request.userid).OrderByDescending(m=>m.Id) .ToList();
                    if (appversion.Count > 0)
                    {
                        foreach (var item in appversion)
                        {
                            BellNotificationAttendanceResponse BellNotificationAttendanceResponse = new BellNotificationAttendanceResponse();
                            BellNotificationAttendanceResponse.Curr_datetime = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.Curr_datetime)); 
                            BellNotificationAttendanceResponse.Userid = item.Userid.ToString();
                            BellNotificationAttendanceResponse.Type = item.Type.ToString();
                            BellNotificationAttendanceResponse.notificationType = item.Type.ToString();
                            BellNotificationAttendanceResponse.Title = item.Title.ToString();
                            BellNotificationAttendanceResponse.Description = item.Description.ToString();
                            BellNotificationAttendanceResponse.Id = item.Id.ToString();





                            BellNotificationAttendance.Data.Add(BellNotificationAttendanceResponse);
                        }


                        BellNotificationAttendance.Status = true;
                        BellNotificationAttendance.Message = "Notification List Fetch Successfully";


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