using Application.Interfaces.Services;
using Application.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;

namespace Rindo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(IUserService service, IHttpContextAccessor httpContextAccessor)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _service.GetUserById(id);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok(result.Value);
        }
        
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUsersByProjectId(Guid projectId)
        {
            var result = await _service.GetUsersByProjectId(projectId);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok(result.Value);
        }
        
        [HttpPost("signup")]
        public async Task<IActionResult> SignUpUser([FromBody]UserDtoSignUp userDtoSignUp)
        {
            var result = await _service.SignUpUser(userDtoSignUp);
            if (!result.IsSuccess) return BadRequest(result.Error);
            //_httpContextAccessor.HttpContext?.Response.Cookies.Append("test-cookies", result.Value.Item2);
            return Ok(result.IsSuccess);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthUser([FromBody]UserDtoAuth userAuth)
        {
            var result = await _service.AuthUser(userAuth);
            if (!result.IsSuccess) return BadRequest(result.Error);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("test-cookies", result.Value.Item2);
            return Ok(result.Value.Item1);
        }

        [Authorize]
        [HttpPut("{id:guid}/firstName")]
        public async Task<IActionResult> ChangeUserFirstName(Guid id, string firstName)
        {
            var result = await _service.ChangeUserFirstName(id, firstName);
            if (!result.IsSuccess) return NotFound(result.Error.Description); 
            return Ok();
        }
        
        [Authorize]
        [HttpPut("{id:guid}/lastName")]
        public async Task<IActionResult> ChangeUserLastName(Guid id, string lastName)
        {
            var result = await _service.ChangeUserLastName(id, lastName);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }
    }
}
