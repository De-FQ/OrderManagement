using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Frontend.HomePage
{
    public class GetInTouchDto
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNumber { get; set; }
        public string Comments { get; set; }

    }
}
