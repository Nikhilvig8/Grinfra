using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace GrInfra.Models
{
    public class BellNotificationAttendance
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
        public List<BellNotificationAttendanceResponse> Data;
        public BellNotificationAttendance()
        {
            Data = new List<BellNotificationAttendanceResponse>();
        }

    }
    public class BellNotificationAttendanceResponse
    {
        public string Id { get; set; }
        public string Curr_datetime { get; set; }
        public string Userid { get; set; }
        public string Type { get; set; }
        public string notificationType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        






    }
    public class BellNotificationAttendanceRequest
    {

        
        public string userid { get; set; }
        public int id { get; set; }




    }

    
}