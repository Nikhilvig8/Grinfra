using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class Coordinates
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<CoordinatesResponse> Data;
        public Coordinates()
        {
            Data = new List<CoordinatesResponse>();
        }
    }
    public class CoordinatesResponse
    {
        public string Lattitude { get; set; }
        public string Longtitude { get; set; }

    }
    public class CoordinatesRequest
    {
        public string branch { get; set; }
        


    }
}