namespace TaskMeUp.Api.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<Entities.Task?> GetTaskByIdAsync(Guid id);
        Task<Entities.Task> CreateTaskAsync(Entities.Task task);
        Task<Entities.Task> UpdateTaskAsync(Entities.Task task);
        Task DeleteTaskAsync(Guid id);
    }
}
