using Microsoft.EntityFrameworkCore;
using myPetraAPI.Model;

namespace myPetraAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
    }
}
