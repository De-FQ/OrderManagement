using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Frontend.HomePage
{
    public class InfomationRequestFormDto
    {


        public string Name { get; set; }

        public string CivilId { get; set; }


        public string EmailAddress { get; set; }

        public string MobileNumber { get; set; }
        public string InformationDetails { get; set; }

        public string ApplicationDocument { get; set; }
        public IFormFile ApplicationDocumentFile { get; set; }

        public string CivilIdDocument { get; set; }
        public IFormFile CivilIdDocumentFile { get; set; }

        public string SignatoryDocument { get; set; }
        public IFormFile SignatoryDocumentFile { get; set; }


        public string __RequestVerificationToken { get; set; }
        public string X_XSRF_TOKEN { get; set; }


    }
}
