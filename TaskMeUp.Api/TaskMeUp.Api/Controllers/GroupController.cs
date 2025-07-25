using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskMeUp.Api.Contracts.DTO;
using TaskMeUp.Api.Interfaces.Services;
using TaskMeUp.Api.Services;

namespace TaskMeUp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService service;

        public GroupController(IGroupService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup([FromBody][Required] GroupDto dto)
        {
            var result = await service.CreateGroup(User, dto);
            if (result.Success || result.ErrorCode != "InternalServerError")
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateGroup([FromBody][Required] GroupDto dto, [FromHeader][Required]Guid groupId)
        {
            var result = await service.UpdateGroupAsync(User, groupId, dto);
            if (result.Success || result.ErrorCode != "InternalServerError")
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteGroup([FromHeader][Required] Guid groupId)
        {
            var result = await service.DeleteGroupAsync(User, groupId);
            if (result.Success || result.ErrorCode != "InternalServerError")
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetGroup([FromHeader][Required] Guid groupId)
        {
            var result = await service.GetGroupByIdAsync(User, groupId);
            if (result.Success || result.ErrorCode != "InternalServerError")
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> AddUserToGroup([FromHeader][Required] Guid groupId,[FromHeader][Required] string username)
        {
            var result = await service.AddUserToGroup(User, groupId, username);
            if (result.Success || result.ErrorCode != "InternalServerError")
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> RemoveUserFromGroup([FromHeader][Required] Guid groupId, [FromHeader][Required] string username)
        {
            var result = await service.RemoveUserFromGroup(User, groupId, username);
            if (result.Success || result.ErrorCode != "InternalServerError")
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody][Required] PartialTask dto, [FromHeader][Required]Guid groupId)
        {
            var result = await service.CreateTask(User, groupId, dto);
            if (result.Success || result.ErrorCode != "InternalServerError")
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateTask([FromBody][Required] PartialTask dto, [FromHeader][Required] Guid groupId, [FromHeader][Required] Guid taskId)
        {
            var result = await service.UpdateTask(User, groupId, taskId, dto);
            if (result.Success || result.ErrorCode != "InternalServerError")
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveTask([FromHeader][Required] Guid groupId, [FromHeader][Required] Guid taskId)
        {
            var result = await service.RemoveTask(User, groupId, taskId);
            if (result.Success || result.ErrorCode != "InternalServerError")
            {
                return StatusCode(200, result);
            }
            else
            {
                return StatusCode(500, result);
            }
        }
    }
}
