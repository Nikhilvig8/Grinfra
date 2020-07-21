using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Controllers
{
    public class AttendanceListByUser
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<AttendanceListByUserResponse> Data;
        public AttendanceListByUser()
        {
            Data = new List<AttendanceListByUserResponse>();
        }
    }
    public class AttendanceListByUserResponse
    {
        public string AttendanceLogId { get; set; }
        public string AttendanceDate { get; set; }
        public string EmployeeId { get; set; }
        public string InTime { get; set; }
        //public string InDeviceId { get; set; }
        public string OutTime { get; set; }
        //public string OutDeviceId { get; set; }
        public string Duration { get; set; }
        //public string LeaveType { get; set; }
        public string HeadId { get; set; }
        public string AddressIn { get; set; }
        public string LattitudeIn { get; set; }
        public string LongitudeIn { get; set; }
        public string AddressOut { get; set; }
        public string LattitudeOut { get; set; }
        public string LongitudeOut { get; set; }
        public string AttendanceStatus { get; set; }
        public string Headremarks { get; set; }
        public string MemberName { get; set; }
        public string InImage { get; set; }
        public string MarkedByIn { get; set; }
        public string MarkedByOut { get; set; }
        public string ModeofpunchOut { get; set; }
        public string PolygonIn { get; set; }
        public string PolygonOut { get; set; }
        public string MarkedBy { get; set; }
        public string Designation { get; set; }

    }
    public class AttendanceListByUserRequest
    {
        public string LoginId { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}