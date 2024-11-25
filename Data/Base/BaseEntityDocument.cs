using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Helpers;

namespace Data.Base
{
    public partial class BaseDocument : BaseCommon
    {
        [StringLength(Constants.DataSize.ImageName)]
        public string DocumentName { get; set; }

        [NotMapped]
        public string DocumentUrl { get; set; }

        [NotMapped]
        public IFormFile Document { get; set; }
    }
}
