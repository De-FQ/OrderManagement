using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;

namespace Utility.Models.Frontend.Project
{

    public class ProjectLandingDto
	{
        public string BannerImageURL { get; set; } = string.Empty;
        public List<LandingDto> Projects { get; set; }
    }

    public class LandingDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ThumbImageURL { get; set; } = string.Empty;
    }

}