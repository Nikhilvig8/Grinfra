using GrInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static GrInfra.Global;

namespace GrInfra.Controllers
{
    [SessionExpireAttribute]
    public class DashBoardController : Controller
    {
        GInfraEntities db = new GInfraEntities();
        public List<SelectListItem> ToSelectList1(List<DropDownModel> lis)
        {
            List<SelectListItem> list = new List<SelectListItem>();

            List<string> dublicate = new List<string>();
            foreach (var row in lis)
            {
                list.Add(new SelectListItem()
                {
                    Value = row.Id,
                    Text = row.Id + " - " + row.Value
                }); ;
            }
            return list;
        }
        // GET: DashBoard
        public ActionResult DashBoard()
        {
            ViewBag.BranchList = ToSelectList1((from A in db.SiteMasters select new DropDownModel { Id = A.BUCode.ToString(), Value = A.BUDescription }).ToList());
            return View();
        }

        public JsonResult DashBoardAbsentSiteWise(string location, string fromdate, string todate)
        {
            string LoginId = Session["LoginId"].ToString();
            string Role = Session["Role"].ToString();

            if (Role == "RL02")
            {
                if (location == null || location == "")
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fromd = Convert.ToDateTime(fromdate);
                        DateTime tod = Convert.ToDateTime(todate);
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate >= fromd && A.AttendanceDate <= tod && B.HRID == LoginId
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate == DateTime.Today && B.HRID == LoginId
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fromd = Convert.ToDateTime(fromdate);
                        DateTime tod = Convert.ToDateTime(todate);
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate >= fromd && A.AttendanceDate <= tod && B.HRID == LoginId && B.BUCode == location
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate == DateTime.Today && B.BUCode == location && B.HRID == LoginId
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            if (Role == "RL03")
            {
                if (location == null || location == "")
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fromd = Convert.ToDateTime(fromdate);
                        DateTime tod = Convert.ToDateTime(todate);
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate >= fromd && A.AttendanceDate <= tod && B.MangerID == LoginId
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate == DateTime.Today && B.MangerID == LoginId
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fromd = Convert.ToDateTime(fromdate);
                        DateTime tod = Convert.ToDateTime(todate);
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate >= fromd && A.AttendanceDate <= tod && B.MangerID == LoginId && B.BUCode == location
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate == DateTime.Today && B.BUCode == location && B.MangerID == LoginId
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else
            {
                if (location == null || location == "")
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fromd = Convert.ToDateTime(fromdate);
                        DateTime tod = Convert.ToDateTime(todate);
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate >= fromd && A.AttendanceDate <= tod
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate == DateTime.Today
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (fromdate != "" && todate != "")
                    {
                        DateTime fromd = Convert.ToDateTime(fromdate);
                        DateTime tod = Convert.ToDateTime(todate);
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate >= fromd && A.AttendanceDate <= tod && B.BUCode == location
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var result = (from A in db.AttendanceLogsNewForMobiles
                                      join B in db.EmployeeMasters on A.EmployeeId equals B.EmployeeId
                                      where A.AttendanceDate == DateTime.Today && B.BUCode == location
                                      group A by A.AttendanceStatus into pg
                                      select new
                                      {
                                          Status = pg.FirstOrDefault().AttendanceStatus,
                                          Count = pg.Count()
                                      }).ToList();

                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
        }
    }
}