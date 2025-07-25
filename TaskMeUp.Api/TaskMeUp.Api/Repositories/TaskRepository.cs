using Microsoft.EntityFrameworkCore;
using TaskMeUp.Api.DAL;
using TaskMeUp.Api.Interfaces.Repositories;

namespace TaskMeUp.Api.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly MainDbContext context;

        public TaskRepository(MainDbContext context)
        {
            this.context = context;
        }

        public async Task<Entities.Task?> GetTaskByIdAsync(Guid id)
        {
            return await context.Tasks.Include(t => t.Group).Include(t => t.AssignedUser).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Entities.Task> CreateTaskAsync(Entities.Task task)
        {
            context.Tasks.Add(task);
            await context.SaveChangesAsync();
            return task;
        }

        public async Task<Entities.Task> UpdateTaskAsync(Entities.Task task)
        {
            context.Tasks.Update(task);              
            await context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            var task = await GetTaskByIdAsync(id);
            if (task != null)
            {
                context.Tasks.Remove(task);
                await context.SaveChangesAsync();
            }
        }
    }
}
