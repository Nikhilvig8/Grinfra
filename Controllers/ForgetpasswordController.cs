using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class ForgetpasswordController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]ForgetpasswordRequest request)
        {
            Forgetpassword Forgetpassword = new Forgetpassword();
            try
            {
                if (request.DOB != "" && request.DOB != null && request.userid != "" && request.userid != null)
                {
                    string result = GInfraEntities.sp_forgetpassword(request.DOB, request.userid).FirstOrDefault();
                    if (result == "true")
                    {
                        Forgetpassword.Status = true;
                        Forgetpassword.Message = "Password send to the Mail";
                        var detail = (from p in GInfraEntities.EmployeeMasters
                                      join o in GInfraEntities.MasterPasswords on p.EmployeeId equals o.UserID
                                      where p.EmployeeId == request.userid




                                      select new
                                      {
                                          password = o.UserPassword,
                                          MemberName = p.EmpName,
                                          Email = p.EmpEmailId,




                                      }).SingleOrDefault();





                        forgetmail forgetmail = new forgetmail();
                        int i = forgetmail.ApplyLeaveMail(detail.Email, detail.MemberName, request.userid);
                        if (i == 1)
                        {
                            //  teamWorksEntities1.sp_Isemailsent("CompOffReject", request.CompOffReqId.ToString());

                        }
                        return Request.CreateResponse(HttpStatusCode.OK, Forgetpassword);
                    }
                    else
                    {
                        Forgetpassword.Status = false;
                        Forgetpassword.Message = "Please Enter valid Data";
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