using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Frontend.Category;

namespace Services.FrontEnd.Categories.SubCategoryWeb
{
    public interface IWebSubCategoryService
    {
        Task<List<WebSubCategoryDto>> GetActiveSubCategories(int categoryId);
    }
}
