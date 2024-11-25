using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utility.API;
using Utility.Helpers;
using Utility.Models.Admin.UserManagement;
using Utility.Models.Base;

namespace Admin.ViewComponents
{
    namespace Admin.ViewComponents
    {
        public class NavigationHeaderViewComponent : ViewComponent
        {
            private readonly IAPIHelper _apiHelper; 
            public NavigationHeaderViewComponent( IAPIHelper apiHelper ) 
            {
                _apiHelper = apiHelper; 
            }


            public IViewComponentResult Invoke()
            {
                if (HttpContext.User.Identity.IsAuthenticated) {  
                    UserModel model = _apiHelper.GetClaimsFrom(HttpContext);
                    
                    return View(model);
                } else
                {
                    return Content("");
                }
            }
        }
    }
}
