using Entities;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepo;

        public UserService(IRepository<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var users= await _userRepo.GetAllAsync();
            return users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public async Task RegisterAsync(User user)
        {
           await _userRepo.AddAsync(user);
           await _userRepo.SaveAsync();
        }
    }
}
