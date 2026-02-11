using Application.Interfaces.Access;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Rindo.Domain.DTO.Auth;
using IAuthorizationService = Application.Interfaces.Services.IAuthorizationService;

namespace Rindo.API.Controllers;

[Route("api/[controller]")]
[AllowAnonymous]
[ApiController]
public class AuthorizationController(IAuthorizationService service, IDataAccessController dataAccessController) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUpUser([FromBody]SignUpDto signUpDto)
    {
        return Ok(await service.SignUpUser(signUpDto));
    }

    [HttpPost("auth")]
    public async Task<IActionResult> AuthUser([FromBody]LoginDto loginDto)
    {
        return Ok(await service.AuthUser(loginDto));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody]RefreshTokenDto refreshTokenDto)
    {
        return Ok(await service.RefreshToken(refreshTokenDto.RefreshToken, dataAccessController.EmployeeId));
    }
}