namespace Biblioteca.Data;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

    public DbSet<Livro> Livros { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Livro>()
            .HasOne(l => l.Usuario)
            .WithMany(u => u.Livros)
            .HasForeignKey(l => l.UsuarioId);
    }
}
