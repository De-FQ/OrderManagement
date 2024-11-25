using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Utility.Enum;
using Utility.Helpers;
using Utility.Models.Base;
namespace Utility.API
{
    /// <summary>
    /// API Helper
    /// </summary>
    public partial class APIHelper : IAPIHelper
    {
        private readonly AppSettingsModel _appSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string lang;
        public APIHelper(AppSettingsModel options, IHttpContextAccessor httpContextAccessor)
        {
            _appSettings = options;
            _httpContextAccessor = httpContextAccessor;
            lang = _appSettings.AppSettings.DefaultLang;
            if (!string.IsNullOrEmpty(System.Globalization.CultureInfo.CurrentCulture.Name))
            {
                lang = System.Globalization.CultureInfo.CurrentCulture.Name.ToUpper();
            }
        }
        #region API Header + ContentType
        private string GetContentType(PostContentType postContentType)
        {
            var contentType = "application/json";
            if (PostContentType.applicationJson == postContentType)
            {
                contentType = "application/json";
            }

            return contentType;
        }

        /// <summary>
        /// Append headers
        /// </summary>
        /// <param name="httpClient">HttpClient object</param>
        private HttpClient AppendHeader(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Add("key", _appSettings.JwtSettings.APIKey);
            httpClient.DefaultRequestHeaders.Add("lang", lang);
            httpClient.DefaultRequestHeaders.Add("user-agent", "web-admin");
            httpClient.DefaultRequestHeaders.Add("deviceTypeId", ((int)DeviceType.Web).ToString());
            httpClient.Timeout = TimeSpan.FromMinutes(_appSettings.HttpClientSettings.ExpirationMinutes);

            var token = GetToken(_httpContextAccessor.HttpContext);
            if (!string.IsNullOrEmpty(token))
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            return httpClient;
        }
        #endregion

        #region API Call
        /// <summary>
        /// Post async      
        /// </summary>
        /// <param name="url">API url</param>
        /// <param name="requestObj">Request object</param>
        public virtual async Task<T> PostAsync<T>(string url, object requestObj, PostContentType postContentType)
        {
            //T responseModel = default;
            using (var httpClient = new HttpClient())
            {
                AppendHeader(httpClient);
                var contentType = GetContentType(postContentType);
                var completeUrl = _appSettings.AppSettings.APIBaseUrl + url;
                StringContent content = new(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, contentType);
                using var response = await httpClient.PostAsync(completeUrl, content);
                return await GetResponseModel<T>(response, completeUrl);

            }

        }

