using Aspcorespa.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aspcorespa.Context
{
    public class AppDBContext : IdentityDbContext<UserEntity>
    {
        public DbSet<Message> Messages { get; set; }

        public AppDBContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Message>()
                    .Property(x => x.SubmissionDate)
                    .HasDefaultValueSql("getutcdate()");

            builder.Entity<UserEntity>()
                    .Property(x => x.CreatedAt)
                    .HasDefaultValue(DateTime.Now);

            builder.Entity<UserEntity>()
                    .HasIndex(x => x.Email)
                    .IsUnique();
        }

    }
}
