using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Pages.Livros;

public class EditModel : PageModel
{
    public readonly BibliotecaContext _context;

    public EditModel(BibliotecaContext context)
    {
        _context = context;
    }

    [BindProperty]
    public int Id { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Título é obrigatório!")]
    public string Titulo { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Autor é obrigatório!")]
    public string Autor { get; set; }

    [BindProperty]
    public bool Disponivel { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var livros = await _context.Livros.FindAsync(id);

        if(livros is null)
        {
            return NotFound();
        }

        Id = livros.Id;
        Titulo = livros.Titulo;
        Autor = livros.Autor;
        Disponivel = livros.Disponivel;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var livro = await _context.Livros.FindAsync(Id);

        if (livro is null)
        {
            return NotFound();
        }

        livro.Titulo = Titulo;
        livro.Autor = Autor;
        livro.Disponivel = Disponivel;

        await _context.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}
