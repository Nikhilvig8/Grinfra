using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class CheckAuthToken
    {
        public Boolean Status { get; set; }
        public string Message { get; set; }


    }

    public class CheckAuthTokenRequest
    {
        public string AuthToken { get; set; }

    }

}