using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class HrNotification
    {

        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<HrNotificationResponse> Data;
        public HrNotification()
        {
            Data = new List<HrNotificationResponse>();
        }
    }
    public class HrNotificationResponse
    {
        
        public string imageurl { get; set; }
   
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public class HrNotificationRequest
    {

        public string branch { get; set; }

    }


}