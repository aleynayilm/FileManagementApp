using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task RegisterAsync(User user);
    }
}
