using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.API;

namespace Utility.Models.Admin.CategoryModel
{
    public class SubCategorySearchParamModel
    {
        public long? CategoryId { get; set; }
        public string? Name { get; set; }
        public bool? Active { get; set; }

        public DataTableParam DatatableParam { get; set; }
    }
}
