using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nkust.Web.Entities;
using System.Reflection.Metadata;

namespace Nkust.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Camera> Cameras { get; set; } = null!;

        public DbSet<PoliceOffice> PoliceOffices { get; set; } = null!;
        public DbSet<PoliceStation> PoliceStation { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Camera>()
            //    .HasOne<PoliceOffice>();

            //modelBuilder.Entity<Camera>()
            //    .HasOne<PoliceStation>();

            modelBuilder.Entity<PoliceOffice>()
                .HasMany(e => e.PoliceStations)
                .WithOne(e=>e.PoliceOffice)
                .HasForeignKey("PoliceOfficeId");
            //.WithOne(e => e.Blog)
            //.HasForeignKey(e => e.BlogId)
            //.IsRequired();
        }
    }
}
