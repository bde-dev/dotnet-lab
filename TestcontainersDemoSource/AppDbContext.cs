using Microsoft.EntityFrameworkCore;

namespace TestcontainersDemoSource;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
        
    }
    
    public DbSet<User> users { get; set; }
}