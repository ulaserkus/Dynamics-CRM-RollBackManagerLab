using Identity.Framework.Core.DTOs;
using Identity.Framework.Core.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Shared.CustomController;

namespace Identity.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : CustomBaseController
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAsync([FromBody] AddUserRequestDto addUserRequestDto)
        {
            var result = await _userService.AddUserAsync(addUserRequestDto);
            return CreateActionResultInstance(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUserRequestDto loginUserRequestDto)
        {
            var result = await _userService.LoginAsync(loginUserRequestDto);
            return CreateActionResultInstance(result);
        }

        [HttpPost("LoginWithKey")]
        public async Task<IActionResult> LoginWithKeyAsync()
        {
            string key = HttpContext.Request.Headers["ApiKey"].ToString();
            var result = await _userService.LoginWithApiKeyAsync(key);
            return CreateActionResultInstance(result);
        }

        [HttpPost("UserKeys/{userId}")]
        public async Task<IActionResult> QueryApiKeysAsync([FromRoute] string userId)
        {
            var result = await _userService.GetUserApiKeysAsync(userId);
            return CreateActionResultInstance(result);
        }
    }
}
