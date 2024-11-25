using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Utility.Helpers;
using Utility.API;
using Utility.Models.Base;
using System.Security.Claims;

namespace Web.Controllers
{
    public class UserAuthController : Controller
    {
        protected ILogger Logger;
        protected readonly AppSettingsModel _appSettings;
        protected readonly IEncryptionServices _encryptionServices;
        protected readonly IAPIHelper _apiHelper;
        public UserAuthController(AppSettingsModel options,
                                ILoggerFactory logger,
                                IEncryptionServices encryptionServices,
                                IAPIHelper apiHelper)
        {
            Logger = logger.CreateLogger(typeof(UserAuthController).Name);
            _appSettings = options;
            _encryptionServices = encryptionServices;
            _apiHelper = apiHelper;
        }

        [AutoValidateAntiforgeryToken]
        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            var loginUser = new LoginModel();
            ViewData["ReturnUrl"] = ReturnUrl;
            loginUser.IsLoginSucceeded = false;
            loginUser.ReturnUrl = ReturnUrl;
            ModelState.Remove(string.Empty);
            ExpireAllCookies();
            return View(loginUser);
        }

        [HttpPost]
        public async Task<JsonResult> Login(LoginModel loginModel)
        {
            var loginResponseModel = new LoginResponseModel();
            if (!ModelState.IsValid)
            {
                loginResponseModel.Message = "Validation Error";
                return Json(loginResponseModel);
            }

            var responseModel = await _apiHelper.PostAsync<AuthResponse>("WebUserAuth/Login", loginModel, PostContentType.applicationJson);

            if (responseModel == null)
            {
                loginResponseModel.Message = "Validation Error";
                return Json(loginResponseModel);
            }

            loginResponseModel.Message = responseModel.Message;
            loginResponseModel.Title = responseModel.Title;

            if (responseModel.Success && responseModel.AccessToken != null)
            {
                if (!string.IsNullOrEmpty(responseModel.AccessToken.ToString()))
                {
                    UserModel tokenUser = _apiHelper.GetUserModelFromToken(responseModel.AccessToken);
                    if (tokenUser is not null)
                    {
                        responseModel.Success = await SignInAsyncWithCookie(tokenUser, responseModel.AccessToken);
                        if (responseModel.Success)
                        {
                            if (string.IsNullOrEmpty(loginModel.ReturnUrl) || loginModel.ReturnUrl == "/")
                            {
                                loginResponseModel.RedirectionUrl = Constants.Redirection.HomeCategory;
                                loginResponseModel.Success = true;
                            }
                            else
                            {
                                loginResponseModel.RedirectionUrl = loginModel.ReturnUrl;
                                loginResponseModel.Success = true;
                            }
                        }


                    }
                }
            }
            if (string.IsNullOrEmpty(loginResponseModel.RedirectionUrl))
            {
                Response.Redirect("/Home/Category");
            }


            return Json(loginResponseModel);
        }


        
        [HttpGet]
        public IActionResult SessionExpired()
        {
            return View();
        }

