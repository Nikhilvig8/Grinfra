using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class MobileInformation
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
       // public List<MobileInformationResponse> Data;
        //public List<HrResponse> HrNotification;
        //public MobileInformation()
        //{
        //    Data = new List<MobileInformationResponse>();
        //    //HrNotification = new List<HrResponse>();
        //}

    }
   
    public class MobileInformationResponse
    {
        
        public string Title { get; set; }
        
        public string Description { get; set; }
       
        public string Image { get; set; }


    }

    public class MobileInformationRequest
    {
        public string Userid { get; set; }
        public string IMEI { get; set; }
        public int Battery { get; set; }
        public string LastActivity { get; set; }
        public string LastLocation { get; set; }
        public string AppVersion { get; set; }
    }


    }