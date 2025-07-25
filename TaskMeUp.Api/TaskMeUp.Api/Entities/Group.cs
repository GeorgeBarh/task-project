namespace TaskMeUp.Api.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public IEnumerable<User> Users { get; set; } = new List<User>();
        public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    }
}
