using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class MobileInformationController : ApiController
    {
        // GET api/<controller>
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]MobileInformationRequest request)
        {
            MobileInformation MobileInformation = new MobileInformation();
           
            try
            {

                if (request.Userid != "" && request.Userid != null)
                {



                    var birthdaylist = GInfraEntities.sp_mobiledetails(request.Userid, request.IMEI, request.Battery, request.LastActivity, request.LastLocation, request.AppVersion).ToList();
                    
                    if (birthdaylist.Count > 0)
                    {
                        foreach (var item in birthdaylist)
                        {
                            if (item.returncode == "True")
                            {
                                MobileInformation.Status = true;
                                MobileInformation.Message = ""+item.returnmessage+"";
                               
                            }
                            else if(item.returncode == "False")
                            {
                                MobileInformation.Status = true;
                                MobileInformation.Message = ""+ item.returnmessage + "";
                               
                            }
                            else
                            {
                                MobileInformation.Status = true;
                                MobileInformation.Message = ""+ item.returnmessage + "";
                              
                            }
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, MobileInformation);







                    }
                    else
                    {
                        MobileInformation.Status = false;
                        MobileInformation.Message = "Pass all values";
                        return Request.CreateResponse(HttpStatusCode.OK, MobileInformation);
                    }
                 
                }
                else
                {
                    MobileInformation.Status = false;
                    MobileInformation.Message = "Bad request";
                    return Request.CreateResponse(HttpStatusCode.OK, MobileInformation);
                }

                //return Request.CreateResponse(HttpStatusCode.OK, BirthdayList);

            }
            catch (Exception ex)
            {
                MobileInformation.Status = false;
                MobileInformation.Message = "" + ex.Message + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileInformation);
            }


        }
    }
}