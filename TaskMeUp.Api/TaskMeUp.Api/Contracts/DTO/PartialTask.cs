namespace TaskMeUp.Api.Contracts.DTO
{
    public class PartialTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public TaskStatus Status { get; set; }
        public PartialUserInfoDto? AssignedUser { get; set; }
    }

    public enum TaskStatus
    {
        InThoughts,
        ToDo,
        InProgress,
        Blocked,
        Done
    }
}
