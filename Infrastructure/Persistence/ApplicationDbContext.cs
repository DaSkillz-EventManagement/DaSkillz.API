using Domain.Entities;
using Domain.Repositories.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IUnitOfWork
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Logo> Logos { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Participant> Participants { get; set; }
        public virtual DbSet<RoleEvent> RoleEvents { get; set; }    
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<SponsorEvent> SponsorEvents { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
