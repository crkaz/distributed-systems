using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkLab
{
    public class MyContext : DbContext
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Person> People { get; set; }

        public MyContext() : base() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies(); // Enable lazy loading.
            optionsBuilder.UseSqlServer("Server = (localdb)\\mssqllocaldb; Database = EFLab7;");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
