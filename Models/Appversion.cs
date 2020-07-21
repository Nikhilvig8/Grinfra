using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class Appversion
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<AppversionResponse> Data;
        public Appversion()
        {
            Data = new List<AppversionResponse>();
        }

    }
    public class AppversionResponse
    {

        public string AppVesrionId { get; set; }
        public string AppVersionUrl { get; set; }
        public string Status { get; set; }


    }
    public class updateappversionresponse
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }


    }
    public class AppversionRequest
    {

        public string AppVesrionId { get; set; }
        public string AppVersionUrl { get; set; }
        public string type { get; set; }


    }
}