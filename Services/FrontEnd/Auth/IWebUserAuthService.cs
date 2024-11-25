using Data.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.FrontEnd.Auth
{
    public interface IWebUserAuthService
    {
        Task<bool> AuthenticateUser(string emailAddress, string password);
        Task<User> GetUserByEmail(string emailAddress);
        Task<IEnumerable<User>> GetUsers();
    }
}
