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
    public class UsersService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
            {
                throw new ArgumentException("User id mismatch");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    throw new KeyNotFoundException($"User with id {id} not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<User> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                Name = createUserDto.Name,
                Email = createUserDto.Email
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found");
            }

            var defaultUserId = _configuration.GetValue<int>("AppSettings:DefaultUserId");

            var tasksToUpdate = await _context.Tasks.Where(t => t.UserId == id).ToListAsync();
            foreach (var task in tasksToUpdate)
            {
                task.UserId = defaultUserId;
            }

            await _context.SaveChangesAsync();
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
