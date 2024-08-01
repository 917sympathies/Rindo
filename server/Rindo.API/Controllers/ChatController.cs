using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rindo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _service;
        
        public ChatController(IChatService service)
        {
            _service = service;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetChatById(Guid id)
        {
            var result = await _service.GetChatById(id);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok(result.Value);
        }
    }
}
