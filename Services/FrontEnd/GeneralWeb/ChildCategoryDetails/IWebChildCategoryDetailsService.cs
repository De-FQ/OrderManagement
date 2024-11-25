using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Models.Frontend.GeneralDto;

namespace Services.FrontEnd.GeneralWeb.ChildCategoryDetails
{
    public interface IWebChildCategoryDetailsService
    {
        Task<ChildCategoryDetailsDto> GetChildCategoryDetails(int childCategoryId);
    }
}
