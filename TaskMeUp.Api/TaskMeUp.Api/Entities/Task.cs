namespace TaskMeUp.Api.Entities
{
    public class Task
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public TaskStatus Status { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Group Group { get; set; } = null!;

        public User? AssignedUser { get; set; }
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
