using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class ReasonList
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<ReasonListResponse> Data;
        public ReasonList()
        {
            Data = new List<ReasonListResponse>();
        }
    }
    public class ReasonListResponse
    {
        public  string id { get; set; }

        public string Reasons { get; set; }
        

    }
}