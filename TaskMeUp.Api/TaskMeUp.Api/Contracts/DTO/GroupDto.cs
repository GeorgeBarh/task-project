namespace TaskMeUp.Api.Contracts.DTO
{
    public class GroupDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public IEnumerable<string> Tags { get; set; } = new List<string>();
    }
}
