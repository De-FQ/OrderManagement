using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;
using Utility.Models.Admin.DTO;
using Utility.Models.Base;

namespace Utility.Models.Frontend.Events
{

    public class EventsLandingDto
    {
        public string BannerImageURL { get; set; } = string.Empty;
        public MediaFileType MediaFileType { get; set; }
        public string BannerPosterURL { get; set; } = string.Empty;
        public List<DropdownListDto> Activities { get; set; }
        public List<DropdownListDto> Attractions { get; set; }
        public List<EventsSummaryDto> Events { get; set; }
    }

    public class EventsLandingFilterDto
    {
        public List<EventsSummaryDto> Events { get; set; }
    }


    public class EventsSummaryDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public string FormatedEventDate { get; set; }
        public string FormatedTime { get; set; }

        public string ActivityType { get; set; }
        public long DisplayOrder { get; set; }

        public long ActivityTypeId { get; set; }
        public long AttractionId { get; set; }

    }



   

}