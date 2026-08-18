using Biblioteca.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Pages.Usuarios;

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
    [Required(ErrorMessage = "Nome é obrigatório!")]
    public string Nome { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Email é obrigatório!")]
    public string Email { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        
        if(usuario is null)
        {
            return NotFound();
        }

        Id = usuario.Id;
        Nome = usuario.Nome;
        Email = usuario.Email;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var usuario = await _context.Usuarios.FindAsync(Id);

        if (usuario is null)
        {
            return NotFound();
        }

        usuario.Nome = Nome;
        usuario.Email = Email;

        await _context.SaveChangesAsync();

        return RedirectToPage("/Usuarios/Index");
    }
}
