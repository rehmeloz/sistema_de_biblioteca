using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Pages.Retiradas
{
    public class IndexModel : PageModel
    {
        private readonly BibliotecaContext _context;

        public IndexModel(BibliotecaContext context)
        {
            _context = context;
        }

        public List<RetiradaLivro> Retiradas { get; set; }

        public async Task OnGetAsync()
        {
            Retiradas = await _context.RetiradaLivros
                .Include(r => r.Livro)
                .Include(r => r.Usuario)
                .OrderByDescending(r => r.DataRetirada)
                .ToListAsync();
        }
    }
}