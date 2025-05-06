using Microsoft.EntityFrameworkCore;
using TaskMeUp.Api.Entities;

namespace TaskMeUp.Api.DAL
{
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
    }
}