        /// <summary>
        /// Post async      
        /// </summary>
        /// <param name="url">API url</param>
        /// <param name="requestObj">Request object</param>
        public virtual async Task<T> PostAsync<T>(string url, object requestObj, string baseUrl = "")
        {
            //T responseModel = default;
            using (var httpClient = new HttpClient())
            {
                AppendHeader(httpClient);

                StringContent content = new(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json");

                string completeUrl = string.Empty;
                if (!string.IsNullOrEmpty(baseUrl))
                    completeUrl = baseUrl + url;
                else
                    completeUrl = _appSettings.AppSettings.APIBaseUrl + url;

                using var response = await httpClient.PostAsync(completeUrl, content);
                return await GetResponseModel<T>(response, completeUrl);

            }

        }        

        /// <summary>
        /// Put async      
        /// </summary>
        /// <param name="url">API url</param>
        /// <param name="requestObj">Request object</param>
        public virtual async Task<T> PutAsync<T>(string url, object requestObj, string baseUrl = "")
        {
            //T responseModel = default;
            using (var httpClient = new HttpClient())
            {
                AppendHeader(httpClient);
                StringContent content = new(JsonConvert.SerializeObject(requestObj), Encoding.UTF8, "application/json");

                string completeUrl = string.Empty;
                if (!string.IsNullOrEmpty(baseUrl))
                    completeUrl = baseUrl + url;
                else
                    completeUrl = _appSettings.AppSettings.APIBaseUrl + url;

                using var response = await httpClient.PutAsync(completeUrl, content);
                return await GetResponseModel<T>(response, completeUrl);

            }

        }

        /// <summary>
        /// Delete async      
        /// </summary>
        /// <param name="url">API url</param>
        public virtual async Task<T> DeleteAsync<T>(string url, string baseUrl = "")
        {
            //T responseModel = default;
            using (var httpClient = new HttpClient())
            {
                AppendHeader(httpClient);

                string completeUrl = string.Empty;
                if (!string.IsNullOrEmpty(baseUrl))
                    completeUrl = baseUrl + url;
                else
                    completeUrl = _appSettings.AppSettings.APIBaseUrl + url;

                using var response = await httpClient.DeleteAsync(completeUrl);
                return await GetResponseModel<T>(response, completeUrl);

            }

        }

        /// <summary>
        /// Get async      
        /// </summary>
        /// <param name="url">API url</param>
        public virtual async Task<T> GetAsync<T>(string url, string baseUrl = "")
        {
            //T responseModel = default;
            using (var httpClient = new HttpClient())
            {
                AppendHeader(httpClient);

                string completeUrl = string.Empty;
                if (!string.IsNullOrEmpty(baseUrl))
                    completeUrl = baseUrl + url;
                else
                    completeUrl = _appSettings.AppSettings.APIBaseUrl + url;

                using var response = await httpClient.GetAsync(completeUrl);


                //if (response.Headers.TryGetValues("Set-Cookie", out IEnumerable<string> cdnCacheStatusList))
                //{
                //    foreach (var key in cdnCacheStatusList)
                //    {
                //        StringValues pair = key.Split("=");
                //        var keyName = pair[0];
                //        var keyValue = pair[1];
                //        if (keyName == "X-XSRF-TOKEN")
                //        {
                //            _httpContextAccessor.HttpContext.Response.Cookies.Append(keyName, keyValue);
                //        }
                //    }

                //}


                return await GetResponseModel<T>(response, completeUrl);

                //if (response.StatusCode.ToString() == "OK")
                //{
                //    string apiResponse = await response.Content.ReadAsStringAsync();
                //    responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                //}
                //else if (response.StatusCode.ToString() == "Unauthorized")
                //{
                //    string apiResponse = "{'success':false,'message':'Unauthorized','messageCode':401}";
                //    responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                //}
                //else if (response.StatusCode.ToString() == "BadRequest")
                //{
                //    string apiResponse = "{'success':false,'message':'BadRequest','messageCode':400}";
                //    responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                //}
                //else if (response.StatusCode.ToString() == "InternalServerError")
                //{
                //    string apiResponse = "{'success':false,'message':'InternalServerError','messageCode':500}";
                //    responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                //}
                //else
                //{
                //    string apiResponse = "{'success':false,'message':'BadRequest','messageCode':0}";
                //    responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                //}
            }

            //return responseModel;
        }

        public virtual async Task<T> GetResponseModel<T>(HttpResponseMessage response, string completeUrl)
        {
            T responseModel = default;
            if (response.StatusCode.ToString() == "OK")
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
            }
            else if (response.StatusCode.ToString() == "Unauthorized")
            {
                Log.Error(response.ToString() + ", url:" + completeUrl);
                string apiResponse = "{'success':false,'message':'Unauthorized','messageCode':401}";
                responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
            }
            else if (response.StatusCode.ToString() == "BadRequest")
            {
                Log.Error(response.ToString() + ", url:" + completeUrl);
                string apiResponse = "{'success':false,'message':'BadRequest','messageCode':400}";
                responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
            }
            else if (response.StatusCode.ToString() == "InternalServerError")
            {
                Log.Error(response.ToString() + ", url:" + completeUrl);
                string apiResponse = "{'success':false,'message':'InternalServerError','messageCode':500}";
                responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
            }
            else
            {
                Log.Error(response.ToString() + ", url:" + completeUrl);
                string apiResponse = "{'success':false,'message':'BadRequest','messageCode':0}";
                responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
            }
            return responseModel;
        }

        #endregion

        #region JWT Token

        public string GenerateAccessToken(IEnumerable<Claim> claims, AppSettingsModel _appSettings)
        {
            var tokenString = string.Empty;
            try
            {
                var APIKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.APIKey));
                var signingCredentials = new SigningCredentials(key: APIKey, algorithm: SecurityAlgorithms.HmacSha512);

