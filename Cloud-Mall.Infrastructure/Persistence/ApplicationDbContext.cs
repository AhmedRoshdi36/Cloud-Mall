using Cloud_Mall.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cloud_Mall.Infrastructure.Persistence
{
    internal class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<StoreCategory> StoreCategories { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<GoverningLocation> GoverningLocations { get; set; }
        public DbSet<StoreAddress> StoreAddresses { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                // ... your decimal precision configurations ...
                entity.Property(p => p.Price).HasPrecision(18, 2);
                entity.Property(p => p.Discount).HasPrecision(18, 2);

                // Address the cascade delete issue for the relationship with Store
                entity.HasOne(p => p.Store)                  // Product has one Store
                      .WithMany(s => s.Products)             // Store has many Products
                      .HasForeignKey(p => p.StoreID)         // Foreign key is StoreID
                      .OnDelete(DeleteBehavior.Restrict); // Or DeleteBehavior.NoAction
                                                          // This tells SQL Server not to cascade delete
                                                          // Products if a Store is deleted.
                                                          // You'll have to manually delete products
                                                          // or handle it in your application logic.
                modelBuilder.Entity<Store>(entity =>
                {
                    entity.HasOne(s => s.Vendor) // Assuming Store has 'public virtual ApplicationUser Vendor { get; set; }'
                          .WithMany(u => u.Stores) // From ApplicationUser.Stores
                          .HasForeignKey(s => s.VendorID) // Assuming Store has 'public string VendorID { get; set; }'
                          .OnDelete(DeleteBehavior.Restrict); // <<< KEY CHANGE TO BREAK THE CYCLE
                });

            });
        }
    }
}
