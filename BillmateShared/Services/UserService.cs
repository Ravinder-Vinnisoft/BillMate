using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BillMate.Data;
using BillMate.Helpers;
using BillMate.Models;
using Microsoft.EntityFrameworkCore;
//using WebApi.Entities;
//using WebApi.Helpers;

namespace BillMate.Services
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password, BillMateDBContext _context);
        //Task<IEnumerable<User>> GetAll();
    }

    public class UserService : IUserService
    {
        private List<User> _users = new List<User>();

        public async Task<User> Authenticate(string username, string password, BillMateDBContext _context)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);

            // return null if user not found
            if (user == null)
                return null;

            if(user.Password != Encryption.CalculateMD5Hash(password))
            {
                return null;
            }

            // authentication successful so return user details without password
            var client = new Client();

            if(user.Role == "Client")
            {
                client = _context.Client.Where(x => x.UserId == user.Id).FirstOrDefault();
            }

            if(client != null)
            {
                user.ClientId = client.Id;
            }

            return user;
        }

        //public async Task<IEnumerable<User>> GetAll()
        //{
        //    return await Task.Run(() => _users.WithoutPasswords());
        //}
    }
}
