using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Rindo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var result = await _service.GetUserById(id);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok(result.Value);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetUsersByProjectId(Guid projectId)
        {
            var result = await _service.GetUsersByProjectId(projectId);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok(result.Value);
        }
        
        [HttpPut("{id:guid}/firstName")]
        public async Task<IActionResult> ChangeUserFirstName(Guid id, string firstName)
        {
            var result = await _service.ChangeUserFirstName(id, firstName);
            if (!result.IsSuccess) return NotFound(result.Error.Description); 
            return Ok();
        }
        
        [HttpPut("{id:guid}/lastName")]
        public async Task<IActionResult> ChangeUserLastName(Guid id, string lastName)
        {
            var result = await _service.ChangeUserLastName(id, lastName);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }
    }
}
