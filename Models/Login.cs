using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class Login
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<LoginResponse> Data;
        public Login()
        {
            Data = new List<LoginResponse>();
        }
    }
    public class LoginRequest
    {
        public string Loginid { get; set; }
        public string Password { get; set; }
        public string MobileUniqueID { get; set; }
        public string DeviceToken { get; set; }

    }
    public class LoginResponse
    {
        public string EmployeeId { get; set; }
        public string EmpName { get; set; }
        public string DateOfJoining { get; set; }
        public string EmpEmailId { get; set; }
        public string EmpMobile { get; set; }
        public string MangerID { get; set; }
        public string ishead { get; set; }
        public string MangerName { get; set; }
        public string MangerEmail { get; set; }
        public string ManagerMobile { get; set; }
        public string HRID { get; set; }
        public string HRName { get; set; }
        public string HRManagerEmail { get; set; }
        public string HRManagerMobile { get; set; }
        public string DateOfBirth { get; set; }
        public string Company { get; set; }
        public string Department { get; set; }
        public string GradeCode { get; set; }
        public string Branch { get; set; }
        public string Designation { get; set; }
        public string Sex { get; set; }
        public string MobileCode { get; set; }
}
}