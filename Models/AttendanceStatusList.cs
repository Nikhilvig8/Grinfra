using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class AttendanceStatusList
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<AttendanceStatusListResponse> Data;
        public AttendanceStatusList()
        {
            Data = new List<AttendanceStatusListResponse>();
        }
    }
    public class AttendanceStatusListResponse
    {
        public string AttendanceStatus { get; set; }
        

    }
}