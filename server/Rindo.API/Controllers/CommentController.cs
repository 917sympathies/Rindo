using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.CommentsService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rindo.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _service;
        
        public CommentController(ICommentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsCountByTaskId(Guid taskId)
        {
            var result = await _service.GetCommentsCountByTaskId(taskId);
            return Ok(result);
        }
    }
}
