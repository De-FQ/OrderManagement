using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Frontend.GeneralDto;

namespace Services.FrontEnd.GeneralWeb.PeiceTypeCategoryWeb
{
    public interface IPriceTypeCategoryWebService
    {
        Task<List<WebPriceTypeCategoryDto>> GetActivePriceTypeCategories(int childCategoryId);
    }
}
