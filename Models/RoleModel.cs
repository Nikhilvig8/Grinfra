using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GrInfra.Models
{
    public class RoleModel
    {
        [Key]
        public string UserId { get; set; }
        public string Role { get; set; }
        public string RoleName { get; set; }
        public bool? isactive { get; set; }
        public bool? Super { get; set; }
        public string EmployeeName { get; set; }
        public string ManagerId { get; set; }
        public string ManagerName { get; set; }
        public string HRId { get; set; }
        public string HRName { get; set; }
        public string Branch { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string BUCode { get; set; }
    }
}