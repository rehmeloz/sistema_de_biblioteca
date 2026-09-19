using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty(SupportsGet = true)]
        public string? FiltroAutor { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FiltroUsuarioId { get; set; }

        public List<RetiradaLivro> RetiradasAtivas { get; set; }
        public List<string> Autores { get; set; }
        public SelectList UsuariosSelectList { get; set; }

        public async Task OnGetAsync()
        {
            Autores = await _context.RetiradaLivros
                .Where(r => !r.Devolvido)
                .Select(r => r.Livro.Autor)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();

            UsuariosSelectList = new SelectList(
                await _context.RetiradaLivros
                    .Where(r => !r.Devolvido)
                    .Select(r => r.Usuario)
                    .Distinct()
                    .OrderBy(u => u.Nome)
                    .ToListAsync(),
                "Id",
                "Nome"
            );

            var query = _context.RetiradaLivros
                .Include(r => r.Livro)
                .Include(r => r.Usuario)
                .Where(r => !r.Devolvido);

            if (!string.IsNullOrEmpty(FiltroAutor))
            {
                query = query.Where(r => r.Livro.Autor == FiltroAutor);
            }

            if (FiltroUsuarioId.HasValue && FiltroUsuarioId > 0)
            {
                query = query.Where(r => r.UsuarioId == FiltroUsuarioId.Value);
            }

            RetiradasAtivas = await query
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

            // Verificar se a retirada existe
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