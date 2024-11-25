
using Admin.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Dynamic;
using System.Text;
using System.Web;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.Models.Admin.UserManagement;
using Utility.Models.Base;
using Utility.ResponseMapper;

namespace Admin.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        protected ILogger Logger;
        protected AdminUserPermissionModel permission;
        protected int _permissionType;
        IRazorViewEngine _razorViewEngine; 
        protected readonly AppSettingsModel _appSettings; 
        protected readonly IAPIHelper _apiHelper;
        protected string FullName = "Unknow";
        protected int UserId = 0;
        protected int RoleId = 0;
        protected string ImagePath = "";
        public BaseController(  IRazorViewEngine razorViewEngine,
                                AppSettingsModel  options,
                                ILogger logger, 
                                IAPIHelper apiHelper,
                                PermissionTypes permissionType = (int)PermissionTypes.None)
        {
            _razorViewEngine= razorViewEngine;
            Logger = logger;
            _appSettings = options; 
            _apiHelper = apiHelper;
            _permissionType = (int)permissionType;

        }

        protected string GetRequestViewPath()
        {
            return  "~/Views/"+ Request.Path.ToString() + ".cshtml";
        }
        /// <summary>
        /// Get all permission for specific permissionType, List,Add,Edit,DisplayOrder,Delete
        /// </summary>
        /// <returns></returns>
        protected async Task   GetPermission()
        {
            
              Task.WaitAll(); 
            ResponseMapper<AdminUserPermissionModel> response = await _apiHelper.GetAsync<ResponseMapper<AdminUserPermissionModel>>("User/GetViewPermission?permissionId=" + _permissionType);
            if (response == null || response.Data == null)
            {
                this.permission = new();
                await Task.Yield();
            }
            else
            {
                permission = response.Data;
            }
            await  Task.CompletedTask;
        }

        //[HttpGet, Route("List")]
        //public virtual async Task<ViewResult> List()
        //{
        //    await GetPermission();
        //    if (permission.AllowView == false)
        //    {
        //        return GetViewBy(Constants.Redirection.AccountAccessDenied);
        //    }
        //    return View(permission);
        //}

        //public virtual async Task<IActionResult> List()
        //{
        //    await GetPermission();
        //    if ( permission.AllowView == false)
        //    {
        //        return  GetViewBy(Constants.Redirection.AccountAccessDenied);
        //    }
        //    return View(permission.GetAllowActive());
        //}

        /// <summary>
        /// Make the base class method, public virtual ...() 
        /// <br></br>Make the derived class method , public override ....() 
        /// <br></br>This will only execute the derived method when it hits the derived Controller.
        /// <br></br>If you don't implement the method in the derived Controller ,then the base method will be called.
        /// <br></br><b>If you don't make the base controller virtual , 
        /// both of methods will be called and lead to routing ambiguous exception.</b>
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //[HttpGet, Route("AddEdit/{guid?}")]
        //public virtual async Task<ViewResult> AddEdit(Guid? guid)
        //{
        //    await GetPermission();
        //    if (guid == null &&  permission.AllowAdd == false)
        //    {
        //        return  GetViewBy(Constants.Redirection.AccountAccessDenied);
        //    }
        //    else if (guid.HasValue && permission.AllowEdit == false)
        //    {
        //        return GetViewBy(Constants.Redirection.AccountAccessDenied);
        //    }
        //    if (!guid.HasValue) guid = Guid.Empty;
        //    return View(new BaseModel { Guid = guid});
        //}
        
        protected async Task<ViewResult> ListView(string viewPath)
        {
            await GetPermission();
            if (permission.AllowList == false)
            {
                return GetViewBy(Constants.Redirection.AccountAccessDenied);
            }
            return View(viewPath, permission);
        } 
        protected async Task<ViewResult> AddEditView(Guid? guid, string viewPath)
        {
            await GetPermission();

            if (guid == null && permission.AllowAdd == false)
            {
                return GetViewBy(Constants.Redirection.AccountAccessDenied);
            }
            else if (guid.HasValue && permission.AllowEdit == false)
            {
                return GetViewBy(Constants.Redirection.AccountAccessDenied);
            }
            else
            {
                if (!guid.HasValue) guid = Guid.Empty;
                return View(viewPath, new BaseModel { Guid = guid });
            }
        }
        
        /// <summary>
        /// Render partial view to string
        /// https://khalidabuhakmeh.com/searched-locations-razor-view-engine-aspdotnet
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model">Model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        protected virtual async Task<string> RenderPartialViewToStringAsync(string viewName, object model)
        {
            //create action context
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor, ModelState);

            //set view name as action name in case if not passed
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            //set model
            ViewData.Model = model;

            //try to get a view by the name
            var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);
            if (viewResult.View == null)
            {
                //or try to get a view by the path
                viewResult = _razorViewEngine.GetView(null, viewName, false);
                if (viewResult.View == null)
                    throw new ArgumentNullException($"{viewName} view was not found");
            }
            await using var stringWriter = new StringWriter();
            var viewContext = new ViewContext(actionContext, viewResult.View, ViewData, TempData, stringWriter, new HtmlHelperOptions());

            await viewResult.View.RenderAsync(viewContext);
            return stringWriter.GetStringBuilder().ToString();
        }
        /// <summary>
        /// https://khalidabuhakmeh.com/searched-locations-razor-view-engine-aspdotnet
        /// </summary>
        /// <param name="viewName"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected virtual ViewEngineResult RenderViewAsync(string viewName, object model = null)
        {
            

            //create action context
            var actionContext = new ActionContext(HttpContext, RouteData, ControllerContext.ActionDescriptor, ModelState);

            //set view name as action name in case if not passed
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            //add view extension if not provided
            if (!viewName.EndsWith(".cshtml")) viewName += ".cshtml"; 

            //set model
            ViewData.Model = model;

            //try to get a view by the name
            var viewResult = _razorViewEngine.FindView(actionContext, viewName, false); 
            if (viewResult.View == null)
            {
                //or try to get a view by the path
                viewResult = _razorViewEngine.GetView(null, viewName, false);
                if (viewResult.View == null)
                    throw new ArgumentNullException($"{viewName} view was not found");
            }

            return viewResult;
        }

        protected virtual ViewResult GetViewBy(string viewPath)
        {
            if (!viewPath.StartsWith("/Views")) viewPath  = "/Views"+ viewPath;
            //add view extension if not provided
            if (!viewPath.EndsWith(".cshtml")) viewPath += ".cshtml";

            var viewResult = View(viewPath);
            return viewResult;
        }

        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
