using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Pages.Usuarios;

public class IndexModel : PageModel
{
    public readonly BibliotecaContext _context;

    public IndexModel(BibliotecaContext context)
    {
        _context = context;
    }

    public List<Usuario> Usuarios { get; set; }

    public async Task OnGetAsync()
    {
        Usuarios = await _context.Usuarios.ToListAsync();
    }
}
