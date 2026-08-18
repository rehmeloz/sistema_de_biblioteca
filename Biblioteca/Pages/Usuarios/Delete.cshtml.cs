using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biblioteca.Pages.Usuarios;

public class DeleteModel : PageModel
{
    public readonly BibliotecaContext _context;

    public DeleteModel(BibliotecaContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Usuario? Usuario { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Usuario = await _context.Usuarios.FindAsync(id);

        if(Usuario is null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        Usuario = await _context.Usuarios.FindAsync(id);

        if(Usuario is not null)
        {
            _context.Usuarios.Remove(Usuario);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("/Usuarios/Index");
    }
}
