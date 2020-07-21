using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class AppversionController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Get()
        {
            Appversion Appversion = new Appversion();
            try
            {





                var appversion = GInfraEntities.AppversionMasters.Where(e => e.Status == "Y").ToList();
                if (appversion.Count > 0)
                {
                    foreach (var item in appversion)
                    {
                        AppversionResponse AppversionResponse = new AppversionResponse();

                        AppversionResponse.AppVersionUrl = item.AppVersionUrl.ToString();
                        AppversionResponse.AppVesrionId = item.AppVesrionId.ToString();


                        AppversionResponse.Status = item.Status.ToString();

                        Appversion.Data.Add(AppversionResponse);
                    }


                    Appversion.Status = true;
                    Appversion.Message = "App version List Fetch Successfully";


                    return Request.CreateResponse(HttpStatusCode.OK, Appversion);




                }
                else
                {
                    Appversion.Status = false;
                    Appversion.Message = "No Records Found";
                    return Request.CreateResponse(HttpStatusCode.OK, Appversion);
                }


            }
            catch (Exception ex)
            {
                Appversion.Status = false;
                Appversion.Message = "" + ex.Message + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Appversion);
            }


        }
    }
}