using DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Order> Orders => Set<Order>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.UnitPrice)
                      .HasPrecision(18, 4);

            });

            builder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Price)
                      .HasPrecision(18, 4);

                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(p => p.Description)
                      .HasMaxLength(500);

                entity.Property(p => p.Category)
                      .HasMaxLength(50);

                entity.HasMany(o => o.OrderItems)
                      .WithOne(o => o.Product)
                      .HasForeignKey(o => o.ProductId)
                      .IsRequired();
            });

            builder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.OrderDate)
                      .IsRequired();

                entity.HasMany(o => o.OrderItems)
                      .WithOne(o => o.Order)
                      .HasForeignKey(o => o.OrderId)
                      .IsRequired();

                entity.HasOne(o => o.User)
                      .WithMany(o => o.Orders)
                      .HasForeignKey(o => o.UserId)
                      .IsRequired();
            });

        }

    }
}
