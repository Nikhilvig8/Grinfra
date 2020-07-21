using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GrInfra.Models;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(ChangePassword chng)
        {
            GInfraEntities db = new GInfraEntities();
            if ((chng.OldPassword == "" || chng.OldPassword == null) && (chng.NewPassword == "" || chng.NewPassword == null) && (chng.ConfirmPassword == "" || chng.ConfirmPassword == null))
            {
                ViewBag.ErrorMsg = "Please Fill All the fields";
            }
            else
            {
                if (chng.NewPassword == chng.ConfirmPassword)
                {
                    string loginid = Session["LoginId"].ToString();
                    string userid = db.MasterPasswords.Where(x => x.UserID == loginid && x.UserPassword == chng.OldPassword).OrderByDescending(z => z.UserID).Take(1).Select(b => b.UserID).DefaultIfEmpty("-").SingleOrDefault().ToString();


                    if (userid != "-")
                    {
                        MasterPassword updatedCustomer = (from c in db.MasterPasswords
                                                          where c.UserID == loginid
                                                          select c).FirstOrDefault();
                        updatedCustomer.UserPassword = chng.ConfirmPassword;




                        int insertedRecords = db.SaveChanges();
                        if (insertedRecords == 1)
                        {
                            ViewBag.ErrorMsg = "Your password has been changed sucecssfully ";
                            ModelState.Clear();
                        }
                        else
                        {
                            ViewBag.ErrorMsg = "Something wrong";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMsg = "Old Password is Incorrect";
                    }
                }
                else
                {
                    ViewBag.ErrorMsg = "Your Confirm Password Does not match";
                }


            }

            return View();

        }
    }
}