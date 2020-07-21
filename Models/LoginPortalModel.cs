using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using GrInfra.Models;

namespace GrInfra.Models
{
    public class LoginPortalModel
    {
        [Key]
        public string UserId { get; set; }
        [DataType(DataType.Password)]
        public string password { get; set; }

    }
    public class ForgetPortalModel
    {
        [Key]
        public string Emailid { get; set; }
        

    }

    public class ChangePassword
    {
        [Key]
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }


    }

}