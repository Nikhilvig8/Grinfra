using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class LoginController : ApiController
    {
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]LoginRequest request)
        {
            Login login = new Login();
            clsAuthentication obj = new clsAuthentication();
            try
            {
              bool b = obj.IsValidAdsUser("10.100.50.20", @"administrator", "Accer#Gril@30th");
                //bool b = obj.IsValidAdsUser("1.6.102.130", @"192.168.60.1\Administrator", "Accer#Gril@20th");
                //       bool b = obj.IsValidAdsUser("1.6.102.130", @"" + request.Loginid + "", "" + request.Password + "");
                //if (b)
                // {
                //     var k = GInfraEntities.sp_login(request.Loginid, request.Password, request.MobileUniqueID).ToList();
                //     LoginResponse LoginResponse = new LoginResponse();
                //     foreach (var kp in k)
                //     {
                //         if (kp.Status == "true")
                //         {
                //             // var item = GInfraEntities.EmployeeMasters.Single(e => e.EmployeeId == request.Loginid);
                //             var item = (from p in GInfraEntities.EmployeeMasters
                //                        join emp in GInfraEntities.MasterPasswords on p.EmployeeId equals emp.UserID
                //                        where emp.UserID == request.Loginid
                //                        select new
                //                        {
                //                            PayCode = p.PayCode,
                //                            DateOfJoining = p.DateOfJoining,
                //                            EmployeeId = p.EmployeeId,
                //                            EmpName = p.EmpName,
                //                            GuardianName = p.GuardianName,
                //                            EmpEmailId = p.EmpEmailId,
                //                            EmpMobile = p.EmpMobile,
                //                            ReportingManger = p.ReportingManger,
                //                            ReportingMangerEmail = p.ReportingMangerEmail,
                //                            ReportingManagerMobile = p.ReportingManagerMobile,
                //                            HRManager = p.HRManager,
                //                            HRManagerEmail = p.HRManagerEmail,
                //                            HRManagerMobile = p.HRManagerMobile,
                //                            DateOfBirth = p.DateOfBirth,
                //                            Company = p.Company,
                //                            Department = p.Department,
                //                            CAT = p.CAT,
                //                            Section = p.Section,
                //                            GradeCode = p.GradeCode,
                //                            Branch = p.Branch,
                //                            Designation = p.Designation,
                //                            Sex = p.Sex,
                //                            MobileCode = emp.Authtokenid
                //                        }).FirstOrDefault();

                //             LoginResponse.PayCode =item.ToString();
                //             LoginResponse.DateOfJoining = item.DateOfJoining.ToString();
                //             LoginResponse.EmployeeId = item.EmployeeId.ToString();
                //             LoginResponse.EmpName = item.EmpName.ToString();
                //             LoginResponse.GuardianName = item.GuardianName.ToString();
                //             LoginResponse.EmpEmailId = item.EmpEmailId.ToString();
                //             LoginResponse.EmpMobile = item.EmpMobile.ToString();
                //             LoginResponse.ReportingManger = item.ReportingManger.ToString();
                //             LoginResponse.ReportingMangerEmail = item.ReportingMangerEmail.ToString();
                //             LoginResponse.ReportingManagerMobile = item.ReportingManagerMobile.ToString();
                //             LoginResponse.HRManager = item.HRManager.ToString();
                //             LoginResponse.HRManagerEmail = item.HRManagerEmail.ToString();
                //             LoginResponse.HRManagerMobile = item.HRManagerMobile.ToString();
                //             LoginResponse.DateOfBirth = item.DateOfBirth.ToString();
                //             LoginResponse.Company = item.Company.ToString();
                //             LoginResponse.Department = item.Department.ToString();
                //             LoginResponse.CAT = item.CAT.ToString();
                //             LoginResponse.Section = item.Section.ToString();
                //             LoginResponse.GradeCode = item.GradeCode.ToString();
                //             LoginResponse.Branch = item.Branch.ToString();
                //             LoginResponse.Designation = item.Designation.ToString();
                //             LoginResponse.Sex = item.Sex.ToString();
                //             LoginResponse.MobileCode = item.MobileCode.ToString();

                //         }

                //         else
                //         {
                //             login.Status = false;
                //             login.Message = "" + kp.Message + "";
                //             return Request.CreateResponse(HttpStatusCode.OK, login);
                //         }
                //     };

                // }
                if (request.MobileUniqueID != "" && request.Password != "" && request.Loginid != null && request.Password != null && request.MobileUniqueID != null)
                {
                    var k = GInfraEntities.sp_login(request.Loginid, request.Password, request.MobileUniqueID,request.DeviceToken).ToList();

                    LoginResponse LoginResponse = new LoginResponse();
                    foreach (var kp in k)
                    {
                        if (kp.Status == "true")
                        {
                            //var item = GInfraEntities.EmployeeMasters.Single(e => e.EmployeeId == request.Loginid);
                            var item = (from p in GInfraEntities.EmployeeMasters
                                        join emp in GInfraEntities.MasterPasswords on p.EmployeeId equals emp.UserID
                                        where emp.UserID == request.Loginid
                                        select new
                                        {

                                            DateOfJoining = p.DateOfJoining,
                                            EmployeeId = p.EmployeeId,
                                            EmpName = p.EmpName,

                                            EmpEmailId = p.EmpEmailId,
                                            EmpMobile = p.EmpMobile,


                                            HRManagerEmail = p.HRManagerEmail,
                                            HRManagerMobile = p.HRManagerMobile,
                                            DateOfBirth = p.DateOfBirth,
                                            Company = p.Company,
                                            Department = p.Department,

                                            GradeCode = p.GradeCode,
                                            Branch = p.BUCode,
                                            Designation = p.Designation,
                                            Sex = p.Sex,
                                            MobileCode = emp.Authtokenid,
                                            managerid = p.MangerID,
                                            MangerName = p.MangerName,
                                            MangerEmail = p.MangerEmail,
                                            ManagerMobile = p.ManagerMobile,
                                            HRID = p.HRID,
                                            HRName = p.HRName

                                        }).FirstOrDefault();
                            
                            LoginResponse.DateOfJoining = item.DateOfJoining.ToString();
                            LoginResponse.EmployeeId = item.EmployeeId.ToString();
                            LoginResponse.EmpName = item.EmpName.ToString();
                            
                            LoginResponse.EmpEmailId = item.EmpEmailId.ToString();
                            LoginResponse.EmpMobile = item.EmpMobile.ToString();
                           
                            LoginResponse.HRManagerEmail = item.HRManagerEmail.ToString();
                            LoginResponse.HRManagerMobile = item.HRManagerMobile.ToString();
                            LoginResponse.DateOfBirth = item.DateOfBirth.ToString();
                            LoginResponse.Company = item.Company.ToString();
                            LoginResponse.Department = item.Department.ToString();
                           
                            LoginResponse.GradeCode = item.GradeCode.ToString();
                            LoginResponse.Branch = item.Branch.ToString();
                            LoginResponse.Designation = item.Designation.ToString();
                            LoginResponse.Sex = item.Sex.ToString();
                            LoginResponse.MobileCode = item.MobileCode.ToString();
                            LoginResponse.MangerName = item.MangerName.ToString();
                            LoginResponse.MangerEmail = item.MangerEmail.ToString();
                            LoginResponse.ManagerMobile = item.ManagerMobile.ToString();
                            LoginResponse.HRID = item.HRID.ToString();
                            LoginResponse.HRManagerEmail = item.HRManagerEmail.ToString();
                            LoginResponse.HRManagerMobile = item.HRManagerMobile.ToString();
                            LoginResponse.HRName = item.HRName.ToString();
                            if (item.managerid!=null)
                            {
                                LoginResponse.MangerID = item.managerid.ToString();
                            }
                            else
                            {
                                LoginResponse.MangerID ="-";
                            }
                            string checkhead = "";
                            try
                            {
                                checkhead = (from n in GInfraEntities.EmployeeMasters
                                             where n.MangerID.Equals(request.Loginid)
                                             select n.EmployeeId).First();
                            }
                            catch
                            {
                                checkhead = null;
                            }

                            if (checkhead != null)
                            {
                                LoginResponse.ishead = "1";
                            }
                            else
                            {
                                LoginResponse.ishead = "0";
                            }


                        }

                        else
                        {
                            login.Status = false;
                            login.Message = "" + kp.Message + "";
                            return Request.CreateResponse(HttpStatusCode.OK, login);
                        }
                    };
                    login.Data.Add(LoginResponse);
                    login.Status = true;
                    login.Message = "Login Successfully";
                    return Request.CreateResponse(HttpStatusCode.OK, login);
                }
                else
                {
                    login.Status = false;
                    login.Message = "" + HttpStatusCode.BadRequest + "";
                    return Request.CreateResponse(HttpStatusCode.BadRequest, login);
                }
            }
            catch (Exception ex)
            {
                login.Status = false;
                login.Message = "" + ex + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, login);
            }


        }

    }
}