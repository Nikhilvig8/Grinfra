using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class BannerimageController : ApiController
    {

        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Get()
        {
            string baseUrl = Url.Request.RequestUri.GetComponents(
    UriComponents.SchemeAndServer, UriFormat.Unescaped);
            BannerImage BannerImage = new BannerImage();

            try
            {

                var list = GInfraEntities.sp_bannerimage().ToList();
                if (list.Count > 0)
                {
                    foreach (var k in list)
                    {
                        BannerImageResponse BannerImageResponse = new BannerImageResponse();

                        BannerImageResponse.imageurl = baseUrl+ k.imageUrl.ToString();
                        if (k.BackgroundImage != "-" && k.BackgroundImage != null && k.Extension== ".mp4")
                        {
                            BannerImageResponse.backgroundImage = baseUrl + k.BackgroundImage.ToString();
                        }
                        else
                        {
                            BannerImageResponse.backgroundImage = "-";
                        }
                        BannerImageResponse.bannerextension = k.Extension.ToString();
                        BannerImageResponse.Title = k.Title.ToString();
                        BannerImageResponse.Description = k.Description.ToString();




                        BannerImage.Data.Add(BannerImageResponse);
                    }
                    BannerImage.Status = true;
                    BannerImage.Message = "Data Fetched Successfully";
                    return Request.CreateResponse(HttpStatusCode.OK, BannerImage);
                }
                else
                {
                    BannerImage.Status = false;
                    BannerImage.Message = "No Record Found";
                    return Request.CreateResponse(HttpStatusCode.OK, BannerImage);
                }
            }
            catch (Exception ex)
            {
                BannerImage.Status = false;
                BannerImage.Message = "" + ex + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, BannerImage);
            }

        }
    }
}