        /// <summary>
        /// This method will be called automatically by javascript 
        /// inside SessionExpired.cshtml page, after making few seconds delay
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AutoLogin()
        {
            var accessToken = string.Empty;
            var refreshToken = string.Empty;
            if (HttpContext.Request.Cookies.ContainsKey(Constants.ClaimTypes.AuthenticationToken))
                accessToken = HttpContext.Request.Cookies[Constants.ClaimTypes.AuthenticationToken];
            if (HttpContext.Request.Cookies.ContainsKey(Constants.ClaimTypes.RefreshToken))
                refreshToken = HttpContext.Request.Cookies[Constants.ClaimTypes.RefreshToken];


            if (!string.IsNullOrEmpty(accessToken) && !string.IsNullOrEmpty(refreshToken))
            {
                RefreshData request = new()
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                };
                var refreshResult = await _apiHelper.PostAsync<RefreshResult>("User/refresh-token", request, PostContentType.applicationJson);
                if (refreshResult != null && refreshResult.Success)
                {
                    if (!string.IsNullOrEmpty(refreshResult.AccessToken.ToString()))
                    {
                        UserModel tokenUser = _apiHelper.GetUserModelFromToken(refreshResult.AccessToken);
                        if (tokenUser is not null && tokenUser.TokenExpired == false)
                        {

                            refreshResult.Success = await SignInAsyncWithCookie(tokenUser, refreshResult.AccessToken);
                            if (refreshResult.Success)
                            {
                                return Json(new { url = Constants.Redirection.HomeCategory }); // "/Home/Index" 
                            }

                        }

                    }
                }
            }
            return Json(new { url = Constants.Redirection.AccountSignOut });
        }

        private async Task<bool> SignInAsyncWithCookie(UserModel tokenUser, string accessToken)
        {
            //var ExpiryTime = DateTimeOffset.UtcNow.AddMinutes(_appSettings.CookieSettings.ExpirationMinutes);
            var ExpiryTime = DateTime.Now.AddDays(_appSettings.CookieSettings.ExpirationDays);
            //increase AccessToken + RefreshToken time to avaoid early expiry of both Cookies
            // RefreshToken will be used to get the fresh token


            //to keep the session time longer then Cookie expiry time, so autologin can work automatically
            // var domain = HttpContext.Request.Path.ToString().ToLower().Contains("localhost") ? "localhost" : ".gtechnosoft.com"
            var options = new CookieOptions() { Expires = DateTime.Now.AddDays(2) };
            ExpireAllCookies();

            var extraClaims = new List<Claim>
            {
                 new Claim("Department","HR"), //its extra claim in admin cookie
            };
            var claimsprincipal = _apiHelper.CreateClientCookieClaimsPrincipal(extraClaims, tokenUser, accessToken);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,

                //ExpiryTime,//
                ExpiresUtc = ExpiryTime,

                IsPersistent = true,

                IssuedUtc = DateTime.Now,
                // The time at which the authentication ticket was issued.

                //RedirectUri = "/Account/UnAuthorized"
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };


            await HttpContext.SignInAsync(Constants.Cookie.AuthenticationScheme, claimsprincipal, authProperties);
            Response.Cookies.Append(Constants.ClaimTypes.AuthenticationToken, accessToken, options);
            Response.Cookies.Append(Constants.ClaimTypes.RefreshToken, tokenUser.RefreshToken, options);

            return true;
        }

        [HttpGet]
        public new async Task<IActionResult> SignOut()
        {
            //RemoveCookie();

            await HttpContext.SignOutAsync(Constants.Cookie.AuthenticationScheme);
            return Redirect(Constants.Redirection.AccountLogin);// RedirectToAction("Login", "Account");

        }

        //[HttpGet]
        //public void RemoveCookie()
        //{
        //    ExpireAllCookies();
        //    //if (HttpContext.Request.Cookies.ContainsKey(Constants.ClaimTypes.AuthenticationToken))
        //    //    HttpContext.Response.Cookies.Delete(Constants.ClaimTypes.AuthenticationToken);
        //    //if (HttpContext.Request.Cookies.ContainsKey(Constants.ClaimTypes.RefreshToken))
        //    //    HttpContext.Response.Cookies.Delete(Constants.ClaimTypes.RefreshToken);
        //    ////X-XSRF-TOKEN
        //    //if (HttpContext.Request.Cookies.ContainsKey(Constants.ClaimTypes.X_XSRF_TOKEN))
        //    //    HttpContext.Response.Cookies.Delete(Constants.ClaimTypes.X_XSRF_TOKEN);
        //}
        [HttpGet]
        private void ExpireAllCookies()
        {
            if (HttpContext.Request != null)
            {

                //int cookieCount = HttpContext.Request.Cookies.Count;
                foreach (var item in HttpContext.Request.Cookies)
                {
                    HttpContext.Response.Cookies.Delete(item.Key);
                    //var expiredCookie = new HttpCookie(item.Key) { Expires = DateTime.Now.AddDays(-1) };
                    //HttpContext.Current.Response.Cookies.Add(expiredCookie);
                }
                //for (var i = 0; i < cookieCount; i++)
                //{

                //    var cookie = HttpContext.Request.Cookies[i.ToString()];
                //    if (cookie != null)
                //    {
                //        var cookieName = cookie.ToString();
                //        HttpContext.Response.Cookies.Delete(cookieName);
                //        //var expiredCookie = new HttpCookie(cookieName) { Expires = DateTime.Now.AddDays(-1) };
                //       // HttpContext.Current.Response.Cookies.Add(expiredCookie);
                //    }
                //}

                // HttpContext.Request.Cookies.

            }
        }
        //[HttpGet]
        //public IActionResult AccessDenied()
        //{
        //    return View();
        //}

        public class RefreshData
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }
        }


        //[HttpGet]
        //public IActionResult UnAuthorized()
        //{
        //    return View();
        //}

        [HttpGet]
        public IActionResult ErrorWithMessage(string message)
        {
            return View(message);
        }

    }
}