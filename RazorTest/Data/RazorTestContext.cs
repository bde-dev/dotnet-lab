using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorTest.Model;

namespace RazorTest.Data
{
    public class RazorTestContext : DbContext
    {
        public RazorTestContext (DbContextOptions<RazorTestContext> options)
            : base(options)
        {
        }

        public DbSet<RazorTest.Model.Movie> Movie { get; set; } = default!;
    }
}