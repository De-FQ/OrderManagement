using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;
using Utility.Models.Base;

namespace Utility.Models.Frontend.Category
{

    public class CategoryLandingListDto
    {
        public Guid Guid { get; set; }
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string BannerImageURL { get; set; } = string.Empty;
        public MediaFileType MediaFileType { get; set; }
        public string PosterFileUrl { get; set; }
        public string LongDescription { get; set; } = string.Empty;
        public List<DropdownListDto> SubCategories { get; set; }
        public List<CategoryItemDto> Categories { get; set; }
    }
}
