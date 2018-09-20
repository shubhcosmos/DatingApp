using System;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;

        public AuthRepository(DataContext context)
        {
            this._context = context;

        }
        public Task<User> Login(string username, string password)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> Register(User user, string password)
        {
            byte[] passwordHash , passwordSalt ;

            CreatePasswordHash(password , out passwordHash , out  passwordSalt);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            throw new NotImplementedException();
        }

        public Task<User> UserExists(string username)
        {
            throw new System.NotImplementedException();
        }
    }
}