using Microsoft.EntityFrameworkCore;
using TaskMeUp.Api.DAL;
using TaskMeUp.Api.Entities;
using TaskMeUp.Api.Interfaces.Repositories;

namespace TaskMeUp.Api.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly MainDbContext context;

        public GroupRepository(MainDbContext context)
        {
            this.context = context;
        }
        public async Task<Group?> GetGroupByIdAsync(Guid id)
        {
            return await context.Groups.Include(u => u.Users).Include(u=>u.Tasks).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Group> CreateGroupAsync(Group group)
        {
            context.Groups.Add(group);
            await context.SaveChangesAsync();
            return group;
        }

        public async Task<Group> UpdateGroupAsync(Group group)
        {
            context.Groups.Update(group);
            await context.SaveChangesAsync();
            return group;
        }

        public async System.Threading.Tasks.Task DeleteGroupAsync(Guid id)
        {
            var group = await GetGroupByIdAsync(id);
            if (group != null)
            {
                context.Groups.Remove(group);
                await context.SaveChangesAsync();
            }
        }
    }
}
