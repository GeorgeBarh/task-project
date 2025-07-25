using Microsoft.EntityFrameworkCore;
using TaskMeUp.Api.DAL;
using TaskMeUp.Api.Entities;
using TaskMeUp.Api.Interfaces.Repositories;
using Task = System.Threading.Tasks.Task;

namespace TaskMeUp.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MainDbContext context;

        public UserRepository(MainDbContext context)
        {
            this.context = context;
        }

        public async Task<User?> GetUserByIdAsync(Guid userId)
        {
            return await context.Users.Include(u => u.Groups).FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await context.Users.Include(u => u.Groups).FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = await GetUserByIdAsync(userId);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }
    }
}
