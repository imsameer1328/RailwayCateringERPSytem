using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET all roles
        [HttpGet("all")]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            return Ok(roles);
        }

        // GET one role by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                return NotFound("Role not found");

            return Ok(role);
        }

        // POST — create a new role
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] Role role)
        {
            role.RoleId = Guid.NewGuid();
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return Ok(role);
        }

        // PUT — update existing role
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, [FromBody] Role updatedRole)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                return NotFound("Role not found");

            role.Name = updatedRole.Name;
            role.Permissions = updatedRole.Permissions;

            await _context.SaveChangesAsync();
            return Ok(role);
        }

        // DELETE — delete a role
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                return NotFound("Role not found");

            // Check if any users have this role
            var hasUsers = await _context.Users.AnyAsync(u => u.RoleId == id);

            if (hasUsers)
                return BadRequest("Cannot delete this role because it is assigned to one or more users. Please reassign those users first.");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return Ok("Role deleted successfully");
        }
    }
}