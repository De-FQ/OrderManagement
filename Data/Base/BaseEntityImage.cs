using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Helpers;

namespace Data.Base
{
    public partial class BaseImage : BaseCommon
    {
        [StringLength(Constants.DataSize.ImageName)]
        public string ImageName { get; set; }

        [NotMapped]
        public string ImageUrl { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
