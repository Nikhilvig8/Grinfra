using GrInfra.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class BannerUploaderController : Controller
    {
        GInfraEntities db = new GInfraEntities();
        // GET: BannerUploader
        public ActionResult BannerUpload()
        {
            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role != "RL01" && Role != "RL02")
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            List<Banner_image> banners = new List<Banner_image>();
            banners = db.Banner_image.ToList();
            return View(banners);
        }

        [HttpPost]
        public ActionResult BannerUpload(HttpPostedFileBase fileupload, HttpPostedFileBase Backfileupload, string Description, string title, string fromdate, string todate)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }

            if (Description == null || Description == "")
            {
                Description = "-";
            }

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
                string fileName1 = "";
                if (Path.GetExtension(fileName) == ".gif" || Path.GetExtension(fileName) == ".jpg" || Path.GetExtension(fileName) == ".png" || Path.GetExtension(fileName) == ".mp4" || Path.GetExtension(fileName) == ".jpeg")
                {
                    int fileSize = fileupload.ContentLength;

                    if (Path.GetExtension(fileName) == ".gif")
                    {
                        if (fileSize > (4 * 1024 * 1024))
                        {
                            ViewBag.Error = "Maxixmum File Size for GIF extension is 4 MB";
                        }
                        else
                        {
                            int Size = fileSize / 1000;
                            fileupload.SaveAs(Server.MapPath("~/BannerData/" + Timestamp + "Banner" + Path.GetExtension(fileName)));
                            Banner_image banner = new Banner_image();
                            banner.imageUrl = "/BannerData/" + +Timestamp + "Banner" + Path.GetExtension(fileName);
                            banner.BackgroundImage = "-";
                            banner.isactive = true;
                            banner.Title = title;
                            banner.Description = Description;
                            banner.Extension = Path.GetExtension(fileName);
                            banner.Fromdate = Fromdate;
                            banner.Todate = Todate;

                            db.Banner_image.Add(banner);
                            db.SaveChanges();
                            ViewBag.Error = "";
                        }
                    }
                    else if (Path.GetExtension(fileName) == ".jpg" || Path.GetExtension(fileName) == ".jpeg" || Path.GetExtension(fileName) == ".png")
                    {
                        if (fileSize > (2 * 1024 * 1024))
                        {
                            ViewBag.Error = "Maxixmum File Size for JPG & PNG extension is 2 MB";
                        }
                        else
                        {
                            int Size = fileSize / 1000;
                            fileupload.SaveAs(Server.MapPath("~/BannerData/" + Timestamp + "Banner" + Path.GetExtension(fileName)));
                            Banner_image banner = new Banner_image();
                            banner.imageUrl = "/BannerData/" + +Timestamp + "Banner" + Path.GetExtension(fileName);
                            banner.BackgroundImage = "-";
                            banner.isactive = true;
                            banner.Title = title;
                            banner.Description = Description;
                            banner.Extension = Path.GetExtension(fileName);
                            banner.Fromdate = Fromdate;
                            banner.Todate = Todate;

                            db.Banner_image.Add(banner);
                            db.SaveChanges();
                            ViewBag.Error = "";
                        }
                    }
                    else if (Path.GetExtension(fileName) == ".mp4")
                    {
                        if (fileSize > (25 * 1024 * 1024))
                        {
                            ViewBag.Error = "Maxixmum File Size for MP4 extension is 25 MB";
                        }
                        else
                        {
                            int Size = fileSize / 1000;
                         
                            if (Backfileupload != null)
                            {
                                fileName1 = Path.GetFileName(Backfileupload.FileName);
                                if (Path.GetExtension(fileName1) == ".png" || Path.GetExtension(fileName1) == ".jpg" || Path.GetExtension(fileName1) == ".gif" || Path.GetExtension(fileName1) == ".jpeg")
                                {
                                    fileupload.SaveAs(Server.MapPath("~/BannerData/" + Timestamp + "Banner" + Path.GetExtension(fileName)));
                                    Backfileupload.SaveAs(Server.MapPath("~/BannerData/" + Timestamp + "Background" + Path.GetExtension(fileName1)));
                                    ViewBag.BackGroundImageError = "";
                                }
                                else
                                {
                                    ViewBag.BackGroundImageError = "Background Image only support PNG, JPG & GIF format ";
                                    List<Banner_image> banner11 = new List<Banner_image>();
                                    banner11 = db.Banner_image.ToList();
                                    return View(banner11);
                                }
                            }
                            else
                            {
                                ViewBag.BackGroundImageError = "Background Image Compuslory In Video Upload Case";
                                List<Banner_image> banner11 = new List<Banner_image>();
                                banner11 = db.Banner_image.ToList();
                                return View(banner11);
                            }
                            Banner_image banner = new Banner_image();
                            banner.imageUrl = "/BannerData/" + +Timestamp + "Banner" + Path.GetExtension(fileName);
                            if (fileName1 != "" && fileName1 != null)
                            {
                                banner.BackgroundImage = "/BannerData/" + +Timestamp + "Background" + Path.GetExtension(fileName1);
                            }
                            else
                            {
                                banner.BackgroundImage = "-";
                            }
                            banner.isactive = true;
                            banner.Title = title;
                            banner.Description = Description;
                            banner.Extension = Path.GetExtension(fileName);
                            banner.Fromdate = Fromdate;
                            banner.Todate = Todate;

                            db.Banner_image.Add(banner);
                            db.SaveChanges();
                            ViewBag.Error = "";
                        }
                    }
                }
                else
                {
                    ViewBag.Error = "Only JPG, PNG, GIF and MP4 file formats are supported";
                }
            }
            List<Banner_image> banners = new List<Banner_image>();
            banners = db.Banner_image.ToList();
            return View(banners);
        }

        public ActionResult ChangeStatus(string id, string status)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            bool stat = false;
            int bannerId = Convert.ToInt32(id);
            if (status == "true")
            {
                stat = true;
            }
            Banner_image banner = db.Banner_image.Where(x => x.id == bannerId).FirstOrDefault();
            banner.isactive = stat;

            db.Entry(banner).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("BannerUpload");
        }

        public ActionResult Delete(int id)
        {
            string LoginId = Session["LoginId"].ToString();

            if (LoginId == "" || LoginId == null)
            {
                return RedirectToAction("LogOut", "LoginPortal");
            }
            Banner_image banner_image = db.Banner_image.Find(id);
            if (banner_image.imageUrl != "-")
            {
                string path = Server.MapPath(banner_image.imageUrl);
                FileInfo file = new FileInfo(path);
                file.Delete();
            }
            if (banner_image.BackgroundImage != "-")
            {
                string path = Server.MapPath(banner_image.BackgroundImage);
                FileInfo file = new FileInfo(path);
                file.Delete();
            }
            db.Banner_image.Remove(banner_image);
            db.SaveChanges();
            return RedirectToAction("BannerUpload");
        }

    }
}