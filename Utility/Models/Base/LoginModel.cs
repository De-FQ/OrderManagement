using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Models.Base
{
    public class LoginModel 
    { 
        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter Email Address")]
        [EmailAddress(ErrorMessage = "Please enter valid Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }

        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool RememberMe { get; set; }
        public bool IsLoginSucceeded { get; set; }
        public string ReturnUrl { get; set; }
    }
}
