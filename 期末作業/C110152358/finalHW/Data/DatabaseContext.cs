using Microsoft.EntityFrameworkCore;

using finalHW.Models;
namespace finalHW.Data
{
    public class DatabaseContext : DbContext
    {

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<DataContent> Datas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<DataContent>().HasData(
                new DataContent
                {
                    Id = 1,
                    FirstName ="YAO",
                    LastName="YUN",
                    Email="yunyaoteoh@gmail.com",
                    Gender="Male",
                    CompanyName="PENTECH"
                },
                 new DataContent
                 {
                     Id = 2,
                     FirstName = "SHENG",
                     LastName = "YUN",
                     Email = "yunsheng@gmail.com",
                     Gender = "Male",
                     CompanyName = "PENTECH"
                 });
        }

    }
}
