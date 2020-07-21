using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class Update
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<UpdateResponse> Data;
        public Update()
        {
            Data = new List<UpdateResponse>();
        }
    }
    public class UpdateResponse
    {
        public string UserId { get; set; }
        public string MemberName { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string Branch { get; set; }
        public Nullable<int> Branch_Code { get; set; }
        public string SBU { get; set; }
        public string SBUHeadName { get; set; }
        public string SBUHeadId { get; set; }
        public string ReportingHeadId { get; set; }
        public System.DateTime DateOfJoining { get; set; }
        public System.DateTime DateOfBirth { get; set; }
        public string CommAddress { get; set; }
        public string ContactNo { get; set; }
        public string CompaneyEmail { get; set; }
        public string PersonalEmail { get; set; }
        public string Bank_Name { get; set; }
        public string Account_No { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public string SBUCode { get; set; }
        public string Department_Code { get; set; }
        public string Mobile_Phone_No_ { get; set; }
        public string Location_Code { get; set; }
        public string First_Name { get; set; }
        public string Qualification { get; set; }
        public string UserTmcPassword { get; set; }
        public string UserNewPassword { get; set; }
        public string AuthToken { get; set; }
        public string EmpDesignation { get; set; }
        public string Gender { get; set; }
        public Nullable<System.DateTime> ConfirmationDate { get; set; }
        public string MaritalStatus { get; set; }
        public string ESIC { get; set; }
        public Nullable<bool> IsConfirmed { get; set; }
        public string ProbationStatus { get; set; }
    }
    public class UpdateRequest
    {
        public string AuthToken { get; set; }
        public string oldpassword { get; set; }
        public string Password { get; set; }
    }

}