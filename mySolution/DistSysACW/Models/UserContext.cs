using Microsoft.EntityFrameworkCore;
using System;

namespace DistSysACW.Models
{
    public class UserContext : DbContext
    {
        public UserContext() : base() { }

        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<ArchivedLog> ArchivedLogs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading.
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DistSysACW;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}