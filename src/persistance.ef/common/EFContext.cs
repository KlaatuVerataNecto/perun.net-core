using Microsoft.EntityFrameworkCore;
using infrastructure.user.entities;

namespace persistance.ef.common
{
    public interface IEFContext {

        DbSet<User> Users { get; set; }
        DbSet<Login> Logins { get; set; }
        void SaveChanges();
    }

    public class EFContext : DbContext, IEFContext
    {
        private IConnectionStringProvider _connectionStringProvider;
        public EFContext(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<UserPassword> UserChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .ToTable("users")
                .HasMany(x=>x.Logins)
                .WithOne(x=>x.User)
                .HasForeignKey(x=>x.user_id);

            modelBuilder.Entity<Login>()
                .ToTable("users_login")
                .HasOne(d => d.User)
                .WithMany(d => d.Logins)
                .HasForeignKey(e => e.user_id);

            modelBuilder.Entity<UserPassword>()
                .ToTable("users_password")
                .HasOne(d => d.Login)
                .WithOne(d => d.UserPasswordReset)
                .HasForeignKey<UserPassword>(e => e.user_login_id);

            modelBuilder.Entity<UserEmail>()
                .ToTable("users_email")
                .HasOne(d => d.Login)
                .WithOne(d => d.UserEmailChange)
                .HasForeignKey<UserPassword>(e => e.user_login_id);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseMySql(_connectionStringProvider.ConnectionString);

        void IEFContext.SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
