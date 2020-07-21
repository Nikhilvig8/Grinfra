using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;
namespace GrInfra.Controllers
{
    public class UpdateController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]UpdateRequest request)
        {
            Update Update = new Update();
            try
            {
                if (request.AuthToken != "" && request.Password != "" && request.oldpassword != "" && request.oldpassword != null && request.AuthToken != null && request.Password != null)
                {
                    var user_id = GInfraEntities.Get_UserId(request.AuthToken).FirstOrDefault();
                    if (user_id == null)
                    {
                        Update.Status = false;
                        Update.Message = "User does not exists";
                        return Request.CreateResponse(HttpStatusCode.NotFound, Update);
                    }
                    else
                    {
                        List<sp_updateUserPassword_Result> sp_updateUserPassword_Result = new List<sp_updateUserPassword_Result>();
                        sp_updateUserPassword_Result = GInfraEntities.sp_updateUserPassword(user_id, request.Password, request.oldpassword).ToList();
                        if (sp_updateUserPassword_Result.Count > 0)
                        {
                            foreach (var item in sp_updateUserPassword_Result)
                            {
                                if (item.returnvalue == "true")
                                {
                                    Update.Status = true;
                                    Update.Message = "Update password successfully";
                                    return Request.CreateResponse(HttpStatusCode.OK, Update);
                                }
                                else
                                {
                                    Update.Status = false;
                                    Update.Message = item.returnmessage;
                                    return Request.CreateResponse(HttpStatusCode.OK, Update);
                                }


                            }
                        }
                        else
                        {
                            Update.Status = false;
                            Update.Message = "User does not exists";
                            return Request.CreateResponse(HttpStatusCode.OK, Update);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, Update);

                    }



                }
                else
                {
                    Update.Status = false;
                    Update.Message = "" + HttpStatusCode.BadRequest + "";
                    return Request.CreateResponse(HttpStatusCode.OK, Update);
                }
            }
            catch (Exception ex)
            {
                Update.Status = false;
                Update.Message = "" + ex.Message + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Update);
            }


        }
    }
}