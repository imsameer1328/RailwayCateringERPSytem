using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User → Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // User → Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            // Fix decimal precision — Order
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            // Fix decimal precision — Ingredient
            modelBuilder.Entity<Ingredient>()
                .Property(i => i.CurrentStock)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.MinimumThreshold)
                .HasColumnType("decimal(18,2)");

            // Fix decimal precision — MenuItem
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");
        }
    }
}