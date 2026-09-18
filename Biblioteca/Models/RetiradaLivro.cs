namespace Biblioteca.Models;

public class RetiradaLivro
{
    public int Id { get; set; }
    public int LivroId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime DataRetirada { get; set; }
    public DateTime? DataDevolucao { get; set; }
    public bool Devolvido { get; set; }
    public Livro Livro { get; set; }
    public Usuario Usuario { get; set; }
}
