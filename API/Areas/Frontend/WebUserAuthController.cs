using API.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.FrontEnd.Auth;
using System;
using System.Threading.Tasks;
using Utility.API;
using Utility.Helpers;
using Utility.Models.Base;

namespace API.Areas.Frontend
{
    [Route("webapi/[controller]")]
    [ApiController]
    [AutoValidateAntiforgeryToken]
    public class WebUserAuthController : ControllerBase
    {
        private readonly IWebUserAuthService _webUserAuthService;
        private readonly IMapper _mapper;
        private readonly IAPIHelper _apiHelper;
        private readonly AppSettingsModel _appSettings;
        private readonly ILogger<WebUserAuthController> _logger;

        public WebUserAuthController(IWebUserAuthService webUserAuthService, IMapper mapper, IAPIHelper apiHelper, AppSettingsModel appSettings, ILogger<WebUserAuthController> logger)
        {
            _webUserAuthService = webUserAuthService;
            _mapper = mapper;
            _apiHelper = apiHelper;
            _appSettings = appSettings;
            _logger = logger;
        }

        [HttpPost, Route("Login")]
        [AllowAnonymous, IgnoreAntiforgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            AuthResponse response = new() { StatusCode = 200 };
            try
            {
                if (string.IsNullOrEmpty(loginModel.EmailAddress) || string.IsNullOrEmpty(loginModel.Password))
                {
                    response.Message = "User does not exist.";
                    response.StatusCode = -1;
                    return Ok(response);
                }

                bool isAuthenticated = await _webUserAuthService.AuthenticateUser(loginModel.EmailAddress, loginModel.Password);

                if (!isAuthenticated)
                {
                    response.Message = "Invalid login message.";
                    response.StatusCode = -1;
                    return Ok(response);
                }

                var user = await _webUserAuthService.GetUserByEmail(loginModel.EmailAddress);

                if (user == null || user.Deleted || !user.Active)
                {
                    response.Message = user.Deleted ? "User is deleted." : "User is inactive.";
                    response.StatusCode = -1;
                    return Ok(response);
                }

                // Store the user's name and role in session
                HttpContext.Session.SetString("UserName", user.FullName);
                HttpContext.Session.SetString("UserRole", user.Roles.Name);

                response.Success = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = "An error occurred during login.";
                response.StatusCode = -1;
                _logger.LogError(ex.Message, Serilog.Events.LogEventLevel.Error);
            }

            return Ok(response);
        }


    }
}
