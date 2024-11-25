using Admin.Extensions;
using Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Diagnostics;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.LoggerService;
using Utility.Models.Base;
using Utility.Models.Frontend.HomePage;
using Utility.ResponseMapper;

namespace Admin.Controllers
{
    //[Route("[controller]/[action]")](policy: Utility.Helpers.Constants.Roles.Root)(Roles= Utility.Helpers.Constants.Roles.RootOrAdministrator)
 //   [Authorize]
    public class HomeController : BaseController
    {
     
        public HomeController(
            IRazorViewEngine razorViewEngine,
            AppSettingsModel options,
            ILoggerFactory logger,
            IAPIHelper apiHelper) :
            base(razorViewEngine, options, 
                logger.CreateLogger(typeof(HomeController).Name),   apiHelper)
        { }

        [Route("~/")]
        [Route("/Home")]
        [Route("~/Home/Index")]
        public IActionResult Index()
        { 
            return View();

        }


        
        public IActionResult Privacy()
        {
            return View();
        }
        
        
    }
}