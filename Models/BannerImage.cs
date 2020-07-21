using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class BannerImage
    {

        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<BannerImageResponse> Data;
        public BannerImage()
        {
            Data = new List<BannerImageResponse>();
        }
    }
    public class BannerImageResponse
    {
        
        public string imageurl { get; set; }
        public string bannerextension { get; set; }
        public string backgroundImage { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }


   
}