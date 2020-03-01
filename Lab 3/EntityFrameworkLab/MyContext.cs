using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkLab
{
    public class MyContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<Address> Addresses { get; set; }

        
        public MyContext() : base() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = EFLab;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
