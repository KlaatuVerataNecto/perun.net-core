using Microsoft.EntityFrameworkCore;
using persistance.ef.entity;

namespace persistance.ef.common
{
    public interface IEFContext {

        DbSet<User> Users { get; set; }
        DbSet<Login> Logins { get; set; }
    }

    public class EFContext : DbContext, IEFContext
    {
        private string _connectionString;
        public EFContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasMany(x=>x.Logins)
                .WithOne(x=>x.User)
                .HasForeignKey(x=>x.user_id);

            modelBuilder.Entity<Login>()
                .ToTable("users_login")
                .HasOne(d=>d.User)
                .WithMany(d=>d.Logins)
                .HasForeignKey(e => e.user_id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseMySql(_connectionString);
    }
}
