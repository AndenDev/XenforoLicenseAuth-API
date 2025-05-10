using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration
{
    public class XenforoDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserAuthenticate> UserAuthentications { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<SessionActivity> SessionActivities { get; set; }

        public XenforoDbContext(DbContextOptions<XenforoDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(XenforoDbContext).Assembly);
        }
    }
}
