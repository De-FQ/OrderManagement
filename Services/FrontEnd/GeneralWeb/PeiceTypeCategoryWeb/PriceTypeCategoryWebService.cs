using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;
using Data.Model.General;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Utility.Models.Frontend.Category;

using Utility.Models.Frontend.GeneralDto;

namespace Services.FrontEnd.GeneralWeb.PeiceTypeCategoryWeb
{
    public class PriceTypeCategoryWebService : IPriceTypeCategoryWebService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly string ServiceName = "WebPriceTypeCategoryService";

        public PriceTypeCategoryWebService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<List<WebPriceTypeCategoryDto>> GetActivePriceTypeCategories(int childCategoryId)
        {
            try
            {
                var items = await _dbcontext.PriceTypeCategories
                    .Where(x => x.ChildCategoryId == childCategoryId && x.Active && !x.Deleted)
                    .Select(x => new WebPriceTypeCategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        ChildCategoryId = x.ChildCategoryId
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + ": " + ex.Message);
                return new List<WebPriceTypeCategoryDto>();
            }
        }
    }
}
