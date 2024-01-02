using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorTest.Data;
using RazorTest.Model;

namespace RazorTest.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorTest.Data.RazorTestContext _context;

        public IndexModel(RazorTest.Data.RazorTestContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Movie != null)
            {
                Movie = await _context.Movie.ToListAsync();
            }
        }
    }
}
