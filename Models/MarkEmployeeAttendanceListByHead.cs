using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class MarkEmployeeAttendanceListByHead
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<MarkEmployeeAttendanceListByHeadResponse> Data;
        public MarkEmployeeAttendanceListByHead()
        {
            Data = new List<MarkEmployeeAttendanceListByHeadResponse>();
        }

    }
    public class MarkEmployeeAttendanceListByHeadRequest
    {

        public string HeadID { get; set; }
     


    }
    public class MarkEmployeeAttendanceListByHeadResponse
    {

        public string employeename { get; set; }
        public string InImage { get; set; }
        public string employeeid { get; set; }
        public string AttendanceLogId { get; set; }
        public string AttendanceDate { get; set; }
        public string Designation { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        public string Duration { get; set; }
        public string HeadId { get; set; }
        public string AddressIn { get; set; }
        public string LattitudeIn { get; set; }
        public string LongitudeIn { get; set; }
        public string AddressOut { get; set; }
        public string LattitudeOut { get; set; }
        public string LongitudeOut { get; set; }
        public string AttendanceStatus { get; set; }
        public string Modeofpunchin { get; set; }
        public string ModeofpunchOut { get; set; }




    }
}