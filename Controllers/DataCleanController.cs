using GrInfra.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GrInfra.Controllers
{
    public class DataCleanController : ApiController
    {
       

        // GET api/<controller>/5
        public string Get(int id)
        {
            GInfraEntities db = new GInfraEntities();
            db.Database.ExecuteSqlCommand("exec deletedata "+id+"");
            return "Done";
        }

      
    }
}