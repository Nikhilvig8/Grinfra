using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GrInfra.Models;

namespace GrInfra.Controllers
{
    public class BirthdayListController : ApiController
    {
        // GET api/<controller>
        GInfraEntities GInfraEntities = new GInfraEntities();
        public HttpResponseMessage Post([FromBody]BirthdayListRequest request)
        {
            BirthdayList BirthdayList = new BirthdayList();
            string baseUrl = Url.Request.RequestUri.GetComponents(
 UriComponents.SchemeAndServer, UriFormat.Unescaped);
            try
            {

                if (request.userid != "" && request.userid != null)
                {



                    var birthdaylist = GInfraEntities.GetBirthdayData(request.userid).ToList();
                    //var list = GInfraEntities.sp_hrnotification().ToList();
                    if (birthdaylist.Count > 0)
                    {
                        foreach (var item in birthdaylist)
                        {
                            BirthdayListResponse BirthdayListResponse = new BirthdayListResponse();

                            BirthdayListResponse.EmployeeCode = item.EmployeeCode.ToString();
                            BirthdayListResponse.EmployeeName = item.EmployeeName;
                            //  BirthdayListResponse.BirthdayDate = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(item.BirthdayDate)); 
                            BirthdayListResponse.BirthdayDate = DateTime.Parse(item.BirthdayDate).ToString("MMM dd");
                            BirthdayListResponse.Branch = item.Branch;
                            BirthdayListResponse.EmployeeMail = item.CmpMail;
                            if (item.MangerID == "Null" && item.MangerID != null)
                            {
                                BirthdayListResponse.ReportingHeadId = "-";
                            }
                            else
                            {
                                BirthdayListResponse.ReportingHeadId = item.MangerID.ToString();
                            }
                            BirthdayList.Birthday.Add(BirthdayListResponse);
                        }


                        BirthdayList.Status = true;
                        BirthdayList.Message = "Birthday list fetch succefully";
                        return Request.CreateResponse(HttpStatusCode.OK, BirthdayList);




                    }
                    else
                    {
                        BirthdayList.Status = false;
                        BirthdayList.Message = "No Records Found";
                        return Request.CreateResponse(HttpStatusCode.OK, BirthdayList);
                    }
                    //if (list.Count > 0)
                    //{
                    //    foreach (var k in list)
                    //    {
                    //        HrResponse HrResponse = new HrResponse();

                    //        HrResponse.Image = baseUrl + k.Image.ToString();
                    //        if (k.Image != "-" && k.Image != null && k.Image != "")
                    //        {
                    //            HrResponse.Image = baseUrl + k.Image.ToString();
                    //        }
                    //        else
                    //        {
                    //            HrResponse.Image = "-";
                    //        }

                    //        HrResponse.Title = k.Title.ToString();
                    //        HrResponse.Description = k.Description.ToString();




                    //        BirthdayList.HrNotification.Add(HrResponse);
                    //    }
                    //    //BirthdayList.Status = true;
                    //    //BirthdayList.Message = "List Fetch Successfully";


                    //    //return Request.CreateResponse(HttpStatusCode.OK, BirthdayList);
                    //}
                    //else
                    //{
                    //    //BirthdayList.Status = false;
                    //    //BirthdayList.Message = "No Records Found";
                    //   // return Request.CreateResponse(HttpStatusCode.OK, BirthdayList);
                    //}
                }
                else
                {
                    BirthdayList.Status = false;
                    BirthdayList.Message = "Bad request";
                    return Request.CreateResponse(HttpStatusCode.OK, BirthdayList);
                }

                //return Request.CreateResponse(HttpStatusCode.OK, BirthdayList);

            }
            catch (Exception ex)
            {
                //BirthdayList.Status = false;
                //BirthdayList.Message = "" + ex.Message + "";
                return Request.CreateResponse(HttpStatusCode.InternalServerError, BirthdayList);
            }


        }
    }
}