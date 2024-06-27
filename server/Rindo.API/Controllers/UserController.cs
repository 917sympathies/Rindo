using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Mapping;
using Application.Services.UserService;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.DTO;
using Rindo.Domain.Entities;
using Rindo.Domain.Repositories;
using Rindo.Infrastructure.Models;

namespace Rindo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _service;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RindoDbContext _context;

        public UserController(IUnitOfWork unitOfWork, IUserService service, IHttpContextAccessor httpContextAccessor, RindoDbContext context)
        {
            _unitOfWork = unitOfWork;
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
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

        [HttpPut("{id:guid}/firstName")]
        public async Task<IActionResult> ChangeUserFirstName(Guid id, string firstName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) return NotFound();
            user.FirstName = firstName;
            await _context.SaveChangesAsync();
            return Ok();
        }
        
        [HttpPut("{id:guid}/lastName")]
        public async Task<IActionResult> ChangeUserLastName(Guid id, string lastName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null) return NotFound();
            user.LastName = lastName;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
