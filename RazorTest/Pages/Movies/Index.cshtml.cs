using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorTest.Model;

namespace RazorTest.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly Data.RazorTestContext _context;

        public IndexModel(Data.RazorTestContext context)
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
