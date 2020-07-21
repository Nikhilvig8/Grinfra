using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class CheckAuthTokenController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();

        [HttpPost]
        public HttpResponseMessage Post([FromBody]CheckAuthTokenRequest request)
        {
            CheckAuthToken CheckAuthToken = new CheckAuthToken();
            try
            {
                if (request.AuthToken != "" && request.AuthToken != null)
                {
                    var user_id = GInfraEntities.Get_UserId(request.AuthToken).FirstOrDefault();
                    if (user_id == null)
                    {
                        CheckAuthToken.Status = false;
                        CheckAuthToken.Message = "Your device's restriction has been removed by Site HR, Now you can login again.";
                        return Request.CreateResponse(HttpStatusCode.OK, CheckAuthToken);
                    }
                    else
                    {

                        CheckAuthToken.Status = true;
                        CheckAuthToken.Message = "User Exists";
                        return Request.CreateResponse(HttpStatusCode.OK, CheckAuthToken);
                    }



                }
                else
                {
                    CheckAuthToken.Status = false;
                    CheckAuthToken.Message = "" + HttpStatusCode.OK + "";
                    return Request.CreateResponse(HttpStatusCode.OK, CheckAuthToken);
                }
            }
            catch (Exception ex)
            {
                CheckAuthToken.Status = false;
                CheckAuthToken.Message = "" + ex.Message + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, CheckAuthToken);
            }


        }






    }
}
