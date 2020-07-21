using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class AttendenceIn
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
    public class AttendenceInResponse
    {
        public int AttendanceLogId { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string AttendanceStatus { get; set; }
    }
    public class AttendenceInRequest
    {
        
        public HttpPostedFileBase FilePath { get; set; }
        public string Filee { get; set; }
        public string UserId { get; set; }
        public string InTime { get; set; }
        public string HeadId { get; set; }
        public string AddressIn { get; set; }
        public string LattitudeIn { get; set; }
        public string LongitudeIn { get; set; }
        public string mode { get; set; }
        public string PolygonIn { get; set; }
        public string action { get; set; }
        


    }
   

}