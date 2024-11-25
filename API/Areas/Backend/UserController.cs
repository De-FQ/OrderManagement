using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Services.Backend.UserManagement;
using UAParser;
using Utility.API;
using Utility.Enum;
using Utility.Helpers;
using Utility.ResponseMapper;
using Utility.Models.Base;
using API.Helpers;
using Utility.Models.Admin.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Antiforgery;
using Serilog;
using System.IO;
using API.Extensions;
using Data.UserManagement;

namespace API.Areas.Backend.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class UserController : BaseController
    { 
         private readonly IMapper _mapper;
        //private readonly IAntiforgery _antiforgery;
        private readonly IWebHostEnvironment _webHostEnvironment;
        IUserRoleService<UserRole> _userRoleService;
        public UserController(
            AppSettingsModel options,
            IAPIHelper apiHelper,
            IUserService<User> systemUserService,
            IUserRoleService<UserRole> userRoleService,
            IWebHostEnvironment webHostEnvironment
            ,  IMapper mapper
            ) :
             base(options, apiHelper, systemUserService, PermissionTypes.Users)
        {
            base.ControllerName = typeof(UserController).Name;
            _webHostEnvironment = webHostEnvironment;
             _mapper = mapper;  
            _userRoleService = userRoleService;
        }
         
        #region Login Admin
        
        [HttpPost, Route("api/User/LoginAdmin")]
        [AllowAnonymous,IgnoreAntiforgeryToken] 
        public async Task<IActionResult> LoginAdmin([FromBody] LoginModel loginModel)
        {
            AuthResponse response = new() { StatusCode = 200};
            EncryptionServices encryptionServices = new();
            try
            {
                // var activeUserCount = await base.UserService.GetAllUserCount();
                //if (activeUserCount == 0) //initialized Database if no users exists
                //{
                //    await InitializeDatabase();
                //}
                UserHistory UserHistory = new()
                {
                    Email = loginModel.EmailAddress,
                    Password = loginModel.Password
                };

                if (UserHistory.Email == null || UserHistory.Password == null)
                {
                    // Resource helper class is set for the localization of En and Ar Language
                    UserHistory.Description = ResourceHelper.GetResource("user_not_exist", IsEnglish); // Please enter email and password.
                    response.Message = UserHistory.Description;
                    response.StatusCode = -1;
                    await base.UserService.AddUserHistory(UserHistory);

                    return Ok(response);
                }

                
                var user = await base.UserService.GetByEmail(loginModel.EmailAddress);
                if (user == null || user.EmailAddress == null || user.EncryptedPassword == null) // no user exits with provided email
                {
                    UserHistory.Description = ResourceHelper.GetResource("user_not_exist", IsEnglish);
                    response.Message = UserHistory.Description;
                    response.StatusCode = -1;
                    await base.UserService.AddUserHistory(UserHistory);

                    return Ok(response);
                }
                else
                {
                   // var md = encryptionServices.EncryptString("Media123");
                    //compare password with user provided with encryptedPassword in User Table
                    var isValidPassword = encryptionServices.ComparePassword(loginModel.Password, user.EncryptedPassword);
                    if (isValidPassword == false) // entered wrong password 
                    {
                        UserHistory.Description = ResourceHelper.GetResource("invalid_login_message", IsEnglish);
                        response.Message = UserHistory.Description;
                        response.Title = ResourceHelper.GetResource("invalid_login_details", IsEnglish);
                        response.StatusCode = -1;
                        await base.UserService.AddUserHistory(UserHistory);
                        return Ok(response);
                    }

                    if (user.Deleted) // user exits with deleted status
                    {
                        UserHistory.Description = ResourceHelper.GetResource("user_is_deleted", IsEnglish);
                        response.Message = UserHistory.Description;
                        response.StatusCode = -1;
                        await base.UserService.AddUserHistory(UserHistory);
                        return Ok(response);
                    }

                    if (!user.Active) // user exits with inActive status
                    {
                        UserHistory.Description = ResourceHelper.GetResource("user_in_active", IsEnglish);
                        response.Message = UserHistory.Description; 
                        await base.UserService.AddUserHistory(UserHistory);
                        response.StatusCode = -1;
                        return Ok(response);
                    }
                     //prepare user image if exists otherwise set default image url 
                    if (!string.IsNullOrEmpty(user.ImageName))
                       user.ImageUrl = base.GetImageUrl(base.AppSettings.ImageSettings.Users, user.ImageName);
                    else
                        user.ImageUrl = base.GetImageUrl(base.AppSettings.ImageSettings.Users, "default.png");
                     
                }
                
                  
                var systemUserModel = _mapper.Map<UserModel>(user); 
                if (user.Roles != null)
                {
                    systemUserModel.RoleId = user.Roles.Id.ToString();
                    systemUserModel.RoleTypeId = user.Roles.UserRoleTypeId.ToString();
                    systemUserModel.RoleName = user.Roles.Name.ToString();
                } else
                {
                    systemUserModel.RoleId = string.Empty;
                    systemUserModel.RoleTypeId = string.Empty;
                    systemUserModel.RoleName = string.Empty;
                }
                var claims = _apiHelper.CreateClaims(systemUserModel, admin: true); // Checks the claim is from valid user or not
                var accessToken = _apiHelper.GenerateAccessToken(claims, base.AppSettings); // Generate token which create JWT Token handler and make the user authenticate and authorize for next step.

                UserHistory.Description = ResourceHelper.GetResource("Login_success", IsEnglish);
                UserHistory.UserId = user.Id;
                await base.UserService.AddUserHistory(UserHistory);

                UserRefreshToken refreshToken = new() 
                { 
                    RefreshToken = systemUserModel.RefreshToken, 
                    Email = user.EmailAddress, 
                    Guid = user.Guid.Value,
                    UserId = user.Id 
                };

                //**********************************************************************************
                //*      Antiforgery Token
                ////**********************************************************************************
                //     var tokens = HttpContext.RequestServices.GetRequiredService<IAntiforgery>();
                //     AntiforgeryTokenSet token = tokens.GetAndStoreTokens(HttpContext);
                //response.X_XSRF_TOKEN = "";// token.RequestToken;
                //**********************************************************************************

                await base.UserService.UpdateRefreshToken(refreshToken.Email, user.Id, systemUserModel.RefreshToken, DateTime.Now.AddDays(1));
                var isDev = _webHostEnvironment.IsDevelopment();
                  
                CommonServiceExtensions.UseAntiforgeryTokenInResponse(HttpContext, "api/User/LoginAdmin", isDev, base.AppSettings);
                response.AccessToken = accessToken;
                response.RefreshToken = systemUserModel.RefreshToken; 
                response.Success = true;
                return Ok(response); 
            }
            catch (Exception ex)
            { 
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }
            return Ok(response);
        }
       
        // Refresh(string token, string refreshToken)
        [AllowAnonymous]
        [HttpPost, Route("api/User/refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshData data)
        {
            if(data is null)
            {
                return BadRequest("Invalid client request");
            }
            string accessToken = data.AccessToken;
            string refreshToken = data.RefreshToken;

            RefreshResult refreshResult = new(); 
            UserModel tokenInfo = _apiHelper.GetPrincipalFromExpiredToken(accessToken);
            
            var user = await base.UserService.GetByEmail(tokenInfo.EmailAddress);
            if(user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid client request");
            }

           
            
            ////invalid refresh token
            //if (data.RefreshToken != tokenInfo.RefreshToken)
            //{       
            //    accessResponse.Message = "Token is Expired, please login again";
            //    accessResponse.Success = false;
            //    accessResponse.StatusCode = 250; 
            //    return Ok(accessResponse);
            //}
            

            if (user != null)
            {
                var systemUserModel = _mapper.Map<UserModel>(user);
                //user last login refreshToken is not same as requsted refresh token
                if (user == null || user.RefreshToken != tokenInfo.RefreshToken)
                {
                    return Unauthorized("Invalid attempt!");
                }
                if (user.Roles != null)
                {
                    systemUserModel.RoleId = user.Roles.Id.ToString();
                    systemUserModel.RoleTypeId = user.Roles.UserRoleTypeId.ToString();
                    systemUserModel.RoleName = user.Roles.Name.ToString();
                }
                else
                {
                    systemUserModel.RoleId = string.Empty;
                    systemUserModel.RoleTypeId = string.Empty;
                    systemUserModel.RoleName = user.Roles.Name.ToString();
                }
                
                var claims = _apiHelper.CreateClaims(systemUserModel, admin: true);
                var newAccessToken = _apiHelper.GenerateAccessToken(claims, base.AppSettings);
                 

                //systemUserModel.RefreshToken = Common.GenerateRefreshToken();
                //authResult = _apiHelper.CreateJwtToken(systemUserModel, this.AppSettings, admin: false);

                //var savedRefreshToken = await UserService.GetRefreshToken(systemUserModel.EmailAddress);

                // saving refresh token to the db
                UserRefreshToken  newRefreshToken = new()
                {
                    RefreshToken = systemUserModel.RefreshToken,
                    Email = user.EmailAddress,
                    Guid = user.Guid.Value,
                    UserId = user.Id
                };

                CommonServiceExtensions.UseAntiforgeryTokenInResponse(HttpContext, "api/User/LoginAdmin", false, base.AppSettings);

                refreshResult.AccessToken = newAccessToken;
                refreshResult.RefreshToken = systemUserModel.RefreshToken;
                refreshResult.Success = true;
                //var isDev = _webHostEnvironment.IsDevelopment();
                //CommonServiceExtensions.UseAntiforgeryTokenInResponse(HttpContext, "user/login", isDev);
                await base.UserService.UpdateRefreshToken(newRefreshToken.Email, user.Id, newRefreshToken.RefreshToken, DateTime.Now.AddDays(1));
                 
               // return Ok(refreshResult);
            }
            return Ok(refreshResult);
        }
         
        [HttpPost, Route("api/User/revoke/{email}")]
        public async Task<IActionResult> Revoke(string email)
        {
            if (!await Allowed())
            {
                return Ok(accessResponse);
            }
            //var user = new User() { EmailAddress = email, ModifiedBy = this.UserId }; 
            
            await base.UserService.UpdateRefreshToken(email, this.UserId, "", DateTime.Now);

            return Ok("Success");
        }
         
        [HttpPost, Route("api/User/revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            if (!await Allowed())
            {
                return Ok(accessResponse);
            }
            await UserService.RevokeAllRefreshToken();
            return Ok("Success");
        }
        #endregion

        #region System User Management
       
        

        [HttpGet, Route("api/User/GetByGuid")]
        public async Task<IActionResult> GetByGuid(Guid guid)
        {
            ResponseMapper<User> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Users, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }

                var item = await base.UserService.GetByGuid(guid);
                item = BuildUrl(item);
                response.GetById(item);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
       
        [HttpPost, Route("api/User/AddEdit")]
        public async Task<IActionResult> AddEdit([FromForm] User item)
        {
            ResponseMapper<User> response = new();
            try
            {
               
                if (!await base.AccessPermission(PermissionTypes.Users, AllowPermission.Edit)) { return Ok(accessResponse); }

                if (await UserService.Exists(item.EmailAddress, item.Guid))
                {
                    accessResponse.Message = item.EmailAddress + ResourceHelper.GetResource("email_is_already_taken", IsEnglish);
                    accessResponse.Success = false;
                    accessResponse.StatusCode = 300;
                    return Ok(accessResponse);
                }

                SaveImage(ref item);
                if (item.Guid.HasValue)
                {
                    if (!await base.AccessPermission(PermissionTypes.Users, AllowPermission.Edit)) { return Ok(accessResponse); }
                    item.ModifiedBy = this.UserId;
                    await base.UserService.Update(item);
                    response.Update(item);
                    response.Message = item.FullName + ResourceHelper.GetResource("user_edited_successfully", IsEnglish);
                }
                else
                {
                    if (!await base.AccessPermission(PermissionTypes.Users, AllowPermission.Add)) { return Ok(accessResponse); }

                    item.Guid = Guid.NewGuid();
                    item.CreatedBy = this.UserId;
                    await base.UserService.Create(item);

                    response.Create(item);
                    response.Message = item.FullName + ResourceHelper.GetResource("user_created_successfully", IsEnglish);
                }
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpPost, Route("api/User/UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromForm] Guid guid, string password)
        {
            ResponseMapper<bool> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Users, AllowPermission.Edit))
                {
                    return Ok(accessResponse);
                }
                 
                var user = new User() { Guid=guid, Password =password, ModifiedBy=this.UserId};
                var item = await base.UserService.UpdatePassword(user);
                response.ToggleActive(item);

            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }
            return Ok(response);
        }

        [HttpPost, Route("api/User/ToggleActive")]
        public async Task<IActionResult> ToggleActiveAsync(Guid guid)
        {
            ResponseMapper<User> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Users, AllowPermission.Active))
                {
                    return Ok(accessResponse);
                }
                 

                var item = await base.UserService.ToggleActive(guid);
                response.ToggleActive(item);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }
            return Ok(response);
        }
        [HttpPost, Route("api/User/GetForDataTable")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GetForDataTable()
        {
            ResponseMapper<dynamic> response = new();
             
            try
            { 
                if (!await base.AccessPermission(PermissionTypes.Users, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }
                 
                SystemUserSearchParamModel systemUserSearchParamModel = new();
                systemUserSearchParamModel.RoleTypeId = this.RoleTypeId;
                //Dynamic
                var items = await base.UserService.GetForDataTable(param: base.GetDataTableParameters, SearchParam: systemUserSearchParamModel);
                foreach (var item in items.Data)
                {
                    //item.FullName = item.FullName;
                    item.ImageUrl = GetBaseImageUrl(AppSettings.ImageSettings.Users) + (string.IsNullOrEmpty(item.ImageName) ? "default.png" : item.ImageName);
                    item.FormattedRegisteredBy = await this.UserService.GetForDisplay(item.CreatedBy, item.CreatedOn, isEnglish: IsEnglish);
                    item.FormattedLastLogin = await this.UserService.GetLastLoginWithDate(item.Id);
                }

                return Ok(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        [HttpDelete, Route("api/User/Delete")]
        public async Task<IActionResult> DeleteAsync(Guid guid)
        {
            ResponseMapper<User> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Users, AllowPermission.Delete))
                {
                    return Ok(accessResponse);
                }

                //var systemUser = await base.UserService.GetByGuid(guid);
                //if (systemUser != null)
                //{
                var item = await base.UserService.Delete(guid);
                    response.Delete(item);
                //}
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/User/ForRoleDropDownList")]
        public async Task<IActionResult> ForRoleDropDownList()
        {
            ResponseMapper<dynamic> response = new();
            try
            {
                if (!await base.AccessPermission(PermissionTypes.Users, AllowPermission.List))
                {
                    return Ok(accessResponse);
                }

                var items = await UserService.GetAllForDropDownList(IsEnglish);
                response.GetAll(items);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }

        [HttpGet, Route("api/User/GetViewPermission")]
        public async Task<IActionResult> GetViewPermission(int permissionId)
        {
            ResponseMapper<AdminUserPermissionModel > response = new();
            try
            {
                //AuthenticateUser is called due to get logged-in user Role Id only
                var auth = AuthenticateUser; 
                var item = await UserService.GetViewPermissionBy(this.RoleId, permissionId);
               
                response.GetById(item);
            }
            catch (Exception ex)
            {
                response.CacheException(ex);
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }
        #endregion


        #region Functions
        private User BuildUrl(User item)
        {
            if (!string.IsNullOrEmpty(item.ImageName))
            {
                item.ImageUrl = GetImageUrl(AppSettings.ImageSettings.Users, item.ImageName);
            }

            return item;
        }
        private void SaveImage(ref User item)
        {
            if (item.Image != null && item.Image.Length > 0)
            {
                 string fileName =   MediaHelper.ConvertImageToWebp(item.ImageName, item.Image, AppSettings.ImageSettings.Users);
                if (!string.IsNullOrEmpty(fileName))
                    item.ImageName = fileName;
            }
        }
        private async Task LoginHistory(UserModel userModel)
        {
            var _Headers = HttpContext.Request.Headers["User-Agent"];
            var _Parser = Parser.GetDefault();
            try
            {
                ClientInfo _ClientInfo = _Parser.Parse(_Headers);

                UserHistory loginHistory = new()
                {
                    UserId = int.Parse(userModel.Id),
                    Browser = _ClientInfo.UA.Family,
                    OperatingSystem = _ClientInfo.OS.Family,
                    Device = _ClientInfo.Device.Family,
                    ActionStatus = "Success"
                };

                await base.UserService.AddUserHistory(loginHistory);
            }
            catch (Exception ex)
            {
                LogInfo(ex.Message, Serilog.Events.LogEventLevel.Error);
            }
        }
        private string GetIPAddress()
        {
            // get source ip address for the current request
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
        #endregion


    }
}

