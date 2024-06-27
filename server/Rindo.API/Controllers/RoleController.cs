using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Services.RoleService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rindo.Domain.DTO;

namespace Rindo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;
        
        public RoleController(IRoleService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDtoOnCreate roleDto)
        {
            var result = await _service.CreateRole(roleDto);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRolesByProjectId(Guid projectId)
        {
            var result = await _service.GetRolesByProjectId(projectId);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody]RolesRights rights)
        {
            await _service.UpdateRoleRights(id, rights);
            return Ok();
        }

        [HttpPut("{id:guid}/name")]
        public async Task<IActionResult> UpdateRoleName(Guid id, string name)
        {
            await _service.UpdateRoleName(id, name);
            return Ok();
        }

        [HttpGet("{projectId:guid}/{userId:guid}")]
        public async Task<IActionResult> GetRightsForUserByProject(Guid projectId, Guid userId)
        {
            var result = await _service.GetRightsByProjectId(projectId, userId);
            return Ok(result);
        }

        [HttpPut("{id:guid}/adduser")]
        public async Task<IActionResult> AddUserToRole(Guid id, Guid userId)
        {
            var result = await _service.AddUserToRole(id, userId);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }

        [HttpPut("{id:guid}/removeuser")]
        public async Task<IActionResult> RemoveUserFromRole(Guid id, Guid userId)
        {
            var result = await _service.RemoveUserFromRole(id, userId);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }
        
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var result = await _service.DeleteRole(id);
            if (!result.IsSuccess) return NotFound(result.Error.Description);
            return Ok();
        }
    }
}
