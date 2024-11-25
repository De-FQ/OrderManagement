using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;

namespace Utility.Models.Frontend.HomePage
{
    public class CareerApplicationFormDto
    {
        public string CareerSeo { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public long CountryId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Religion { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public int NumberOfChildren { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string IDNumber { get; set; }
        public AcademicDegree HighestQualificationHeld { get; set; }
        public bool Experienced { get; set; }
        public int ExperienceInYears { get; set; }
        public string Category { get; set; }
        public long? CareerId { get; set; }
        public JobType JobType { get; set; }
        public JoiningDate JoiningDate { get; set; }
        public string Others { get; set; }
        public string SkillsAndCompetencies { get; set; }
        public LanguageFluency EnglishFluency { get; set; }
        public LanguageFluency ArabicFluency { get; set; }
        public string OtherLanguages { get; set; }
        public bool Kuwaiti { get; set; }
        public string UploadCV { get; set; }
        public string IdDocument { get; set; }
        public string EducationalQualification { get; set; }
        public string OtherAttachment { get; set; }        
        //public ICollection<ExperienceDetail> ExperienceDetails { get; set; }

        [NotMapped]
        public IFormFile UploadCVFile { get; set; }       

        [NotMapped]
        public IFormFile IdDocumentFile { get; set; }      

        [NotMapped]
        public IFormFile EducationalQualificationFile { get; set; }   

        [NotMapped]
        public IFormFile OtherAttachmentFile { get; set; }
        public string __RequestVerificationToken { get; set; }
        public string X_XSRF_TOKEN { get; set; }
    }
}
