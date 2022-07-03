using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Persistence
{
    public class UserRepository
    {
        private readonly RentContext _context;
        public UserRepository(RentContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUser(Guid Id) 
        {
            return await _context.Users.FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<User> CreateUser(User User)
        {
            _context.Users.Add(User);
            await _context.SaveChangesAsync();
            return User;
        }

        public async Task<User> UpdateUser(User User)
        {
            _context.Users.Update(User);
            await _context.SaveChangesAsync();
            return User;
        }

        public async Task<User> DeleteUser(User User)
        {
            _context.Users.Remove(User);
            await _context.SaveChangesAsync();
            return User;
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(m => m.Email == email);
        }
    }
}