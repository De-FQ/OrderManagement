using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Utility.Models.Admin.UserManagement;
using Utility.Models.Base;

namespace Utility.API
{
    public enum PostContentType
    {
        applicationJson = 1,
        /// <summary>
        /// input parameter [FromForm]
        /// </summary>
    }
    public interface IAPIHelper
    {
        /// <summary>
        /// Post async      
        /// </summary>
        /// <param name="url">API url</param>
        /// <param name="requestObj">Request object</param>
        Task<T> PostAsync<T>(string url, object requestObj, PostContentType postType);        

        /// <summary>
        /// Post async      
        /// </summary>
        /// <param name="url">API url</param>
        /// <param name="requestObj">Request object</param>
        /// <param name="includeBaseUrl">Need to include base url</param>
        /// <param name="baseUrl">Base url</param>
        Task<T> PostAsync<T>(string url, object requestObj, string baseUrl = "");

        /// <summary>
        /// Put async      
        /// </summary>
        /// <param name="url">API url</param>
        /// <param name="requestObj">Request object</param>
        Task<T> PutAsync<T>(string url, object requestObj, string baseUrl = "");

        /// <summary>
        /// Delete async      
        /// </summary>
        /// <param name="url">API url</param>
        Task<T> DeleteAsync<T>(string url, string baseUrl = "");

        /// <summary>
        /// Get async      
        /// </summary>
        /// <param name="url">API url</param>
        Task<T> GetAsync<T>(string url, string baseUrl = "");

        Task<T>  GetResponseModel<T>(HttpResponseMessage message,string completeUrl);
        /// <summary>
        /// Get user ip address      
        /// </summary>
        /// <param name="url">API url</param>
        string GetUserIP();

        #region Client Cookie
        ClaimsPrincipal CreateClientCookieClaimsPrincipal(List<Claim> extraClaims, UserModel tokenUser, string accessToken);
        #endregion
        #region Jwt Token
        List<Claim> CreateClaims(UserModel user, bool admin);
        string GenerateAccessToken(IEnumerable<Claim> claims, AppSettingsModel _appSettings);

        //string GenerateRefreshToken(string email);
        UserModel GetClaimPrincipal(ClaimsPrincipal claimsPrincipal);
        //AuthResult GetUserInfo(AuthResult authResult, bool? admin = null);
        //  string GenerateRefreshToken();
        UserModel GetUserModelFromToken(string token);
        UserModel GetPrincipalFromExpiredToken(string token);
        UserModel GetClaimsFrom(HttpContext httpContext);
        string GetToken(HttpContext httpContext);
        string GetRefreshToken(HttpContext httpContext);
        string GetRoleId(HttpContext httpContext);

        #endregion

        #region Social Media Rest API
        Task<T> GetAsyncBare<T>(string url);
        #endregion

    }
}

