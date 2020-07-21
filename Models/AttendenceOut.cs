using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class AttendenceOut
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public int AttendanceLogId { get; set; }
        public string InTime { get; set; }
        public string AttendanceStatus { get; set; }
        public string OutTime { get; set; }
        //public List<AttendenceInResponse> Data;
        //public AttendenceIn()
        //{
        //    Data = new List<AttendenceInResponse>();
        //}
    }
    public class AttendenceOutResponse
    {
        public int AttendanceLogId { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string AttendanceStatus { get; set; }
    }
    public class AttendenceOutRequest
    {
        
       // public HttpPostedFileBase FilePath { get; set; }
       // public string Filee { get; set; }
        public string AttendanceLogId { get; set; }
        public System.DateTime AttendanceDate { get; set; }
        public string UserId { get; set; }
        public string InTime { get; set; }
        public string InDeviceId { get; set; }
        public string OutTime { get; set; }
        public string OutDeviceId { get; set; }
        public string Duration { get; set; }
        public string LeaveType { get; set; }
        public string HeadId { get; set; }
        public string AddressIn { get; set; }
        public string LattitudeIn { get; set; }
        public string LongitudeIn { get; set; }
        public string AddressOut { get; set; }
        public string LattitudeOut { get; set; }
        public string LongitudeOut { get; set; }
        public string AttendanceStatus { get; set; }
        public string mode { get; set; }
        public string PolygonOut { get; set; }
        public string action { get; set; }

    }
   

}