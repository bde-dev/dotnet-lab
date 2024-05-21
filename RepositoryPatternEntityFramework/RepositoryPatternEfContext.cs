using Microsoft.EntityFrameworkCore;
using RepositoryPatternEntityFramework.Models;

namespace RepositoryPatternEntityFramework;

public class RepositoryPatternEfContext : DbContext
{
    public RepositoryPatternEfContext(DbContextOptions<RepositoryPatternEfContext> options)
    : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
}