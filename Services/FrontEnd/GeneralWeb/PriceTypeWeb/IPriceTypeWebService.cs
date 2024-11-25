using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Frontend.GeneralDto;

namespace Services.FrontEnd.GeneralWeb.PriceTypeWeb
{
    public interface IPriceTypeWebService
    {
        Task<List<WebPriceTypeDto>> GetActivePriceTypes(int priceTypeCategoryId);
    }
}
