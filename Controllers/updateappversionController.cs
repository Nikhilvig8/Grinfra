using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class updateappversionController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]AppversionRequest request)
        {
            updateappversionresponse Forgetpassword = new updateappversionresponse();
            try
            {
                if (request.type != "" && request.type != null && request.AppVersionUrl != "" && request.AppVersionUrl != null && request.AppVesrionId != "" && request.AppVesrionId != null)
                {
                    string result = GInfraEntities.sp_updateappversion(request.type, request.AppVesrionId, request.AppVersionUrl).FirstOrDefault();
                    if (result == "true")
                    {
                        Forgetpassword.Status = true;
                        Forgetpassword.Message = "Updated App version successfully";
                        return Request.CreateResponse(HttpStatusCode.OK, Forgetpassword);
                    }
                    else
                    {
                        Forgetpassword.Status = false;
                        Forgetpassword.Message = "Please fill  All fields";
                        return Request.CreateResponse(HttpStatusCode.OK, Forgetpassword);
                    }




                }
                else
                {
                    Forgetpassword.Status = false;
                    Forgetpassword.Message = "" + HttpStatusCode.BadRequest + "";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, Forgetpassword);
                }
            }
            catch (Exception ex)
            {
                Forgetpassword.Status = false;
                Forgetpassword.Message = "" + ex.Message + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Forgetpassword);
            }


        }
    }
}