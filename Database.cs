using BankTimeNET.models;
using Microsoft.EntityFrameworkCore;

namespace BankTimeNET
{
    public class DatabaseContext: DbContext
    {
        public DbSet<Service>? Services { get; set; }
        public DbSet<Bank>? Banks { get; set; }
        public DbSet<User>? Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=BankTimeNET;Integrated Security=True");
        }
    }
}
