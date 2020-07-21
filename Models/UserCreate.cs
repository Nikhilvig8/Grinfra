using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class UserCreate
    {

        public string DateOfJoining { get; set; }
        public string EmployeeId { get; set; }
        public string EmpName { get; set; }
        public string EmpEmailId { get; set; }
        public string EmpMobile { get; set; }
        public string ReportingManger { get; set; }
        public string ReportingMangerName { get; set; }
        public string HRID { get; set; }
        public string HRName { get; set; }

        public string HRManagerMobile { get; set; }
        public string DateOfBirth { get; set; }

        public string Department { get; set; }

        public string Branch { get; set; }
        public string Designation { get; set; }
        public string Sex { get; set; }
        public string Role { get; set; }
        public string BUCode { get; set; }
        public string IMEI { get; set; }
        public int Battery { get; set; }
        public string LastActivity { get; set; }
        public string LastLocation { get; set; }
        public string AppVersion { get; set; }
        


    }
}