using Microsoft.EntityFrameworkCore;
using TaskMeUp.Api.Entities;
using Task = TaskMeUp.Api.Entities.Task;

namespace TaskMeUp.Api.DAL
{
    public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Task> Tasks { get; set; } = null!;
    }
}
