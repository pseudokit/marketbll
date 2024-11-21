using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class TradeMarketDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; } = default!;

        public DbSet<Customer> Customers { get; set; } = default!;

        public DbSet<Receipt> Receipts { get; set; } = default!;

        public DbSet<ReceiptDetail> ReceiptsDetails { get; set; } = default!;

        public DbSet<Product> Products { get; set; } = default!;

        public DbSet<ProductCategory> ProductCategories { get; set; } = default!;

        public TradeMarketDbContext(DbContextOptions<TradeMarketDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                        .HasOne(c => c.Person)
                        .WithOne(p => p.Customer)
                        .HasForeignKey<Customer>(c => c.PersonId);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Receipts)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);

            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(rd => rd.Receipt)
                .WithMany(r => r.ReceiptDetails)
                .HasForeignKey(rd => rd.ReceiptId);

            modelBuilder.Entity<ReceiptDetail>()
                .HasOne(rd => rd.Product)
                .WithMany(p => p.ReceiptDetails)
                .HasForeignKey(rd => rd.ProductId);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.Products)
                .HasForeignKey(p => p.ProductCategoryId);
        }

    }
}
