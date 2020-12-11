using CoinbitBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoinbitBackend.Services
{
    public class DBRepository : DbContext
    {
        public DBRepository(DbContextOptions<DBRepository> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .Property(b => b.mobile)
                .IsRequired();

            modelBuilder.Entity<Customer>().Property(e => e.createDate)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>().Property(e => e.createDate)
                .HasDefaultValueSql("NOW()")
                .ValueGeneratedOnAdd();
        }

        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }

    }
}
