using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;
using Data.Model.General;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Utility.Models.Frontend.GeneralDto;

namespace Services.FrontEnd.GeneralWeb.PriceTypeWeb
{
    public class PriceTypeWebService : IPriceTypeWebService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly string ServiceName = "WebPriceTypeService";

        public PriceTypeWebService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<List<WebPriceTypeDto>> GetActivePriceTypes(int priceTypeCategoryId)
        {
            try
            {
                var items = await _dbcontext.PriceTypes
                    .Where(x => x.PriceTypeCategoryId == priceTypeCategoryId && x.Active && !x.Deleted)
                    .Select(x => new WebPriceTypeDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PriceTypeCategoryId = x.PriceTypeCategoryId
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + ": " + ex.Message);
                return new List<WebPriceTypeDto>();
            }
        }
    }
}
