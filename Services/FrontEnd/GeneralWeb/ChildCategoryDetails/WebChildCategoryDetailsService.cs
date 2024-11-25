using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;
using Data.Model.SiteCategory;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Utility.Models.Frontend.Category;
using Utility.Models.Admin.DTO;
using Utility.Models.Frontend.GeneralDto;
using Services.FrontEnd.GeneralWeb.ChildCategoryDetails;

namespace Services.Web.Categorys
{
    public class WebChildCategoryDetailsService : IWebChildCategoryDetailsService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly string ServiceName = "WebChildCategoryDetailsService";

        public WebChildCategoryDetailsService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<ChildCategoryDetailsDto> GetChildCategoryDetails(int childCategoryId)
        {
            try
            {
                var childCategory = await _dbcontext.ChildCategories
                    .Where(c => c.Id == childCategoryId && c.Active) 
                    .Select(c => new ChildCategoryDetailsDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ImageName = c.ImageName, 
                        PriceTypeCategories = c.PriceTypeCategories
                            .Where(ptc => ptc.Active)
                            .Select(ptc => new WebPriceTypeCategoryDto
                            {
                                Id = ptc.Id,
                                Name = ptc.Name,
                                PriceTypes = ptc.PriceTypes
                                    .Where(pt => pt.Active)
                                    .Select(pt => new WebPriceTypeDto
                                    {
                                        Id = pt.Id,
                                        Name = pt.Name,
                                        ProductPrices = _dbcontext.ProductPrices
                                            .Where(pp => pp.PriceTypeId == pt.Id && pp.Active)
                                            .Select(pp => new WebProductPriceDto
                                            {
                                                Id = pp.Id,
                                                Price = pp.Price
                                            }).ToList()
                                    }).ToList()
                            }).ToList()
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync();

                return childCategory;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + ": " + ex.Message);
                return null;
            }
        }
    }
}
