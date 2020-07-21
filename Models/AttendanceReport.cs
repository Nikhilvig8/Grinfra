using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class AttendanceReport
    {
        public int AttendanceLogId { get; set; }
        public System.DateTime AttendanceDate { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public Nullable<System.DateTime> InTime { get; set; }
        public Nullable<System.DateTime> OutTime { get; set; }
        public string Duration { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string HRId { get; set; }
        public string HRName { get; set; }
        public string AddressIn { get; set; }
        public string AddressOut { get; set; }
        public string AttendanceStatus { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string BUCode { get; set; }
        public string Inimage { get; set; }
        public string distancepunchin { get; set; }
        public string distancepunchout { get; set; }
        public string Officelat { get; set; }
        public string officelong { get; set; }
        public string bufferdistance { get; set; }
        public string lat1 { get; set; }
        public string long1 { get; set; }
        public string lat2 { get; set; }
        public string long2 { get; set; }
        public string blat1 { get; set; }
        public string blong1 { get; set; }

        //public Nullable<System.DateTime> BioMetricInTime { get; set; }
        //public Nullable<System.DateTime> BioMetricOutTime { get; set; }
    }
}