using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class AttendanceListByHead
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public string Search { get; set; }
        public List<AttendanceListByHeadResponse> Data { get; set; }
        public AttendanceListByHead()
        {
            Data = new List<AttendanceListByHeadResponse>();
        }
    }
    public class AttendanceListByHeadResponse
    {
        public string AttendanceLogId { get; set; }
        
        public string AttendanceDate { get; set; }
        public string DateName { get; set; }
        public string InImage { get; set; }
        public string EmployeeId { get; set; }
        public string InTime { get; set; }

        public string OutTime { get; set; }

        public string Duration { get; set; }
        public string AttendanceStatus { get; set; }
        public string Headremarks { get; set; }
        public string MemberName { get; set; }
        public string AddressOut { get; set; }
        public string HeadId { get; set; }
        public string AddressIn { get; set; }
        public string LattitudeIn { get; set; }
        public string LongitudeIn { get; set; }
        public string LattitudeOut { get; set; }

        
        public string MarkedByIn { get; set; }
        public string MarkedByOut { get; set; }
        public string ModeofpunchOut { get; set; }
        public string PolygonIn { get; set; }
        public string PolygonOut { get; set; }
        public string Designation { get; set; }
        







    }
    public class AttendanceListByHeadRequest
    {

        public string HeadId { get; set; }
        public string FromDate { get; set; }
        public string ToDate  { get; set; }
        public string Employee { get; set; }
        public string Status { get; set; }

    }

}