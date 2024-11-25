using System;
using System.Collections.Generic;
using Utility.Enum;

namespace Utility.Models.Frontend.Category
{
    public class CategoryDetailDto
    {
        public Guid Guid { get; set; }
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string LongDescription { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public string PreviousRedirectionURL { get; set; } = string.Empty;
        public string NextRedirectionURL { get; set; } = string.Empty;
        public string DetailURL { get; set; }

        public string FacebookLink { get; set; }
        public string TwitterLink { get; set; }
        public string WhatsAppLink { get; set; }

        public MediaFileType MediaFileType { get; set; }
        public string BannerURL { get; set; } = string.Empty;
        public string BannerPosterURL { get; set; } = string.Empty;

        public string GoogleLocationLink { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string CategoryTypeSeoName { get; set; }
        public string CategoryTypeName { get; set; }

        public List<GalleryDto> Galleries { get; set; }
    }

    public class CategoryModelDto
    {
        public Guid Guid { get; set; }
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string WorkingHour { get; set; }
        public string ThumbImageURL { get; set; } = string.Empty;
    }

    public class GalleryDto
    {
        public string MediaFileURL { get; set; }
        public MediaFileType MediaFileType { get; set; }
        public string ThumbImageURL { get; set; }
        public int DisplayOrder { get; set; }
    }
}
