using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Biblioteca.Pages.Livros;

public class DeleteModel : PageModel
{
    public readonly BibliotecaContext _context;

    public DeleteModel(BibliotecaContext context)
    {
        _context = context;
    }

    [BindProperty]
    public Livro? Livro { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Livro = await _context.Livros.FindAsync(id);

        if(Livro is null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var livro = await _context.Livros.FindAsync(id);

        if(livro is not null)
        {
            _context.Livros.Remove(livro);
            await _context.SaveChangesAsync();
        }

        return RedirectToPage("/Livros/Index");
    }
}
