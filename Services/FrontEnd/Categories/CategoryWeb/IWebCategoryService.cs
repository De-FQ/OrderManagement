using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Model.SiteCategory;
using Utility.Models.Frontend.Category;

namespace Services.Web.Categorys
{
    public interface IWebCategoryService
    {
        Task<List<WebCategoryDto>> GetActiveCategories();
    }
}
