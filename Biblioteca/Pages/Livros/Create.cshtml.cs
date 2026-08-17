using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Biblioteca.Models;

namespace Biblioteca.Pages.Livros;

public class CreateModel : PageModel
{
    public readonly BibliotecaContext _context;

    public CreateModel(BibliotecaContext context)
    {
        _context = context;
    }

    [BindProperty]
    [Required(ErrorMessage = "Título é obrigatório!")]
    public string Titulo { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Autor é obrigatório!")]

    public string Autor { get; set; } = string.Empty;
    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var livro = new Livro
        {
            Titulo = Titulo,
            Autor = Autor,
            Disponivel = true
        };

        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Livros/Index");
    }
}
