using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(ApplicationDbContext context)
        {
            if (await context.Roles.AnyAsync())
                return;

            var roles = new List<Role>
            {
                new() { RoleId = Guid.NewGuid(), Name = "Super Admin", Permissions = "Full access" },
                new() { RoleId = Guid.NewGuid(), Name = "Manager", Permissions = "Manage journeys, orders, inventory" },
                new() { RoleId = Guid.NewGuid(), Name = "Waiter", Permissions = "Place orders" },
                new() { RoleId = Guid.NewGuid(), Name = "Kitchen Staff", Permissions = "View and update order status" }
            };

            context.Roles.AddRange(roles);
            await context.SaveChangesAsync();
        }
    }
}
