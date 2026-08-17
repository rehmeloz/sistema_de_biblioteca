using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Pages.Usuarios;

public class CreateModel : PageModel
{
    public readonly BibliotecaContext _context;

    public CreateModel(BibliotecaContext context)
    {
        _context = context;
    }

    [BindProperty]
    [Required(ErrorMessage = "Nome é obrigatório!")]
    public string Nome { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email é obrigatório!")]
    public string Email { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var usuario = new Usuario
        {
            Nome = Nome,
            Email = Email
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Usuarios/Index");
    }
}
