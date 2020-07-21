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
    public class ReasonListController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Get()
        {

            ReasonList ReasonList = new ReasonList();

            try
            {

                var list = GInfraEntities.sp_ReasonsList().ToList();
                if (list.Count > 0)
                {
                    foreach (var k in list)
                    {
                        ReasonListResponse ReasonListResponse = new ReasonListResponse();
                        ReasonListResponse.id = k.id.ToString();
                        ReasonListResponse.Reasons = k.reasons.ToString();
                        
                        ReasonList.Data.Add(ReasonListResponse);
                    }
                    ReasonList.Status = true;
                    ReasonList.Message = "Data Fetched Successfully";
                    return Request.CreateResponse(HttpStatusCode.OK, ReasonList);
                }
                else
                {
                    ReasonList.Status = false;
                    ReasonList.Message = "No Record Found";
                    return Request.CreateResponse(HttpStatusCode.OK, ReasonList);
                }
            }
            catch (Exception ex)
            {
                ReasonList.Status = false;
                ReasonList.Message = "" + ex + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ReasonList);
            }

        }
    }
}