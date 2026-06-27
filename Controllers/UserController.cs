using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Data;
using RailwayCateringERPSystem.Helpers;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return NotFound("User not found");
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            user.UserId = Guid.NewGuid();
            user.PasswordHash = PasswordHelper.HashPassword(user.PasswordHash);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            user.FullName = updatedUser.FullName;
            user.Username = updatedUser.Username;
            user.Phone = updatedUser.Phone;
            user.Status = updatedUser.Status;
            user.RoleId = updatedUser.RoleId;

            if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
            {
                user.PasswordHash = PasswordHelper.HashPassword(updatedUser.PasswordHash);
            }

            await _context.SaveChangesAsync();
            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound("User not found");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok("User deleted successfully");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Users.AnyAsync())
                return StatusCode(403, new { message = "Registration is closed. A Super Admin already exists." });

            await DbInitializer.SeedRolesAsync(_context);

            var adminRole = await _context.Roles.FirstAsync(r => r.Name == "Super Admin");

            var user = new User
            {
                UserId = Guid.NewGuid(),
                FullName = request.FullName,
                Username = request.Username,
                PasswordHash = PasswordHelper.HashPassword(request.Password),
                Phone = request.Phone ?? "",
                Status = "Active",
                RoleId = adminRole.RoleId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new LoginResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                RoleName = "Super Admin",
                RoleId = user.RoleId
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid username or password" });

            return Ok(new LoginResponse
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Username = user.Username,
                RoleName = user.Role?.Name ?? "",
                RoleId = user.RoleId
            });
        }

    }
}
