using Microsoft.EntityFrameworkCore;
using WorkLogerCA.Models;

namespace WorkLogerCA.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
            Database.EnsureCreated();

        }

        public DbSet<Transport> Transport { get; set; } = null!;
        public DbSet<Equipment> Equipment { get; set; } = null!;
        public DbSet<Work> Work { get; set; } = null!;
        public DbSet<Request> Request { get; set; } = null!;

    }
}
