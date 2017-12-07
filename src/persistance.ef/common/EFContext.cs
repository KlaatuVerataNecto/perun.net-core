using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using infrastructure.user.entities;
using infrastructure.email.entities;
using infrastucture.libs.providers;
using domain.model;
using infrastructure.user.models;

namespace persistance.ef.common
{
    public interface IEFContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        DbSet<Post> Posts { get; set; }

        // TODO: Remap those
        DbSet<UserDb> Users { get; set; }
        DbSet<LoginDb> Logins { get; set; }
        DbSet<UserPasswordDb> UserChanges { get; set; }
        DbSet<EmailTemplateDb> EmailTemplates { get; set; }
        DbSet<EmailQueueDb> EmailQueueItems { get; set; }
        void SaveChanges();
    }

    public class EFContext : DbContext , IEFContext
    {
        private IConnectionStringProvider _connectionStringProvider;
        public EFContext(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }
        public DbSet<Post> Posts { get; set; }

        // TODO: Remap those
        public DbSet<UserDb> Users { get; set; }
        public DbSet<LoginDb> Logins { get; set; }
        public DbSet<UserPasswordDb> UserChanges { get; set; }        
        public DbSet<EmailTemplateDb> EmailTemplates { get; set; }
        public DbSet<EmailQueueDb> EmailQueueItems { get; set; }

        void IEFContext.SaveChanges()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDb>()
                .ToTable("users")
                .HasMany(x=>x.Logins)
                .WithOne(x=>x.User)
                .HasForeignKey(x=>x.user_id)
                .IsRequired();

            modelBuilder.Entity<LoginDb>()
                .ToTable("users_login")
                .HasOne(d => d.User)
                .WithMany(d => d.Logins)
                .HasForeignKey(e => e.user_id)
                .IsRequired();

            modelBuilder.Entity<UserPasswordDb>()
                .ToTable("users_password")
                .HasOne(d => d.Login)
                .WithMany(d => d.UserPasswordResets)
                .HasForeignKey(e => e.user_login_id);

            modelBuilder.Entity<UserUsernameDb>()
                .ToTable("users_username")
                .HasOne(d => d.User)
                .WithOne(d => d.UsernameToken)
                .HasForeignKey<UserUsernameDb>(b => b.user_id);

            modelBuilder.Entity<UserEmailDb>()
                .ToTable("users_email")
                .HasOne(d => d.Login)
                .WithMany(d => d.UserEmailChanges)
                .HasForeignKey(e => e.user_login_id);

            modelBuilder.Entity<EmailTemplateDb>()
                .ToTable("email_template");

            modelBuilder.Entity<EmailQueueDb>()
                .ToTable("email_queue");

            modelBuilder.Entity<Post>().ToTable("posts").Property<string>("title").HasField("_title");
            modelBuilder.Entity<Post>().ToTable("posts").Property<int>("id").HasField("_id");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseMySql(_connectionStringProvider.ConnectionString);

    }
}
