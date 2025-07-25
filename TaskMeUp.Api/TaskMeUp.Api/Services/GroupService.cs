using System.Security.Claims;
using TaskMeUp.Api.Contracts.DTO;
using TaskMeUp.Api.Interfaces.Repositories;
using TaskMeUp.Api.Interfaces.Services;
using TaskStatus = TaskMeUp.Api.Contracts.DTO.TaskStatus;

namespace TaskMeUp.Api.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUserRepository userRepo;
        private readonly IGroupRepository groupRepository;
        private readonly ITaskRepository taskRepository;

        public GroupService(IUserRepository userRepo, IGroupRepository groupRepository, ITaskRepository taskRepository)
        {
            this.userRepo = userRepo;
            this.groupRepository = groupRepository;
            this.taskRepository = taskRepository;
        }

        public async Task<ApiResult<PartialGroup>> CreateGroup(ClaimsPrincipal userPrincipal, GroupDto dto)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await userRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (string.IsNullOrEmpty(dto.Name))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Goup name is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var group = new Entities.Group
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    Icon = dto.Icon,
                    Tags = dto.Tags,
                    Id = Guid.NewGuid(),
                    Users = new List<Entities.User> { user }
                };
                var createdGroup = await groupRepository.CreateGroupAsync(group);
                return new ApiResult<PartialGroup>
                {
                    Success = true,
                    Data = new PartialGroup
                    {
                        Id = createdGroup.Id,
                        Name = createdGroup.Name,
                        Description = createdGroup.Description,
                        Icon = createdGroup.Icon,
                        Tags = createdGroup.Tags
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<PartialGroup>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }

        public async Task<ApiResult<PartialGroup>> UpdateGroupAsync(ClaimsPrincipal userPrincipal, Guid groupId, GroupDto dto)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await userRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (!user.Groups.Any(g => g.Id == groupId))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User is not a member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var group = await groupRepository.GetGroupByIdAsync(groupId);
                if (group == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Group not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (string.IsNullOrEmpty(dto.Name))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Group name is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                group.Tags = dto.Tags;
                group.Name = dto.Name;
                group.Description = dto.Description;
                group.Icon = dto.Icon;
                var updatedGroup = await groupRepository.UpdateGroupAsync(group);
                return new ApiResult<PartialGroup>
                {
                    Success = true,
                    Data = new PartialGroup
                    {
                        Id = updatedGroup.Id,
                        Name = updatedGroup.Name,
                        Description = updatedGroup.Description,
                        Icon = updatedGroup.Icon,
                        Tags = updatedGroup.Tags
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<PartialGroup>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }

        public async Task<ApiResult<PartialGroup>> DeleteGroupAsync(ClaimsPrincipal userPrincipal, Guid groupId)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await userRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (!user.Groups.Any(g => g.Id == groupId))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User is not a member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                await groupRepository.DeleteGroupAsync(groupId);
                return new ApiResult<PartialGroup>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<PartialGroup>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }

        public async Task<ApiResult<GroupInfoDto>> GetGroupByIdAsync(ClaimsPrincipal userPrincipal, Guid groupId)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await userRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (!user.Groups.Any(g => g.Id == groupId))
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "User is not a member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var group = await groupRepository.GetGroupByIdAsync(groupId);
                if (group == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Group not found.",
                        ErrorCode = "NotFound"
                    };
                }
                return new ApiResult<GroupInfoDto>
                {
                    Success = true,
                    Data = new GroupInfoDto
                    {
                        Id = group.Id,
                        Name = group.Name,
                        Description = group.Description,
                        Icon = group.Icon,
                        Tags = group.Tags,
                        Tasks = group.Tasks.Select(t => new PartialTask
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            DueDate = t.DueDate,
                            Status = (TaskStatus)t.Status,
                            AssignedUser = t.AssignedUser is null ? null : new PartialUserInfoDto
                            {
                                Portrait = t.AssignedUser.Portrait,
                                Username = t.AssignedUser.Username
                            }
                        }),
                        Users = group.Users.Select(u => new PartialUserInfoDto
                        {
                            Username = u.Username,
                            Portrait = u.Portrait,
                        })
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<GroupInfoDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }

        public async Task<ApiResult<PartialGroup>> AddUserToGroup(ClaimsPrincipal userPrincipal, Guid groupId, string usernameToAdd)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await userRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (!user.Groups.Any(g => g.Id == groupId))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User is not a member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var group = await groupRepository.GetGroupByIdAsync(groupId);
                if (group == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Group not found.",
                        ErrorCode = "NotFound"
                    };
                }
                var userToAdd = await userRepo.GetUserByUsernameAsync(usernameToAdd);
                if (userToAdd == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (group.Users.Any(u => u.Id == userToAdd.Id))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User is already a member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var groupUsersList = group.Users.ToList();
                groupUsersList.Add(userToAdd);
                group.Users = groupUsersList;
                var updatedGroup = await groupRepository.UpdateGroupAsync(group);
                return new ApiResult<PartialGroup>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<PartialGroup>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }

        public async Task<ApiResult<PartialGroup>> RemoveUserFromGroup(ClaimsPrincipal userPrincipal, Guid groupId, string usernameToAdd)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await userRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (!user.Groups.Any(g => g.Id == groupId))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User is not a member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var group = await groupRepository.GetGroupByIdAsync(groupId);
                if (group == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "Group not found.",
                        ErrorCode = "NotFound"
                    };
                }
                var userToRemove = await userRepo.GetUserByUsernameAsync(usernameToAdd);
                if (userToRemove == null)
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (!group.Users.Any(u => u.Id == userToRemove.Id))
                {
                    return new ApiResult<PartialGroup>
                    {
                        Success = false,
                        Message = "User is already a not member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var usersTask = group.Tasks.Where(t => t.AssignedUser != null && t.AssignedUser.Id == userToRemove.Id).ToList();
                foreach (var task in usersTask)
                {
                    task.AssignedUser = null;
                    await taskRepository.UpdateTaskAsync(task);
                }
                var groupUsersList = group.Users.ToList();
                groupUsersList.Remove(userToRemove);
                group.Users = groupUsersList;
                var updatedGroup = await groupRepository.UpdateGroupAsync(group);
                return new ApiResult<PartialGroup>
                {
                    Success = true
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<PartialGroup>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }

        public async Task<ApiResult<GroupInfoDto>> UpdateTask(ClaimsPrincipal userPrincipal, Guid groupId, Guid taskId, PartialTask updatedTask)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await userRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (!user.Groups.Any(g => g.Id == groupId))
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "User is not a member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var group = await groupRepository.GetGroupByIdAsync(groupId);
                if (group == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Group not found.",
                        ErrorCode = "NotFound"
                    };
                }
                var task = group.Tasks.FirstOrDefault(t => t.Id == taskId);
                if (task == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Task not found.",
                        ErrorCode = "NotFound"
                    };
                }
                task.Title = updatedTask.Title;
                task.Description = updatedTask.Description;
                task.DueDate = updatedTask.DueDate;
                task.Status = (Entities.TaskStatus)updatedTask.Status;
                task.AssignedUser = updatedTask.AssignedUser != null
                    ? await userRepo.GetUserByUsernameAsync(updatedTask.AssignedUser.Username)
                    : null;
                await taskRepository.UpdateTaskAsync(task);
                group = await groupRepository.GetGroupByIdAsync(groupId);
                return new ApiResult<GroupInfoDto>
                {
                    Success = true,
                    Data = new GroupInfoDto
                    {
                        Id = group.Id,
                        Name = group.Name,
                        Description = group.Description,
                        Icon = group.Icon,
                        Tags = group.Tags,
                        Tasks = group.Tasks.Select(t => new PartialTask
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            DueDate = t.DueDate,
                            Status = (TaskStatus)t.Status,
                            AssignedUser = t.AssignedUser is null ? null : new PartialUserInfoDto
                            {
                                Portrait = t.AssignedUser.Portrait,
                                Username = t.AssignedUser.Username
                            }
                        }),
                        Users = group.Users.Select(u => new PartialUserInfoDto
                        {
                            Username = u.Username,
                            Portrait = u.Portrait,
                        })
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<GroupInfoDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }
        public async Task<ApiResult<GroupInfoDto>> CreateTask(ClaimsPrincipal userPrincipal, Guid groupId, PartialTask newTask)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await userRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (!user.Groups.Any(g => g.Id == groupId))
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "User is not a member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var group = await groupRepository.GetGroupByIdAsync(groupId);
                if (group == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Group not found.",
                        ErrorCode = "NotFound"
                    };
                }
                var newTaskInDb = new Entities.Task
                {
                    Id = Guid.NewGuid(),
                    Title = newTask.Title,
                    Description = newTask.Description,
                    DueDate = newTask.DueDate,
                    Status = (Entities.TaskStatus)newTask.Status,
                    AssignedUser = newTask.AssignedUser != null
                        ? await userRepo.GetUserByUsernameAsync(newTask.AssignedUser.Username)
                        : null,
                    CreatedAt = DateTime.UtcNow,
                    Group = group
                };
                await taskRepository.CreateTaskAsync(newTaskInDb);
                group = await groupRepository.GetGroupByIdAsync(groupId);
                return new ApiResult<GroupInfoDto>
                {
                    Success = true,
                    Data = new GroupInfoDto
                    {
                        Id = group.Id,
                        Name = group.Name,
                        Description = group.Description,
                        Icon = group.Icon,
                        Tags = group.Tags,
                        Tasks = group.Tasks.Select(t => new PartialTask
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            DueDate = t.DueDate,
                            Status = (TaskStatus)t.Status,
                            AssignedUser = t.AssignedUser is null ? null : new PartialUserInfoDto
                            {
                                Portrait = t.AssignedUser.Portrait,
                                Username = t.AssignedUser.Username
                            }
                        }),
                        Users = group.Users.Select(u => new PartialUserInfoDto
                        {
                            Username = u.Username,
                            Portrait = u.Portrait,
                        })
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<GroupInfoDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }
        public async Task<ApiResult<GroupInfoDto>> RemoveTask(ClaimsPrincipal userPrincipal, Guid groupId, Guid taskId)
        {
            try
            {
                var username = userPrincipal.FindFirst(ClaimTypes.Name)?.Value;
                if (string.IsNullOrEmpty(username))
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Username is required.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var user = await userRepo.GetUserByUsernameAsync(username);
                if (user == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "User not found.",
                        ErrorCode = "NotFound"
                    };
                }
                if (!user.Groups.Any(g => g.Id == groupId))
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "User is not a member of this group.",
                        ErrorCode = "InvalidInput"
                    };
                }
                var group = await groupRepository.GetGroupByIdAsync(groupId);
                if (group == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Group not found.",
                        ErrorCode = "NotFound"
                    };
                }
                var taskToDelete = group.Tasks.FirstOrDefault(t => t.Id == taskId);
                if (taskToDelete == null)
                {
                    return new ApiResult<GroupInfoDto>
                    {
                        Success = false,
                        Message = "Task not found.",
                        ErrorCode = "NotFound"
                    };
                }
                await taskRepository.DeleteTaskAsync(taskToDelete.Id);
                group = await groupRepository.GetGroupByIdAsync(groupId);
                return new ApiResult<GroupInfoDto>
                {
                    Success = true,
                    Data = new GroupInfoDto
                    {
                        Id = group.Id,
                        Name = group.Name,
                        Description = group.Description,
                        Icon = group.Icon,
                        Tags = group.Tags,
                        Tasks = group.Tasks.Select(t => new PartialTask
                        {
                            Id = t.Id,
                            Title = t.Title,
                            Description = t.Description,
                            DueDate = t.DueDate,
                            Status = (TaskStatus)t.Status,
                            AssignedUser = t.AssignedUser is null ? null : new PartialUserInfoDto
                            {
                                Portrait = t.AssignedUser.Portrait,
                                Username = t.AssignedUser.Username
                            }
                        }),
                        Users = group.Users.Select(u => new PartialUserInfoDto
                        {
                            Username = u.Username,
                            Portrait = u.Portrait,
                        })
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new ApiResult<GroupInfoDto>
                {
                    Success = false,
                    Message = "An error occurred",
                    ErrorCode = "InternalServerError"
                };
            }
        }
    }
}
