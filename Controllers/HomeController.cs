
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GrInfra.Models;
using OfficeOpenXml;

namespace GrInfra.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            //FTPClient ftp = new FTPClient("ftp://127.0.0.1/", "Test", "test");

            //ftp.Download("attendance.xlsx", "");
            ImportData();
            return View();
        }

        public bool ImportData()
        {
            var result = false;
            try
            {
                string path = Server.MapPath("~/UploadFiles") + "\\attendance.xlsx";
                FileInfo file = new FileInfo(path);
                //var package = new ExcelPackage(new System.IO.FileInfo(path));
                //int startColumn = 1;
                //int startRow = 2;
                //ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                //object data = null;
                GInfraEntities db = new GInfraEntities();

                using (ExcelPackage package = new ExcelPackage(file))
                {

                    ExcelWorksheet workSheet = package.Workbook.Worksheets["attendance"];
                    int totalRows = workSheet.Dimension.Rows;



                    for (int i = 3; i <= totalRows; i++)
                    {
                        BiometricAttendance biometric = new BiometricAttendance();
                        BiometricAttendance biometricCheck = new BiometricAttendance();
                        string EmployeeId = workSheet.Cells[i, 2].Value.ToString();
                        DateTime StartDate = Convert.ToDateTime(workSheet.Cells[i, 3].Value);
                        biometricCheck = db.BiometricAttendances.Where(x => x.EmployeeId == EmployeeId && x.StartDate == StartDate).FirstOrDefault();
                        if (biometricCheck == null)
                        {
                            biometric.EmployeeId = workSheet.Cells[i, 2].Value.ToString();
                            biometric.StartDate = Convert.ToDateTime(workSheet.Cells[i, 3].Value);
                            if (workSheet.Cells[i, 4].Value == null)
                            {
                                biometric.PunchInDate = null;
                            }
                            else
                            {
                                biometric.PunchInDate = Convert.ToDateTime(workSheet.Cells[i, 4].Value);
                            }

                            if (workSheet.Cells[i, 5].Value == null)
                            {
                                biometric.PunchInTime = null;
                            }
                            else
                            {
                                DateTime temp = Convert.ToDateTime(workSheet.Cells[i, 5].Value);
                                biometric.PunchInTime = temp.TimeOfDay;
                            }

                            if (workSheet.Cells[i, 6].Value == null)
                            {
                                biometric.PunchOutDate = null;
                            }
                            else
                            {
                                biometric.PunchOutDate = Convert.ToDateTime(workSheet.Cells[i, 6].Value);
                            }

                            if (workSheet.Cells[i, 7].Value == null)
                            {
                                biometric.PunchOutTime = null;
                            }
                            else
                            {
                                DateTime temp = Convert.ToDateTime(workSheet.Cells[i, 7].Value);
                                biometric.PunchOutTime = temp.TimeOfDay;
                            }

                            if (workSheet.Cells[i, 8].Value == null)
                            {
                                biometric.CustRegularized = null;
                            }
                            else
                            {
                                biometric.CustRegularized = workSheet.Cells[i, 8].Value.ToString();
                            }

                            db.BiometricAttendances.Add(biometric);
                            db.SaveChanges();
                        }
                        else
                        {
                            if (workSheet.Cells[i, 4].Value == null)
                            {
                                biometricCheck.PunchInDate = null;
                            }
                            else
                            {
                                DateTime temp = Convert.ToDateTime(workSheet.Cells[i, 4].Value);
                                if (biometricCheck.PunchInDate != null)
                                {
                                    if (temp <= biometricCheck.PunchInDate)
                                    {
                                        biometricCheck.PunchInDate = temp;
                                    }
                                }
                                else
                                {
                                    biometricCheck.PunchInDate = temp;
                                }
                            }

                            if (workSheet.Cells[i, 5].Value == null)
                            {
                                biometricCheck.PunchInTime = null;
                            }
                            else
                            {
                                DateTime temp = Convert.ToDateTime(workSheet.Cells[i, 5].Value);
                                TimeSpan t = temp.TimeOfDay;
                                if (biometricCheck.PunchInTime != null)
                                {
                                    if (t <= biometricCheck.PunchInTime)
                                    {
                                        biometricCheck.PunchInTime = temp.TimeOfDay;
                                    }
                                }
                                else
                                {
                                    biometricCheck.PunchInTime = temp.TimeOfDay;
                                }
                            }


                            if (workSheet.Cells[i, 6].Value == null)
                            {
                                biometricCheck.PunchOutDate = null;
                            }
                            else
                            {
                                DateTime temp = Convert.ToDateTime(workSheet.Cells[i, 6].Value);
                                if (biometricCheck.PunchOutDate != null)
                                {
                                    if (temp >= biometricCheck.PunchOutDate)
                                    {
                                        biometricCheck.PunchOutDate = temp;
                                    }
                                }
                                else
                                {
                                    biometricCheck.PunchOutDate = temp;
                                }
                            }

                            if (workSheet.Cells[i, 7].Value == null)
                            {
                                biometricCheck.PunchOutTime = null;
                            }
                            else
                            {
                                DateTime temp = Convert.ToDateTime(workSheet.Cells[i, 7].Value);
                                TimeSpan t = temp.TimeOfDay;
                                if (biometricCheck.PunchOutTime != null)
                                {
                                    if (t >= biometricCheck.PunchOutTime)
                                    {
                                        biometricCheck.PunchOutTime = temp.TimeOfDay;
                                    }
                                }
                                else
                                {
                                    biometricCheck.PunchOutTime = temp.TimeOfDay;
                                }
                            }

                            if (workSheet.Cells[i, 8].Value == null)
                            {
                                biometric.CustRegularized = null;
                            }
                            else
                            {
                                biometric.CustRegularized = workSheet.Cells[i, 8].Value.ToString();
                            }

                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }

            return result;
        }
    }
}

