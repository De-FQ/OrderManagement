using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Enum;
using Utility.Helpers;

namespace Data.Base
{



    /// <summary>
    /// One extra image is missing 
    ///     for example when user click on attraction menu, there is a one banner image, 
    ///         we will upload this image from <b>MENU</b>
    /// </summary>
    public partial class BaseIconImage : BaseCommon
    {
       



        //It will be used for landing page thumbnail.
        [StringLength(Constants.DataSize.ImageName)]
        public string ThumbnailImageName { get; set; }

        [NotMapped]
        public string ThumbnailImageUrl { get; set; }

        [NotMapped]
        public IFormFile ThumbnailImage { get; set; }





    }
}
