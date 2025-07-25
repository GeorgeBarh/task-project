using TaskMeUp.Api.Entities;

namespace TaskMeUp.Api.Interfaces.Repositories
{
    public interface IGroupRepository
    {
        Task<Group?> GetGroupByIdAsync(Guid id);
        Task<Group> CreateGroupAsync(Group group);
        Task<Group> UpdateGroupAsync(Group group);
        System.Threading.Tasks.Task DeleteGroupAsync(Guid id);
    }
}
