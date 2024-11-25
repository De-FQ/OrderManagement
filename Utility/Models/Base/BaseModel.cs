using System.ComponentModel.DataAnnotations.Schema;
using Utility.Enum;

namespace Utility.Models.Base
{
    public class BaseModel
    { 
        //public Guid? ApplicationFormGuid { get; set; }
        //public Guid? SubmittedFormGuid { get; set; }
        //public Guid? ApplicantGuid { get; set; } 
        //public int DisplayOrder { get; set; } = 0; 
        //public bool Deleted { get; set; } = false;

        public long Id { get; set; }
        public Guid? Guid { get; set; }
        public string EntityGuid { get; set; }
        public string EntityId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Item { get; set; }

        public string Description { get; set; }
        public double DiscountPercentage { get; set; }
        public bool ShowInMenu { get; set; }
        public bool DiscountActive { get; set; }
        public int DisplayOrder { get; set; }
        public string FullNameEn { get; set; }
        public string FullNameAr { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public GenderType GenderTypeId { get; set; }
        public string EncryptedPassword { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string Token { get; set; }
        public string ImageName { get; set; }
        public string ImageUrl { get; set; }
        public long UserTypeId { get; set; }
        public bool Active { get; set; }
        public bool RoleActive { get; set; }
        //public string CompanyGuid { get; set; } 
        public  string __RequestVerificationToken { get; set; }
    }
}
