using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskMeUp.Api.Contracts.DTO;
using TaskMeUp.Api.Entities;
using TaskMeUp.Api.Interfaces.Repositories;
using TaskMeUp.Api.Interfaces.Services;

namespace TaskMeUp.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration config;
        private readonly IUserRepository repo;

        public AuthService(IConfiguration config, IUserRepository repo)
        {
            this.config = config;
            this.repo = repo;
        }
        public async Task<ApiResult<UserInfoDto>> Register(UserDto userDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password))
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "Username and password are required.",
                        ErrorCode = "InvalidInput"
                    };
                }

                var existingUser = await repo.GetUserByUsernameAsync(userDto.Username);
                if (existingUser != null)
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "Username must be unique.",
                        ErrorCode = "Conflict"
                    };
                }
                // Hash the password
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                // Create a new user entity
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = userDto.Username,
                    PasswordHash = passwordHash,
                    Portrait = userDto.Portrait
                };
                var token = CreateToken(user);

                var newUserInDb = await repo.CreateUserAsync(user);

                return new ApiResult<UserInfoDto>
                {
                    Success = true,
                    Data = new UserInfoDto
                    {
                        Username = newUserInDb.Username,
                        Token = token,
                        Portrait = newUserInDb.Portrait
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<UserInfoDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }
        public async Task<ApiResult<UserInfoDto>> Login(PartialUserDto userDto)
        {
            try
            {
                if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password))
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "Username and password are required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await repo.GetUserByUsernameAsync(userDto.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(userDto.Password, user.PasswordHash))
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "Invalid username or password.",
                        ErrorCode = "Unauthorized"
                    };
                }
                var token = CreateToken(user);
                return new ApiResult<UserInfoDto>
                {
                    Success = true,
                    Data = new UserInfoDto
                    {
                        Username = user.Username,
                        Token = token,
                        Portrait = user.Portrait,
                        Groups = user.Groups.Select(s => new PartialGroup
                        {
                            Description = s.Description,
                            Icon = s.Icon,
                            Id = s.Id,
                            Name = s.Name,
                            Tags = s.Tags
                        })
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<UserInfoDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }
        public async Task<ApiResult<UserInfoDto>> GetUserByUsername(ClaimsPrincipal userPrincipal)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await repo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                var token = CreateToken(user);
                return new ApiResult<UserInfoDto>
                {
                    Success = true,
                    Data = new UserInfoDto
                    {
                        Username = user.Username,
                        Token = token,
                        Portrait = user.Portrait,
                        Groups = user.Groups.Select(s => new PartialGroup
                        {
                            Description = s.Description,
                            Icon = s.Icon,
                            Id = s.Id,
                            Name = s.Name,
                            Tags = s.Tags
                        })
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<UserInfoDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }
        public async Task<ApiResult<UserInfoDto>> UpdateUser(ClaimsPrincipal userPrincipal, UserDto userDto, string oldPassword)
        {
            try
            {
                var oldUsername = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(oldUsername) || string.IsNullOrEmpty(oldPassword))
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "Old Username and Password is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                if (string.IsNullOrEmpty(userDto.Username) || string.IsNullOrEmpty(userDto.Password))
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "Username and password are required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await repo.GetUserByUsernameAsync(oldUsername);
                if (user == null || !BCrypt.Net.BCrypt.Verify(oldPassword,user.PasswordHash))
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                // Hash the password
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
                // Update the user entity
                user.Username = userDto.Username;
                user.PasswordHash = passwordHash;
                user.Portrait = userDto.Portrait;
                var updatedUserInDb = await repo.UpdateUserAsync(user);
                var token = CreateToken(updatedUserInDb);
                return new ApiResult<UserInfoDto>
                {
                    Success = true,
                    Data = new UserInfoDto
                    {
                        Username = updatedUserInDb.Username,
                        Token = token,
                        Portrait = updatedUserInDb.Portrait,
                        Groups = updatedUserInDb.Groups.Select(s=>new PartialGroup
                        {
                            Description = s.Description,
                            Icon = s.Icon,
                            Id = s.Id,
                            Name = s.Name,
                            Tags = s.Tags
                        })
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<UserInfoDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }
        public async Task<ApiResult<UserInfoDto>> DeleteUser(ClaimsPrincipal userPrincipal)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await repo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<UserInfoDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                await repo.DeleteUserAsync(user.Id);
                return new ApiResult<UserInfoDto>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<UserInfoDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue<string>("AppSettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                config.GetValue<string>("AppSettings:Issuer"),
                config.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.WriteToken(tokenDescriptor);

            return token;
        }
    }
}
