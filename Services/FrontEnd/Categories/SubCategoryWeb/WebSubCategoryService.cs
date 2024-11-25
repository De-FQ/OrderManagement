using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;
using Data.Model.SiteCategory;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using Utility.Models.Frontend.Category;
using Services.FrontEnd.Categories.SubCategoryWeb;

namespace Services.Web.Categorys
{
    public class WebSubCategoryService : IWebSubCategoryService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly string ServiceName = "WebSubCategoryService";

        public WebSubCategoryService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<List<WebSubCategoryDto>> GetActiveSubCategories(int categoryId)
        {
            try
            {
                var items = await _dbcontext.SubCategories
                    .Where(x => x.CategoryId == categoryId && x.Active && !x.Deleted)
                    .Select(x => new WebSubCategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ImageName = x.ImageName,
                        CategoryId = x.CategoryId
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + ": " + ex.Message);
                return new List<WebSubCategoryDto>();
            }
        }
    }
}
