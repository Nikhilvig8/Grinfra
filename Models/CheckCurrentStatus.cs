using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class CheckCurrentStatus
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<CheckCurrentStatusResponse> Data;
        public CheckCurrentStatus()
        {
            Data = new List<CheckCurrentStatusResponse>();
        }



    }

    public class CheckCurrentStatusResponse
    {
        public string intime { get; set; }
        public string outtime { get; set; }
        public string AttendanceLogId { get; set; }
      //  public string Status { get; set; }



    }
    public class CheckCurrentStatusRequest
    {
        public string authtoken { get; set; }

    }
}