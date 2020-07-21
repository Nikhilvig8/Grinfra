using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace GrInfra.Models
{
    public class Forgetpassword
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }


    }


    public class ForgetpasswordRequest
    {
        public string DOB { get; set; }
        public string userid { get; set; }

    }
    public class forgetmail
    {
        public int ApplyLeaveMail(string usermail, string username, string tmc)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.To.Add(usermail);
                mail.Subject = "" + username + "- Credentials";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                mail.Body = "Dear <b>" + username + "</b><br/><br/>Your UserId : " + tmc + " and Password is password .<br/><br/> <br/><br/>Wish You a Great Day Ahead!<br/><br/>Regards,<br/>Human Capital";
               SmtpServer.Send(mail);
                return 1;

            }
            catch (Exception)
            {
                return 0;
            }
        }
    }

}