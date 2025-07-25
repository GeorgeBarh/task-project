using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TaskMeUp.Api.Contracts.DTO;
using TaskMeUp.Api.Interfaces.Services;

namespace TaskMeUp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody][Required] UserDto userDto)
        {
            var result = await authService.Register(userDto);
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
        public async Task<IActionResult> Login([FromBody][Required] PartialUserDto userDto)
        {
            var result = await authService.Login(userDto);
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
        [Authorize]
        public async Task<IActionResult> GetMe()
        {
            var result = await authService.GetUserByUsername(User);
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
        [Authorize]
        public async Task<IActionResult> UpdateMe([FromHeader][Required] string oldPassword, [FromBody][Required] UserDto userDto)
        {
            var result = await authService.UpdateUser(User, userDto, oldPassword);
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
        [Authorize]
        public async Task<IActionResult> DeleteMe()
        {
            var result = await authService.DeleteUser(User);
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
