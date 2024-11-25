using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Utility.Helpers;

namespace Utility.Models.Base
{
    public partial class BaseEntityImageDto : BaseEntityCommonDto
    { 
        public string ImageName { get; set; } 
        public string ImageUrl { get; set; }  
    }
}
