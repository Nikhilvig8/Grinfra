using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class BirthdayList
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<BirthdayListResponse> Birthday;
        //public List<HrResponse> HrNotification;
        public BirthdayList()
        {
            Birthday = new List<BirthdayListResponse>();
            //HrNotification = new List<HrResponse>();
        }

    }
    public class BirthdayListResponse
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string BirthdayDate { get; set; }
        public string Branch { get; set; }
        public string EmployeeMail { get; set; }
        public string ReportingHeadId { get; set; }
        // public string BranchCode { get; set; }
        // public string ID { get; set; }


    }
    public class HrResponse
    {
        
        public string Title { get; set; }
        
        public string Description { get; set; }
       
        public string Image { get; set; }


    }

    public class BirthdayListRequest
    {
        public string userid { get; set; }
    }


    }