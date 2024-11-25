
using Data.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Utility.API;
using Utility.Models.Base;
using Utility.Models.Frontend.HomePage;

namespace API.Hubs
{
    /// <summary>
    /// <para>for ApplicationDbContext Database should be connection prior in program.cs after 
    /// scope builder.AddDatabaseContext();</para>
    /// https://www.youtube.com/watch?v=pl0OobPmWTk
    /// </summary>
    [Authorize]
    public class UserHub : Hub
    {
        protected readonly ApplicationDbContext _dbcontext; 
        private readonly APIHelper _apiHelper; 
        public UserHub(ApplicationDbContext context, AppSettingsModel options, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = context;
            _apiHelper = new APIHelper(options, httpContextAccessor);  
        }

        public static int TotalViews { get; set; }
        public static int TotalUsers { get; set; }

        public override Task OnConnectedAsync()
        {
            TotalUsers++;
            UserModel user=new();
            if (Context.User.Claims.Any())
            {
                user = _apiHelper.GetClaimPrincipal( Context.User);
            }
            var count =   _dbcontext.Users.Where(o => o.Active).AsNoTracking().CountAsync().Result;
            //send update to all clients that total views have been updated
            //var users = _dbcontext.Users.Where(x => x.Active).;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            TotalUsers--;
            Clients.All.SendAsync("updateTotalUsers", TotalUsers).GetAwaiter().GetResult();
            return base.OnDisconnectedAsync(exception);
        }

        public async Task NewWindowLoaded()
        {
            TotalViews++;
            //send update to all clients that total views have been updated
            await Clients.All.SendAsync("updateTotalViews", TotalViews);
        }

        public async Task ConnectedUser()
        {
            TotalUsers++;
            //send update to all clients that total views have been updated
            await Clients.Caller.SendAsync("ConnectedUser", TotalViews);
        }

        
    }
}