                var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.SecretKey));
                var encryptingCredentials = new EncryptingCredentials(key: SecretKey, alg: SecurityAlgorithms.Aes128KW, enc: SecurityAlgorithms.Aes128CbcHmacSha256);

                var handler = new JwtSecurityTokenHandler();

                var jwtSecurityToken = handler.CreateJwtSecurityToken(
                    issuer: _appSettings.JwtSettings.Issuer,
                    audience: _appSettings.JwtSettings.Audience,
                    subject: new ClaimsIdentity(claims),
                    issuedAt: DateTime.Now,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddMinutes(_appSettings.JwtSettings.ExpirationMinutes),
                //    expires: DateTime.UtcNow.AddMinutes(), 
                   signingCredentials: signingCredentials,
                    encryptingCredentials: encryptingCredentials
                    );

                tokenString = handler.WriteToken(jwtSecurityToken);


            }
            catch (Exception exp)
            {
                tokenString = exp.Message;
            }

            return tokenString;
        }
        /// <summary>
        /// Get UserModel from Token
        /// 1. Convert token to ClaimsPrincipal
        /// 2. ClaimsPincipal to UserModel
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserModel GetUserModelFromToken(string token)
        {
            UserModel userModel = new();
            try
            {
                var APIKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.APIKey));
                var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.SecretKey));
                var tokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = APIKey,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = _appSettings.JwtSettings.Issuer,
                    ValidateIssuer = true,

                    ValidAudience = _appSettings.JwtSettings.Audience,
                    ValidateAudience = true,

                    ValidateLifetime = true,

                    // This is the decryption key
                    TokenDecryptionKey = SecretKey,
                    ClockSkew = TimeSpan.Zero
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.Aes128KW, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");
                userModel = GetClaimPrincipal(principal);

            }
            catch (Exception e)
            {
                var errorType = e.GetType();
                if (errorType == typeof(SecurityTokenExpiredException))
                {
                    userModel.TokenExpired = true;
                    userModel.Status = 401;
                    userModel.ErrorMessage = e.GetType().ToString();
                }
                else if (errorType == typeof(SecurityTokenException))
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }
                else if (errorType == typeof(SecurityTokenDecryptionFailedException))
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }

                else if (errorType == typeof(SecurityKeyIdentifierClause))
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }
                else
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }
            }
            return userModel;

        }
        /// <summary>
        /// new for testing
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="SecurityTokenException"></exception>
        public UserModel GetPrincipalFromExpiredToken_(string token)
        {
            UserModel userModel = new();
            try
            {
                var APIKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.APIKey));
                var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.SecretKey));


                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = APIKey,
                    ValidateIssuerSigningKey = true,
                    // This is the decryption key
                    TokenDecryptionKey = SecretKey,
                    ClockSkew = TimeSpan.Zero
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null)
                { throw new SecurityTokenException("Invalid token"); }
                if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.Aes128KW, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token");
                }

            }
            catch (Exception e)
            {
                var errorType = e.GetType();
                if (errorType == typeof(SecurityTokenExpiredException))
                {
                    userModel.TokenExpired = true;
                    userModel.Status = 401;
                    userModel.ErrorMessage = e.GetType().ToString();
                }
                else if (errorType == typeof(SecurityTokenException))
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }
                else if (errorType == typeof(SecurityTokenDecryptionFailedException))
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }

                else if (errorType == typeof(SecurityKeyIdentifierClause))
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }
                else
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }
            }
            return userModel;

        }
        public UserModel GetPrincipalFromExpiredToken(string token)
        {
            UserModel userModel = new();
            try
            {
                var APIKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.APIKey));
                var SecretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtSettings.SecretKey));

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = _appSettings.JwtSettings.Issuer,
                    ValidateIssuer = true,//you might want to validate the audience and issuer depending on your use case

                    ValidAudience = _appSettings.JwtSettings.Audience,
                    ValidateAudience = true, //you might want to validate the audience and issuer depending on your use case

                    IssuerSigningKey = APIKey,
                    ValidateIssuerSigningKey = true,

                    // This is the decryption key
                    TokenDecryptionKey = SecretKey,

                    //here we are saying that we don't care about the token's expiration date
                    ValidateLifetime = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.Aes128KW, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                userModel = GetClaimPrincipal(principal);

            }
            catch (Exception e)
            {
                var errorType = e.GetType();
                if (errorType == typeof(SecurityTokenExpiredException))
                {
                    userModel.TokenExpired = true;
                    userModel.Status = 401;
                    userModel.ErrorMessage = e.GetType().ToString();
                }
                else if (errorType == typeof(SecurityTokenException))
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }
                else if (errorType == typeof(SecurityTokenDecryptionFailedException))
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }

                else if (errorType == typeof(SecurityKeyIdentifierClause))
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }
                else
                {
                    userModel.ErrorMessage = e.GetType().ToString();
                }
            }
            return userModel;
        }


        public ClaimsPrincipal CreateClientCookieClaimsPrincipal(List<Claim> extraClaims, UserModel tokenUser, string accessToken)
        {
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();    
            try
            {
                //create cookie claims for client
                var claims = new List<Claim>() {
                            new Claim(ClaimTypes.Email, tokenUser.EmailAddress),
                            new Claim(ClaimTypes.Name, tokenUser.FullName),
                            new Claim(ClaimTypes.Role, tokenUser.RoleName),
                            //custom claim
                            new Claim(Constants.ClaimTypes.Id, tokenUser.Id),
                            new Claim(Constants.ClaimTypes.Guid, tokenUser.Guid),
                            new Claim(Constants.ClaimTypes.RoleId, tokenUser.RoleId),
                            new Claim(Constants.ClaimTypes.RoleTypeId, tokenUser.RoleTypeId),
                            new Claim(Constants.ClaimTypes.ImageUrl, tokenUser.ImageUrl),
                            new Claim(Constants.ClaimTypes.AuthenticationToken, accessToken),
                            new Claim(Constants.ClaimTypes.RefreshToken, tokenUser.RefreshToken)
                            };
                if (extraClaims.Count > 0)
                {
                    claims.AddRange(extraClaims);
                }
                //claims Identity
                var identity = new ClaimsIdentity(claims, Constants.Cookie.AuthenticationScheme);
                //var claimsprincipal 
                claimsPrincipal = new ClaimsPrincipal(identity);
            } catch (Exception ex)
            {
                Log.Error("Utility.APIHelper.CreateClientCookieClaimsPrincipal:", ex.Message);
            }
            return claimsPrincipal;
        }

        public List<Claim> CreateClaims(UserModel user, bool admin)
        {
            user.RefreshToken = Common.GenerateRefreshToken();
            var role = admin ? Constants.ClaimTypes.Admin : Constants.ClaimTypes.WebUser;
            var fullName = !string.IsNullOrEmpty(user.FullName) ? user.FullName : string.Empty;
            var email = !string.IsNullOrEmpty(user.EmailAddress) ? user.EmailAddress : string.Empty;
            List<Claim> claims = new()
                {

                    new Claim(JwtRegisteredClaimNames.Sub, fullName),
                    new Claim(ClaimTypes.Name, fullName),
                    new Claim(ClaimTypes.Email,email),
                    new Claim(ClaimTypes.Role,role),
                    //custom
                    new Claim(Constants.ClaimTypes.Id, user.Id.ToString()),
                    new Claim(Constants.ClaimTypes.Guid, user.Guid),
                    new Claim(Constants.ClaimTypes.RoleId, user.RoleId),
                    new Claim(Constants.ClaimTypes.RoleName,!string.IsNullOrEmpty(user.RoleName)? user.RoleName:string.Empty),
                    new Claim(Constants.ClaimTypes.RoleTypeId,!string.IsNullOrEmpty(user.RoleTypeId)? user.RoleTypeId:string.Empty),
                    new Claim(Constants.ClaimTypes.ImageUrl,!string.IsNullOrEmpty(user.ImageUrl)? user.ImageUrl:string.Empty),
                    new Claim(Constants.ClaimTypes.UserType, role),
                    new Claim(Constants.ClaimTypes.RefreshToken,  user.RefreshToken)
                    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    //new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                    //new Claim(Constants.ClaimTypes.Id, user.Id.ToString()), 
                    //new Claim(Constants.ClaimTypes.FullName ,!string.IsNullOrEmpty(user.FullName )? user.FullName :string.Empty),
                  
                };
            return claims;
        }

        public UserModel GetClaimPrincipal(ClaimsPrincipal claimsPrincipal)
        {
            UserModel user = new();
            if (claimsPrincipal.Claims.Any())
            {
                user.FullName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                user.EmailAddress = claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value;

                //custom claims
                user.Id = claimsPrincipal.FindFirst(Constants.ClaimTypes.Id)?.Value;
                user.Guid = claimsPrincipal.FindFirst(Constants.ClaimTypes.Guid)?.Value;
                user.RoleId = claimsPrincipal.FindFirst(Constants.ClaimTypes.RoleId)?.Value;
                user.RoleTypeId = claimsPrincipal.FindFirst(Constants.ClaimTypes.RoleTypeId)?.Value;
                user.RoleName = claimsPrincipal.FindFirst(Constants.ClaimTypes.RoleName)?.Value;
                user.ImageUrl = claimsPrincipal.FindFirst(Constants.ClaimTypes.ImageUrl)?.Value;
                user.RefreshToken = claimsPrincipal.FindFirst(Constants.ClaimTypes.RefreshToken)?.Value;
            }
            return user;
        }



        public UserModel GetClaimsFrom(HttpContext httpContext)
        {

            UserModel model = new();
            if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                model.FullName = httpContext.User.FindFirstValue(ClaimTypes.Name);
                model.EmailAddress = httpContext.User.FindFirstValue(ClaimTypes.Email);
                model.RoleName = httpContext.User.FindFirstValue(ClaimTypes.Role);
                //Custom claims
                model.Id = httpContext.User.FindFirstValue(Constants.ClaimTypes.Id);
                model.Guid = httpContext.User.FindFirstValue(Constants.ClaimTypes.Guid);
                model.RoleId = httpContext.User.FindFirstValue(Constants.ClaimTypes.RoleId);
                model.RoleTypeId = httpContext.User.FindFirstValue(Constants.ClaimTypes.RoleTypeId);
                model.ImageUrl = httpContext.User.FindFirstValue(Constants.ClaimTypes.ImageUrl);
                model.Token = httpContext.User.FindFirstValue(Constants.ClaimTypes.AuthenticationToken);
                model.RefreshToken = httpContext.User.FindFirstValue(Constants.ClaimTypes.RefreshToken);
            }
            return model;
        }


        public string GetToken(HttpContext httpContext)
        {
            string token = string.Empty;
            if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                token = httpContext.User.FindFirstValue(Constants.ClaimTypes.AuthenticationToken);
            }
            return token;
        }

        public string GetRefreshToken(HttpContext httpContext)
        {
            string refreshToken = string.Empty;
            if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                refreshToken = httpContext.User.FindFirstValue(Constants.ClaimTypes.RefreshToken);
            }
            return refreshToken;
        }

        public string GetRoleId(HttpContext httpContext)
        {
            string roleId = string.Empty;
            if (httpContext.User.Identity != null && httpContext.User.Identity.IsAuthenticated)
            {
                roleId = httpContext.User.FindFirstValue(Constants.ClaimTypes.RoleId);
            }
            return roleId;
        }


        public virtual string GetUserIP()
        {
            var result = string.Empty;
            try
            {
                //first try to get IP address from the forwarded header
                if (_httpContextAccessor.HttpContext.Request.Headers != null)
                {
                    var forwardedHttpHeaderKey = "X-FORWARDED-FOR";

                    var forwardedHeader = _httpContextAccessor.HttpContext.Request.Headers[forwardedHttpHeaderKey];
                    if (!StringValues.IsNullOrEmpty(forwardedHeader))
                        result = forwardedHeader.FirstOrDefault();
                }

                //if this header not exists try get connection remote IP address
                if (string.IsNullOrEmpty(result) && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                    result = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return string.Empty;
            }

            return result;
        }


        #endregion


        #region Social Media Rest API
        public virtual async Task<T> GetAsyncBare<T>(string url)
        {

            T responseModel = default(T);
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(url))
                {
                    if (response.StatusCode.ToString() == "OK")
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                    }
                    else if (response.StatusCode.ToString() == "Unauthorized")
                    {
                        string apiResponse = "{'success':false,'message':'Unauthorized','messageCode':401}";
                        responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                    }
                    else if (response.StatusCode.ToString() == "BadRequest")
                    {
                        string apiResponse = "{'success':false,'message':'BadRequest','messageCode':400}";
                        responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                    }
                    else if (response.StatusCode.ToString() == "Forbidden")
                    {
                        string apiResponse = "{'success':false,'message':'Forbidden','messageCode':403}";
                        responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                    }
                    else
                    {
                        string apiResponse = "{'success':false,'message':'BadRequest','messageCode':0}";
                        responseModel = JsonConvert.DeserializeObject<T>(apiResponse);
                    }
                }
            }

            return responseModel;
        }
        #endregion

    }
}
