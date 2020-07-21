using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace GrInfra.Models
{
    public class ApproveRejectAttendance
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }
       
    }
    public class ApproveRejectAttendanceResponse
    {
        public int AttendanceLogId { get; set; }
        public string AttendanceDate { get; set; }
        public string DateName { get; set; }
        public string EmployeeId { get; set; }
        public string InTime { get; set; }

        public string OutTime { get; set; }

        public string Duration { get; set; }
        public string AttendanceStatus { get; set; }
        public string Headremarks { get; set; }
        public string MemberName { get; set; }
        public string AddressOut { get; set; }
        public string HeadId { get; set; }
        public string AddressIn { get; set; }
        public string LattitudeIn { get; set; }
        public string LongitudeIn { get; set; }
        public string LattitudeOut { get; set; }






    }
    public class ApproveRejectAttendanceRequest
    {

        public  string AttendanceLogId { get; set; }
        public string ManagerComment { get; set; }
        public string Action { get; set; }
        public string userid { get; set; }




    }

    public class ApproveRejectODRegurizeMail
    {
        public int ApplyLeaveMail(string usermailid, string headmailid, string touser, string fromdate)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //mail.To.Add(usermailid);
                //mail.CC.Add(headmailid);
                mail.To.Add("nikhil.vig@teamcomputers.com");
                mail.CC.Add("nikhil.vig@teamcomputers.com");
                mail.Subject = "" + touser + "- Attendance Approved";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                mail.Body = "Dear <b>" + touser + ",</b><br/><br/>Your Attendance for dated i.e " + fromdate + " has been Approved.<br/><br/><br/><br/>Wish You a Great Day Ahead!<br/><br/>Regards,<br/>Human Capital";
                //SmtpServer.Send(mail);
                return 1;

            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int RejectODMail(string usermailid, string headmailid, string touser, string fromdate)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //mail.To.Add(usermailid);
                //mail.CC.Add(headmailid);
                //mail.To.Add("paraag.d@grinfra.com");
                mail.To.Add("nikhil.vig@teamcomputers.com");
                mail.CC.Add("nikhil.vig@teamcomputers.com");
                mail.Subject = "" + touser + "- Attendance Rejected";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                mail.Body = "Dear <b>" + touser + ",</b><br/><br/>Your Attendance for dated i.e " + fromdate + " has been Rejected.<br/><br/><br/><br/>Wish You a Great Day Ahead!<br/><br/>Regards,<br/>Human Capital";
                // SmtpServer.Send(mail);
                return 1;

            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}