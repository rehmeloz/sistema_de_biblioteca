using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using Biblioteca.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblioteca.Pages.Retiradas
{
    public class CreateModel : PageModel
    {
        private readonly BibliotecaContext _context;

        public CreateModel(BibliotecaContext context)
        {
            _context = context;
        }

        [BindProperty]
        public int SelecionaLivroId { get; set; }

        [BindProperty]
        public int SelecionaUsuarioId { get; set; }

        public List<string> Autores { get; set; }
        public SelectList UsuariosSelectList { get; set; }

        public async Task OnGetAsync()
        {
            Autores = await _context.Livros
                .Where(l => l.Disponivel)
                .Select(l => l.Autor)
                .Distinct()
                .OrderBy(a => a)
                .ToListAsync();

            UsuariosSelectList = new SelectList(
                await _context.Usuarios.OrderBy(u => u.Nome).ToListAsync(),
                "Id",
                "Nome"
            );
        }

        public async Task<IActionResult> OnGetLivrosPorAutorAsync(string autor)
        {
            var livros = await _context.Livros
                .Where(l => l.Autor == autor && l.Disponivel)
                .OrderBy(l => l.Titulo)
                .Select(l => new { l.Id, l.Titulo })
                .ToListAsync();

            return new JsonResult(livros);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SelecionaLivroId == 0 || SelecionaUsuarioId == 0)
            {
                ModelState.AddModelError(string.Empty, "Selecione um livro e um usuário.");
                await OnGetAsync();
                return Page();
            }

            var livro = await _context.Livros.FindAsync(SelecionaLivroId);

            if (livro == null || !livro.Disponivel)
            {
                ModelState.AddModelError(string.Empty, "Este livro não está disponível para retirada.");
                await OnGetAsync();
                return Page();
            }

            var retiradaAtiva = await _context.RetiradaLivros
                .FirstOrDefaultAsync(r => r.LivroId == SelecionaLivroId && !r.Devolvido);

            if (retiradaAtiva != null)
            {
                ModelState.AddModelError(string.Empty, "Este livro já foi retirado e ainda não foi devolvido.");
                await OnGetAsync();
                return Page();
            }

            try
            {
                var retirada = new RetiradaLivro
                {
                    LivroId = SelecionaLivroId,
                    UsuarioId = SelecionaUsuarioId,
                    DataRetirada = DateTime.Now,
                    Devolvido = false
                };

                livro.Disponivel = false;

                _context.RetiradaLivros.Add(retirada);
                _context.Livros.Update(livro);

                await _context.SaveChangesAsync();

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Erro ao registrar retirada: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }
    }
}