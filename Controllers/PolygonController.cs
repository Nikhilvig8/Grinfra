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
    public class PolygonController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]CoordinatesRequest request)
        {

            Coordinates Coordinates = new Coordinates();

            try
            {
                if (request.branch != null)
                {

                    var list = GInfraEntities.sp_polygon(request.branch).ToList();
                    if (list.Count > 0)
                    {
                        foreach (var k in list)
                        {
                            CoordinatesResponse CoordinatesResponse = new CoordinatesResponse();

                            CoordinatesResponse.Lattitude = k.lattitude.ToString();
                            CoordinatesResponse.Longtitude = k.longitude.ToString();




                            Coordinates.Data.Add(CoordinatesResponse);
                        }
                        Coordinates.Status = true;
                        Coordinates.Message = "Data Fetched Successfully";
                        return Request.CreateResponse(HttpStatusCode.OK, Coordinates);
                    }
                    else
                    {
                        Coordinates.Status = false;
                        Coordinates.Message = "No Record Found";
                        return Request.CreateResponse(HttpStatusCode.OK, Coordinates);
                    }
                }
                else
                {
                    Coordinates.Status = false;
                    Coordinates.Message = "BadRequest";
                    return Request.CreateResponse(HttpStatusCode.OK, Coordinates);
                }
            }
            catch (Exception ex)
            {
                Coordinates.Status = false;
                Coordinates.Message = "" + ex + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, Coordinates);
            }

        }
    }
}