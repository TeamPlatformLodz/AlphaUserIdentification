using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using AlphaUserIdentification.Models;

namespace AlphaUserIdentification.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Publication> Publications { get; set; }
        public DbSet<Team> Teams{ get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Member> Member { get; set; }
        public DbSet<PublishedFor> PublishedFor { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.Entity<Publication>()
                .HasKey(c => c.PublicationId);
            builder.Entity<Member>()
                .HasKey(m => new { m.ApplicationUserId, m.TeamId });
            builder.Entity<Administrator>()
                .HasKey(m => new { m.ApplicationUserId, m.TeamId });
            builder.Entity<PublishedFor>()
                .HasKey(m => new { m.PublicationId, m.TeamId });
        }

        public DbSet<AlphaUserIdentification.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}
