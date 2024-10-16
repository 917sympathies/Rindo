using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;
using Rindo.Domain.Services;

namespace Rindo.API.Controllers
{
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
        public async Task<IActionResult> SignUpUser([FromBody]UserDtoSignUp userDtoSignUp)
        {
            var result = await _service.SignUpUser(userDtoSignUp);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.IsSuccess);
        }

        [HttpPost("auth")]
        public async Task<IActionResult> AuthUser([FromBody]UserDtoAuth userAuth)
        {
            var result = await _service.AuthUser(userAuth);
            if (!result.IsSuccess) return BadRequest(result.Error);
            _httpContextAccessor.HttpContext?.Response.Cookies.Append("_rindo", result.Value.Item2);
            return Ok(result.Value.Item1);
        }
    }
}
