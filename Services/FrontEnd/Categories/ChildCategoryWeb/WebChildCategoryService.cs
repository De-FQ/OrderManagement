using Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;
using Data.Model.SiteCategory;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Utility.Models.Frontend.Category;
using Services.FrontEnd.Categories.ChildCategoryWeb;

namespace Services.Web.Categorys
{
    public class WebChildCategoryService : IWebChildCategoryService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly string ServiceName = "WebChildCategoryService";

        public WebChildCategoryService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<List<WebChildCategoryDto>> GetActiveChildCategories(int subCategoryId)
        {
            try
            {
                var items = await _dbcontext.ChildCategories
                    .Where(x => x.SubCategoryId == subCategoryId && x.Active && !x.Deleted)
                    .Select(x => new WebChildCategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Description = x.Description,
                        ImageName = x.ImageName,
                        SubCategoryId = x.SubCategoryId
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return items;
            }
            catch (Exception ex)
            {
                Log.Error(ServiceName + ": " + ex.Message);
                return new List<WebChildCategoryDto>();
            }
        }
    }
}
