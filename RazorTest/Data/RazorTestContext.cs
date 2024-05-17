using Microsoft.EntityFrameworkCore;

namespace RazorTest.Data
{
    public class RazorTestContext : DbContext
    {
        public RazorTestContext (DbContextOptions<RazorTestContext> options)
            : base(options)
        {
        }

        public DbSet<Model.Movie> Movie { get; set; } = default!;
    }
}