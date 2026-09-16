using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

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

    [BindProperty]
    [Required(ErrorMessage = "Usuário é obrigatório!")]
    public int UsuarioId { get; set; }

    public SelectList UsuariosSelectList { get; set; }

    public void OnGet()
    {
        UsuariosSelectList = new SelectList(
            _context.Usuarios.ToList(),
            nameof(Usuario.Id),
            nameof(Usuario.Nome)
        );
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            UsuariosSelectList = new SelectList(
                _context.Usuarios.ToList(),
                nameof(Usuario.Id),
                nameof(Usuario.Nome)
            );
            return Page();
        }

        var livro = new Livro
        {
            Titulo = Titulo,
            Autor = Autor,
            Disponivel = true,
            UsuarioId = UsuarioId
        };

        _context.Livros.Add(livro);
        await _context.SaveChangesAsync();

        return RedirectToPage("/Livros/Index");
    }
}
