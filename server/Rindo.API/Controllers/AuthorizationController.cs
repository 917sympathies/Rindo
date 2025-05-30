using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;
using Rindo.Domain.Services;

namespace Rindo.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ControllerBase
{
    private readonly IAuthorizationService _service;
    private readonly IHttpContextAccessor _httpContextAccessor;
        
    public AuthorizationController(IAuthorizationService service, IHttpContextAccessor httpContextAccessor)
    {
        _service = service;
        _httpContextAccessor = httpContextAccessor;
    }
        
    [HttpPost("signup")]
    public async Task<IActionResult> SignUpUser([FromBody]SignUpDto signUpDto)
    {
        var result = await _service.SignUpUser(signUpDto);
        if (!result.IsSuccess) return BadRequest(result.Error);
        return Ok(result.IsSuccess);
    }

    [HttpPost("auth")]
    public async Task<IActionResult> AuthUser([FromBody]LoginDto loginDto)
    {
        var result = await _service.AuthUser(loginDto);
        if (!result.IsSuccess) return BadRequest(result.Error);
        _httpContextAccessor.HttpContext?.Response.Cookies.Append("_rindo", result.Value.Token);
        return Ok(result.Value.User);
    }
}