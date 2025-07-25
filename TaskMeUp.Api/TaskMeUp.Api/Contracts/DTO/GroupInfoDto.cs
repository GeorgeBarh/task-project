using TaskMeUp.Api.Entities;

namespace TaskMeUp.Api.Contracts.DTO
{
    public class GroupInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public IEnumerable<string> Tags { get; set; } = new List<string>();
        public IEnumerable<PartialUserInfoDto> Users { get; set; } = new List<PartialUserInfoDto>();
        public IEnumerable<PartialTask> Tasks { get; set; } = new List<PartialTask>();
    }
}
