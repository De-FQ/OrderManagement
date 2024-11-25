using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Base;

namespace Utility.Models.Admin.DTO
{
    public class PriceTypeCategoryDto : BaseEntityImageDto
    {
        public string Name { get; set; }
        public int ChildCategoryId { get; set; }
    }
}
