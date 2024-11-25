using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Enum;

namespace Utility.Models.Frontend.HomePage
{
    public class ActivityTypeDto
    {
        public string SeoName { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; } = string.Empty;
        public string RedirectURL { get; set; } = string.Empty;
    }

}
