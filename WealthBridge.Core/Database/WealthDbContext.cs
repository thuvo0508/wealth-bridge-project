using Microsoft.EntityFrameworkCore;

namespace WealthBridge.Core.Database
{
    public class WealthBridgeDbContext : DbContext
    {
        public WealthBridgeDbContext()
        {
        }

        public WealthBridgeDbContext(DbContextOptions<WealthBridgeDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Borrower> Borrower { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=DefaultConnection");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
