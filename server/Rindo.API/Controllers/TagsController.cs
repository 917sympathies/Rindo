using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rindo.Domain.Entities;
using Rindo.Infrastructure.Models;

namespace Rindo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly RindoDbContext _context;
        public TagsController(RindoDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag(string name, Guid projectId)
        {
            var tag = new Tag() { Name = name, ProjectId = projectId };
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return Ok(tag);
        }

        [HttpGet]
        public async Task<IActionResult> GetTagsByProjectId(Guid projectId)
        {
            var tags = await _context.Tags.Where(t => t.ProjectId == projectId).ToListAsync();
            return Ok(tags);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
