using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SuperComUserTasks_.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }

    }
}