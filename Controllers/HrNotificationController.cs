using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class HrNotificationController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
      
        public HttpResponseMessage Post([FromBody]HrNotificationRequest request)
        {
            string baseUrl = Url.Request.RequestUri.GetComponents(
    UriComponents.SchemeAndServer, UriFormat.Unescaped);
            HrNotification HrNotification = new HrNotification();

            try
            {
                if (request.branch != null)
                {
                    var list = GInfraEntities.sp_hrnotification(request.branch).ToList();
                    if (list.Count > 0)
                    {
                        foreach (var k in list)
                        {
                            HrNotificationResponse HrNotificationResponse = new HrNotificationResponse();


                            if (k.Image != "-" && k.Image != null && k.Image != "")
                            {
                                HrNotificationResponse.imageurl = baseUrl + k.Image.ToString();
                            }
                            else
                            {
                                HrNotificationResponse.imageurl = "-";
                            }

                            HrNotificationResponse.Title = k.Title.ToString();
                            HrNotificationResponse.Description = k.Description.ToString();




                            HrNotification.Data.Add(HrNotificationResponse);
                        }
                        HrNotification.Status = true;
                        HrNotification.Message = "Data Fetched Successfully";
                        return Request.CreateResponse(HttpStatusCode.OK, HrNotification);
                    }
                    else
                    {
                        HrNotification.Status = false;
                        HrNotification.Message = "No Record Found";
                        return Request.CreateResponse(HttpStatusCode.OK, HrNotification);
                    }
                }
                else
                {
                    HrNotification.Status = false;
                    HrNotification.Message = "BadRequest";
                    return Request.CreateResponse(HttpStatusCode.OK, HrNotification);
                }
            }
            catch (Exception ex)
            {
                HrNotification.Status = false;
                HrNotification.Message = "" + ex + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, HrNotification);
            }

        }
    }
}