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

namespace Services.Web.Categorys
{
    public class WebCategoryService : IWebCategoryService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly string ServiceName = "WebCategoryService";

        public WebCategoryService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<List<WebCategoryDto>> GetActiveCategories()
        {
            try
            {
                var items = await _dbcontext.Categories
                    .Where(x => x.Active && !x.Deleted)
                    .Select(x => new WebCategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ImageName = x.ImageName
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + ": " + ex.Message);
                return new List<WebCategoryDto>();
            }
        }
    }
}
