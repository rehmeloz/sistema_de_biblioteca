using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Pages.Livros;

public class IndexModel : PageModel
{
    public readonly BibliotecaContext _context;

    public IndexModel(BibliotecaContext context)
    {
        _context = context;
    }

    public List<Livro> Livros { get; set; }

    public async Task OnGetAsync(bool? disponivel)
    {
        if(disponivel == true)
        {
            Livros = await _context.Livros.Where(l => l.Disponivel).ToListAsync();
        }
        else
        {
            Livros = await _context.Livros.ToListAsync();
        }    
    }
}
