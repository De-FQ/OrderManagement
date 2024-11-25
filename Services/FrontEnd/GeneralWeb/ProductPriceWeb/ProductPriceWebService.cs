using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;
using Data.Model.General;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Utility.Models.Frontend.GeneralDto;

namespace Services.FrontEnd.GeneralWeb.ProductPriceWeb
{
    public class ProductPriceWebService : IProductPriceWebService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly string ServiceName = "WebProductPriceService";

        public ProductPriceWebService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<List<WebProductPriceDto>> GetActiveProductPrices(int childCategoryId)
        {
            try
            {
                var items = await _dbcontext.ProductPrices
                    .Where(x => x.ChildCategoryId == childCategoryId && x.Active && !x.Deleted)
                    .Select(x => new WebProductPriceDto
                    {
                        Id = x.Id,
                        ChildCategoryId = x.ChildCategoryId,
                        PriceTypeId = x.PriceTypeId,
                        PriceTypeCategoryId = x.PriceTypeCategoryId,
                        Price = x.Price
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + ": " + ex.Message);
                return new List<WebProductPriceDto>();
            }
        }
    }
}
