using GrInfra.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hangfire;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class HRAnnouncementController : Controller
    {
        GInfraEntities db = new GInfraEntities();
       
        public List<SelectListItem> ToSelectList(List<DropDownModel> lis)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<string> dublicate = new List<string>();
            list.Add(new SelectListItem()
            {
                Value = "0",
                Text = "Select All"
            });
            foreach (var row in lis)
            {
                list.Add(new SelectListItem()
                {
                    Value = row.Id,
                    Text = row.Value
                });
            }
            return list;
        }


        // GET: HRAnnouncement
        public ActionResult List()
        {
            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role != "RL01" && Role != "RL02")
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.Id.ToString(), Value = A.BUCode + " - " + A.BUDescription }).ToList());
            List<HR_Announcement> list = new List<HR_Announcement>();
            list = db.HR_Announcement.OrderByDescending(x => x.id).ToList();
            foreach (var item in list)
            {
                String BranchName = "";
                string[] BranchIdList = item.BranchId.Split(',');
                foreach (string id in BranchIdList)
                {
                    int Id = Convert.ToInt32(id);
                    SiteMaster siteMaster = db.SiteMasters.Where(x => x.Id == Id).FirstOrDefault();
                    BranchName += siteMaster.BUDescription + ",";
                }

                BranchName = BranchName.Remove(BranchName.Length - 1, 1);
                item.BranchId = BranchName;
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult List(HttpPostedFileBase fileupload, HttpPostedFileBase Backfileupload, string Description, string title, string fromdate, string todate, string[] branch)
        {
            string LoginId = Session["LoginId"].ToString();

            if(Description == null || Description == "")
            {
                Description = "-";
            }

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            if (branch == null)
            {
                ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.Id.ToString(), Value = A.BUDescription }).ToList());
                List<HR_Announcement> list = new List<HR_Announcement>();
                list = db.HR_Announcement.OrderByDescending(x => x.id).ToList();
                foreach (var item in list)
                {
                    String BranchName = "";
                    string[] BranchIdList = item.BranchId.Split(',');
                    foreach (string id in BranchIdList)
                    {
                        int Id = Convert.ToInt32(id);
                        SiteMaster siteMaster = db.SiteMasters.Where(x => x.Id == Id).FirstOrDefault();
                        BranchName += siteMaster.BUDescription + ",";
                    }

                    BranchName = BranchName.Remove(BranchName.Length - 1, 1);
                    item.BranchId = BranchName;
                }

                ViewBag.BranchError = "Please Select atleast one branch";
                return View(list);
            }
            bool status = false;

            ViewBag.BranchError = "";
            string BranchId = "";

            foreach (string id in branch)
            {
                if (id == "0")
                {
                    status = true;
                }
            }
            if (status == true)
            {
                List<SiteMaster> list = db.SiteMasters.ToList();
                foreach (var lis in list)
                {
                    BranchId += lis.Id + ",";
                }
            }
            else
            {
                foreach (string id in branch)
                {
                    BranchId += id + ",";
                }
            }
            BranchId = BranchId.Remove(BranchId.Length - 1, 1);
            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            if (fileupload != null)
            {
                int year = DateTime.Now.Year;
                DateTime Fromdate = DateTime.Today;
                DateTime Todate = new DateTime(year, 12, 31);

                if (fromdate != null && fromdate != "")
                {
                    Fromdate = Convert.ToDateTime(fromdate);
                }
                if (todate != null && todate != "")
                {
                    Todate = Convert.ToDateTime(todate);
                }

                string fileName = Path.GetFileName(fileupload.FileName);
                if (Path.GetExtension(fileName) == ".gif" || Path.GetExtension(fileName) == ".jpg" || Path.GetExtension(fileName) == ".png" || Path.GetExtension(fileName) == ".jpeg")
                {
                    int fileSize = fileupload.ContentLength;
                    if (fileSize > (4 * 1024 * 1024))
                    {
                        ViewBag.Error = "Maxixmum File Size Allowed is 4 MB";
                        ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.Id.ToString(), Value = A.BUDescription }).ToList());
                        List<HR_Announcement> list = new List<HR_Announcement>();
                        list = db.HR_Announcement.OrderByDescending(x => x.id).ToList();
                        foreach (var item in list)
                        {
                            String BranchName = "";
                            string[] BranchIdList = item.BranchId.Split(',');
                            foreach (string id in BranchIdList)
                            {
                                int Id = Convert.ToInt32(id);
                                SiteMaster siteMaster = db.SiteMasters.Where(x => x.Id == Id).FirstOrDefault();
                                BranchName += siteMaster.BUDescription + ",";
                            }

                            BranchName = BranchName.Remove(BranchName.Length - 1, 1);
                            item.BranchId = BranchName;
                        }
                        return View(list);
                    }
                    else
                    {
                        int Size = fileSize / 1000;
                        fileupload.SaveAs(Server.MapPath("~/BannerData/" + Timestamp + "Banner" + Path.GetExtension(fileName)));

                        HR_Announcement banner = new HR_Announcement();
                        banner.Image = "/BannerData/" + +Timestamp + "Banner" + Path.GetExtension(fileName);
                        banner.isactive = true;
                        banner.Title = title;
                        banner.Description = Description;
                        banner.FromDate = Fromdate;
                        banner.ToDate = Todate;
                        banner.BranchId = BranchId;

                        db.HR_Announcement.Add(banner);
                        db.SaveChanges();
                        BackgroundJob.Enqueue(() => SFTPDataController.SendHrannoucement());
                        
                        ViewBag.Error = "";
                    }
                }
                else
                {
                    ViewBag.Error = "Only JPG, PNG & GIF  file formats are supported";
                    ViewBag.BranchList = ToSelectList((from A in db.SiteMasters select new DropDownModel { Id = A.Id.ToString(), Value = A.BUDescription }).ToList());
                    List<HR_Announcement> list = new List<HR_Announcement>();
                    list = db.HR_Announcement.OrderByDescending(x => x.id).ToList();
                    foreach (var item in list)
                    {
                        String BranchName = "";
                        string[] BranchIdList = item.BranchId.Split(',');
                        foreach (string id in BranchIdList)
                        {
                            int Id = Convert.ToInt32(id);
                            SiteMaster siteMaster = db.SiteMasters.Where(x => x.Id == Id).FirstOrDefault();
                            BranchName += siteMaster.BUDescription + ",";
                        }

                        BranchName = BranchName.Remove(BranchName.Length - 1, 1);
                        item.BranchId = BranchName;
                    }
                    return View(list);
                }
            }
            else
            {
                int year = DateTime.Now.Year;
                DateTime Fromdate = DateTime.Today;
                DateTime Todate = new DateTime(year, 12, 31);

                if (fromdate != null && fromdate != "")
                {
                    Fromdate = Convert.ToDateTime(fromdate);
                }
                if (todate != null && todate != "")
                {
                    Todate = Convert.ToDateTime(todate);
                }

                HR_Announcement banner = new HR_Announcement();
                banner.Image = "-";
                banner.isactive = true;
                banner.Title = title;
                banner.Description = Description;
                banner.FromDate = Fromdate;
                banner.ToDate = Todate;
                banner.BranchId = BranchId;

                db.HR_Announcement.Add(banner);
                db.SaveChanges();
                ViewBag.Error = "";
            }
            return RedirectToAction("List");
        }

        public ActionResult ChangeStatus(string id, string status)
        {
            bool stat = false;
            int hrId = Convert.ToInt32(id);
            if (status == "true")
            {
                stat = true;
            }
            HR_Announcement hr = db.HR_Announcement.Where(x => x.id == hrId).FirstOrDefault();
            hr.isactive = stat;

            db.Entry(hr).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            HR_Announcement hr = db.HR_Announcement.Find(id);
            if (hr.Image != "-")
            {
                string path = Server.MapPath(hr.Image);
                FileInfo file = new FileInfo(path);
                file.Delete();
            }
            db.HR_Announcement.Remove(hr);
            db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}