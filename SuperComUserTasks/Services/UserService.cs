using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SuperComUserTasks_.Models;
using Task = System.Threading.Tasks.Task;

namespace SuperComUserTasks_.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
            {
                return false;
            }

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
            {
                return false;
            }

            if (_context.Users.Any(u => u.Email == user.Email && u.Id != id))
            {
                return false;
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<User> CreateUserAsync(CreateUserDto createUserDto)
        {
            if (_context.Users.Any(u => u.Email == createUserDto.Email))
            {
                return null; // Email conflict
            }

            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<bool> DeleteUserAsync(int id, int defaultUserId)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            var tasksToUpdate = await _context.Tasks.Where(t => t.UserId == id).ToListAsync();
            foreach (var task in tasksToUpdate)
            {
                task.UserId = defaultUserId;
            }

            await _context.SaveChangesAsync();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
