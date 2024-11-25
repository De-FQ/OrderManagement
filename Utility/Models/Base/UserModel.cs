
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;
using Utility.Enum;
using Utility.Helpers;

namespace Utility.Models.Base
{
    public   class UserModel 
    { 
        public string Id { get; set; }
        public string Guid { get; set; }
        public string FullName { get; set; } 
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public GenderType GenderTypeId { get; set; } 
        public string EncryptedPassword { get; set; }
        public string RoleId { get; set; } 
        public string RoleName { get; set; }
        public string RoleTypeId { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool Active { get; set; }
        public bool RoleActive { get; set; }
        //public string CompanyGuid { get; set; }
        //public string EntityGuid { get; set; }
        //public string EntityId { get; set; }
        public string ErrorMessage { get; set; }
        public bool TokenExpired { get; set; }
        public int Status { get; set; }
    }
}
