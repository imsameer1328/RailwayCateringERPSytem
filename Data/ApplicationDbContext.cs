using Microsoft.EntityFrameworkCore;
using RailwayCateringERPSystem.Models;

namespace RailwayCateringERPSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // ── All Tables ──
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<Journey> Journeys { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<MenuItemIngredient> MenuItemIngredients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<InventoryLog> InventoryLogs { get; set; }
        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── MODULE 1: User & Role ──

            // User → Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── MODULE 2: Train & Journey ──

            // Journey → Train
            modelBuilder.Entity<Journey>()
                .HasOne(j => j.Train)
                .WithMany(t => t.Journeys)
                .HasForeignKey(j => j.TrainId)
                .OnDelete(DeleteBehavior.Restrict);

            // Journey → Manager (User)
            modelBuilder.Entity<Journey>()
                .HasOne(j => j.Manager)
                .WithMany(u => u.Journeys)
                .HasForeignKey(j => j.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── MODULE 3: Menu Management ──

            // MenuItemIngredient → MenuItem
            modelBuilder.Entity<MenuItemIngredient>()
                .HasOne(mi => mi.MenuItem)
                .WithMany(m => m.MenuItemIngredients)
                .HasForeignKey(mi => mi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // MenuItemIngredient → Ingredient
            modelBuilder.Entity<MenuItemIngredient>()
                .HasOne(mi => mi.Ingredient)
                .WithMany(i => i.MenuItemIngredients)
                .HasForeignKey(mi => mi.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── MODULE 4: Order & Service ──

            // Order → Journey
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Journey)
                .WithMany(j => j.Orders)
                .HasForeignKey(o => o.JourneyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order → Waiter (User)
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItem → Order
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict);

            // OrderItem → MenuItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // ── MODULE 5: Inventory ──

            // InventoryLog → Ingredient
            modelBuilder.Entity<InventoryLog>()
                .HasOne(i => i.Ingredient)
                .WithMany(i => i.InventoryLogs)
                .HasForeignKey(i => i.IngredientId)
                .OnDelete(DeleteBehavior.Restrict);

            // InventoryLog → Journey
            modelBuilder.Entity<InventoryLog>()
                .HasOne(i => i.Journey)
                .WithMany(j => j.InventoryLogs)
                .HasForeignKey(i => i.JourneyId)
                .OnDelete(DeleteBehavior.Restrict);

            // InventoryLog → LoggedBy (User)
            modelBuilder.Entity<InventoryLog>()
                .HasOne(i => i.LoggedBy)
                .WithMany(u => u.InventoryLogs)
                .HasForeignKey(i => i.LoggedById)
                .OnDelete(DeleteBehavior.Restrict);

            // ── MODULE 6: Reports ──

            // Report → Journey (one-to-one)
            modelBuilder.Entity<Journey>()
                .HasOne(j => j.Report)
                .WithOne(r => r.Journey)
                .HasForeignKey<Report>(r => r.JourneyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Report → GeneratedBy (User)
            modelBuilder.Entity<Report>()
                .HasOne(r => r.GeneratedBy)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.GeneratedById)
                .OnDelete(DeleteBehavior.Restrict);

            // ── Decimal Precision ──

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                .Property(oi => oi.UnitPrice)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.CurrentStock)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Ingredient>()
                .Property(i => i.MinimumThreshold)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<MenuItemIngredient>()
                .Property(mi => mi.QuantityNeeded)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<InventoryLog>()
                .Property(i => i.QuantityChange)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<InventoryLog>()
                .Property(i => i.StockAfter)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Report>()
                .Property(r => r.TotalRevenue)
                .HasColumnType("decimal(18,2)");
        }
    }
}