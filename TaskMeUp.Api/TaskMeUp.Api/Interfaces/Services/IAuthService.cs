using System.Security.Claims;
using TaskMeUp.Api.Contracts.DTO;

namespace TaskMeUp.Api.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ApiResult<UserInfoDto>> Register(UserDto userDto);
        Task<ApiResult<UserInfoDto>> Login(PartialUserDto userDto);
        Task<ApiResult<UserInfoDto>> GetUserByUsername(ClaimsPrincipal user);
        Task<ApiResult<UserInfoDto>> UpdateUser(ClaimsPrincipal userPrincipal, UserDto userDto,string oldPassword);
        Task<ApiResult<UserInfoDto>> DeleteUser(ClaimsPrincipal userPrincipal);
    }
}
