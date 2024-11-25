using Data.EntityFramework;
using Microsoft.AspNetCore.SignalR;
using Utility.API;
using System.Text.Json;
using Utility.Models.Frontend.HomePage;

namespace API.Hubs
{
    /// <summary>
    /// <para>for ApplicationDbContext Database should be connection prior in program.cs after 
    /// scope builder.AddDatabaseContext();</para>
    /// https://www.youtube.com/watch?v=pl0OobPmWTk
    /// </summary>

    public class WebHub : Hub
    {
        protected readonly ApplicationDbContext _dbcontext;
        protected readonly AppSettingsModel _options; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WebHub(ApplicationDbContext context, AppSettingsModel options, IHttpContextAccessor httpContextAccessor)
        {
            _dbcontext = context;
            _options =  options; 
        }
         

        public override Task OnConnectedAsync()
        {
            //var x = Context.ConnectionId;
            Clients.User(Context.ConnectionId).SendAsync("connected","data").GetAwaiter().GetResult();
            return base.OnConnectedAsync();
        }
         
        public override Task OnDisconnectedAsync(Exception exception)
        {
            
            return base.OnDisconnectedAsync(exception);
        }
        public async Task NewWindowLoaded()
        {
             
            //send update to all clients that total views have been updated
            await Clients.All.SendAsync("connected", 1);
        }     
        private bool getLang(string lang)
        {
            if (lang == null) { return true; }
            if (lang == "en") { return true; };
            return false;
        }
        
    }
}