//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GrInfra.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MasterPassword
    {
        public int id { get; set; }
        public string UserID { get; set; }
        public string UserPassword { get; set; }
        public string Authtokenid { get; set; }
        public string AuthtokenStatus { get; set; }
        public string oldAuthtoken { get; set; }
        public string Role { get; set; }
        public string DeviceToken { get; set; }
        public Nullable<bool> isactive { get; set; }
        public Nullable<bool> Super { get; set; }
    }
}
