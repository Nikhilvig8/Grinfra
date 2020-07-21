
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GrInfra.Models;
using GrInfra.Controllers;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Net.Mail;
using System.Text;

namespace AttendenceTeamWorks.Controllers
{
    public class LoginPortalController : Controller
    {

        public ActionResult Login()
        {
            

            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login", "LoginPortal");
        }
        [HttpPost]
        public ActionResult Login(LoginPortalModel _login)
        {


            if (ModelState.IsValid) //validating the user inputs  
            {
                bool isExist = false;
                using (GInfraEntities _entity = new GInfraEntities())  // out Entity name is "EventManagementDB_Entities"  
                {
                    isExist = _entity.MasterPasswords.Where(x => x.UserID.Trim() == _login.UserId.Trim() && x.UserPassword == _login.password && x.isactive == true).Any(); //validating the user name in tblLogin table whether the user name is exist or not  
                    if (isExist)
                    {
                        MasterPassword masterPassword = _entity.MasterPasswords.Where(x => x.UserID.Trim() == _login.UserId.Trim() && x.UserPassword == _login.password).FirstOrDefault();
                        //Get the Menu details from entity and bind it in MenuModels list.  
                        FormsAuthentication.SetAuthCookie(_login.UserId.Trim(), false); // set the formauthentication cookie  
                        Session["LoginId"] = _login.UserId;

                        if (masterPassword.Super == true)
                        {
                            Session["Role"] = "RL01";
                        }
                        else
                        {
                            if (masterPassword.Role == "RL04")
                            {
                                ViewBag.ErrorMsg = "UnAuthorized Credentials! Contact Super Admin";
                                return View();
                            }
                            Session["Role"] = masterPassword.Role;
                        }


                        return RedirectToAction("DashBoard", "DashBoard");

                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Please enter the Valid Credentials!...";
                        return View();
                    }
                }
            }
            return View();

        }
        [HttpGet]
        public ActionResult Forgotten()
        {


            return View("Forgotten");
        }
        [HttpPost]
        public ActionResult Forgotten(ForgetPortalModel _forget)
        {
            GInfraEntities db = new GInfraEntities();
            if (_forget.Emailid == null || _forget.Emailid == "")
            {
                ViewBag.ErrorMsg = "Enter Valid EmailID";
            }
            else
            {
                string userid = db.EmployeeMasters.Where(a => a.EmpEmailId == _forget.Emailid).OrderByDescending(z => z.EmpEmailId).Take(1).Select(b => b.EmployeeId).DefaultIfEmpty("-").SingleOrDefault().ToString();
                if (userid == "-" || userid == null)
                {
                    ViewBag.ErrorMsg = "This EmailID does not exist";
                }
                else
                {

                    var detail = (from p in db.EmployeeMasters
                                  join o in db.MasterPasswords on p.EmployeeId equals o.UserID
                                  where p.EmployeeId == userid




                                  select new
                                  {
                                      password = o.UserPassword,
                                      MemberName = p.EmpName,
                                      Email = p.EmpEmailId,




                                  }).SingleOrDefault();

                    string newpassword = CreatePassword(6);
                    int i = ApplyLeaveMail(_forget.Emailid, detail.MemberName, userid, detail.password);
                    if(i==1)
                    {
                        //MasterPassword updatedCustomer = (from c in db.MasterPasswords
                        //                                  where c.UserID == userid
                        //                                  select c).FirstOrDefault();
                        //updatedCustomer.UserPassword = newpassword;

                        


                        //int insertedRecords = db.SaveChanges();
                        //if(insertedRecords==1)
                        //{
                            ViewBag.ErrorMsg = "Hi "+ detail.MemberName + ", Please Look into your Mail Inbox";
                        ModelState.Clear();
                        //}
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Something Wrong With your EmailID";
                    }



                    
                }
            }


            return View("Forgotten");
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        public int ApplyLeaveMail(string usermail, string username, string tmc,string password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                mail.To.Add("nikhil.vig@teamcomputers.com");
                mail.Subject = "" + username + "- Credentials";
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.High;
                mail.Body = "Dear <b>" + username + "</b><br/><br/>Your UserId : " + tmc + " and Password is "+ password + " .<br/><br/> <br/><br/>Wish You a Great Day Ahead!<br/><br/>Regards,<br/>Human Capital";
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