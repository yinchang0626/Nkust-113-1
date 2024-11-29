using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nkust.Web.Entities;

namespace Nkust.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Camera> Cameras { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
