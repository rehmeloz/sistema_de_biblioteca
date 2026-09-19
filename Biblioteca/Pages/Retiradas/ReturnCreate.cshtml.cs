using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Biblioteca.Data;

namespace Biblioteca.Pages.Retiradas
{
    public class ReturnCreateModel : PageModel
    {
        private readonly BibliotecaContext _context;

        public ReturnCreateModel(BibliotecaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int SelecionaRetiradaId { get; set; }

        public List<RetiradaLivro> RetiradasAtivas { get; set; }

        public async Task OnGetAsync()
        {
            RetiradasAtivas = await _context.RetiradaLivros
                .Include(r => r.Livro)
                .Include(r => r.Usuario)
                .Where(r => !r.Devolvido)
                .OrderByDescending(r => r.DataRetirada)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SelecionaRetiradaId == 0)
            {
                ModelState.AddModelError(string.Empty, "Selecione uma retirada para devolver.");
                await OnGetAsync();
                return Page();
            }

            var retirada = await _context.RetiradaLivros
                .Include(r => r.Livro)
                .FirstOrDefaultAsync(r => r.Id == SelecionaRetiradaId);

            if (retirada == null)
            {
                ModelState.AddModelError(string.Empty, "Retirada não encontrada.");
                await OnGetAsync();
                return Page();
            }

            if (retirada.Devolvido)
            {
                ModelState.AddModelError(string.Empty, "Este livro já foi devolvido.");
                await OnGetAsync();
                return Page();
            }

            try
            {
                retirada.Devolvido = true;
                retirada.DataDevolucao = DateTime.Now;

                var livro = retirada.Livro;
                livro.Disponivel = true;

                _context.RetiradaLivros.Update(retirada);
                _context.Livros.Update(livro);

                await _context.SaveChangesAsync();

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao registrar devolução: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }
    }
}