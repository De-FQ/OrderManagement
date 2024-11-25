using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Frontend.GeneralDto;

namespace Services.FrontEnd.GeneralWeb.ProductPriceWeb
{
    public interface IProductPriceWebService
    {
        Task<List<WebProductPriceDto>> GetActiveProductPrices(int childCategoryId);
    }
}
