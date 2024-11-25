using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Frontend.HomePage
{

    public class EventListDto
    {
        public int SNo { get; set; }
        public DateTime EventDate { get; set; }
        public string FormattedEventDate { get; set; }
        public int TotalEventCount { get; set; }
        public List<EventsDto> Events { get; set; }

    }


    public class EventsDto
    {
        public string SeoName { get; set; }
        public string ShortName { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string FormattedEventDate { get; set; }
        public long DisplayOrder { get; set; }

    }
}
