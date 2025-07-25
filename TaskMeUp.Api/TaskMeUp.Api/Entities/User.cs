namespace TaskMeUp.Api.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Portrait { get; set; } = string.Empty;
        public IEnumerable<Group> Groups { get; set; } = new List<Group>();
    }
}
