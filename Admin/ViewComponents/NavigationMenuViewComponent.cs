
using Data.UserManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Utility.API;
using Utility.Helpers;
using Utility.ResponseMapper;

namespace Admin.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IAPIHelper _apiHelper;
        public NavigationMenuViewComponent( IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                //var claims = _apiHelper.GetClaimsFrom(HttpContext);
                //if (!string.IsNullOrEmpty(claims.Token))
                //{
                    var responseModel = await _apiHelper.GetAsync<ResponseMapper<List<UserPermission>>>("Common/GetMenuList");
                    if(responseModel !=null && responseModel.StatusCode == 250)
                    {
                        HttpContext.Response.Redirect(Constants.Redirection.AccountLogin);
                    }
                    else if (responseModel.Success && responseModel.Data != null)
                    {
                        //var access = HttpContext.Response.Headers;
                        //var cookies = HttpContext.Response.Cookies;
                        ////string menuPermission = JsonConvert.SerializeObject(responseModel.Data);
                        //// HttpContext.Session.SetString("menuPermission", menuPermission);

                        return View(responseModel.Data);
                    }
               // }
            } 
            return Content("");
          
        }
    }
}
