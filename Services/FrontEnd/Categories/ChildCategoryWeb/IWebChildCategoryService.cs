using System.Collections.Generic;
using System.Threading.Tasks;
using Utility.Models.Frontend.Category;

namespace Services.FrontEnd.Categories.ChildCategoryWeb
{
    public interface IWebChildCategoryService
    {
        Task<List<WebChildCategoryDto>> GetActiveChildCategories(int subCategoryId);
    }
}
