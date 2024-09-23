using AzureIntelliFunc.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureIntelliFunc.Data
{
    public class AzureDBContext : DbContext
    {
        public AzureDBContext(DbContextOptions<AzureDBContext> dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<SalesRequest> SalesRequests { get; set; }
        public DbSet<GroceryItem> groceryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<SalesRequest>(entity => { entity.HasKey(c => c.Id); });
            modelBuilder.Entity<GroceryItem>(entity => { entity.HasKey(c => c.Id); });
        }
    }
}
