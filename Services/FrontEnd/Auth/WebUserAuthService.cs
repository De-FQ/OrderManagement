using AutoMapper;
using Data.EntityFramework;
using Data.UserManagement;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Helpers;

namespace Services.FrontEnd.Auth
{
    public class WebUserAuthService : IWebUserAuthService
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly IMapper _mapper;
        private readonly string ServiceName = "WebCategoryService";

        public WebUserAuthService(ApplicationDbContext dbcontext, IMapper mapper)
        {
            _dbcontext = dbcontext;
            _mapper = mapper;
        }

        public async Task<bool> AuthenticateUser(string emailAddress, string password)
        {
            var user = await _dbcontext.Users
                .Include(x => x.Roles)
                .Where(x => x.EmailAddress.ToLower() == emailAddress.ToLower())
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user != null)
            {
                var decryptedPassword = new EncryptionServices().DecryptString(user.EncryptedPassword);

                // Assuming user.Roles is a single object, not a collection
                if (decryptedPassword == password && user.Roles.Name == "Cashier")
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<User?> GetUserByEmail(string emailAddress)
        {
            return await _dbcontext.Users
                .Include(x => x.Roles)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.EmailAddress.ToLower() == emailAddress.ToLower());
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _dbcontext.Users
                    .Where(x => x.Deleted == false)
                    .ToListAsync();
        }

    }
}
