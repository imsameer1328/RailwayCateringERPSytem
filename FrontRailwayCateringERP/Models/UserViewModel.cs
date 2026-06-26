using Microsoft.AspNetCore.Mvc.Rendering;

namespace FrontRailwayCateringERP.Models
{
    public class UserViewModel
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }      
        public string Username { get; set; }      
        public string Phone { get; set; }
        public string PasswordHash { get; set; }
        public Guid RoleId { get; set; }      // FK to Role
        public RoleViewModel Role { get; set; }   // ← nested Role object
        public string Status { get; set; }
        // ── Just a simple list of roles ──
        public List<RoleViewModel> RoleList { get; set; } = new();
    }
}