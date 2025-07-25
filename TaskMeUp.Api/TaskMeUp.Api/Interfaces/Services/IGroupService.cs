using System.Security.Claims;
using TaskMeUp.Api.Contracts.DTO;

namespace TaskMeUp.Api.Interfaces.Services
{
    public interface IGroupService
    {
        Task<ApiResult<PartialGroup>> CreateGroup(ClaimsPrincipal userPrincipal, GroupDto dto);
        Task<ApiResult<PartialGroup>> UpdateGroupAsync(ClaimsPrincipal userPrincipal, Guid groupId, GroupDto dto);
        Task<ApiResult<PartialGroup>> DeleteGroupAsync(ClaimsPrincipal userPrincipal, Guid groupId);
        Task<ApiResult<GroupInfoDto>> GetGroupByIdAsync(ClaimsPrincipal userPrincipal, Guid groupId);
        Task<ApiResult<PartialGroup>> AddUserToGroup(ClaimsPrincipal userPrincipal, Guid groupId, string usernameToAdd);
        Task<ApiResult<PartialGroup>> RemoveUserFromGroup(ClaimsPrincipal userPrincipal, Guid groupId, string usernameToAdd);
        Task<ApiResult<GroupInfoDto>> UpdateTask(ClaimsPrincipal userPrincipal, Guid groupId, Guid taskId, PartialTask updatedTask);
        Task<ApiResult<GroupInfoDto>> CreateTask(ClaimsPrincipal userPrincipal, Guid groupId, PartialTask newTask);
        Task<ApiResult<GroupInfoDto>> RemoveTask(ClaimsPrincipal userPrincipal, Guid groupId, Guid taskId);
    }
}
