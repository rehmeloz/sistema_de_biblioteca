namespace Biblioteca.Models;

public class Livro
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Autor { get; set; } = string.Empty;
    public bool Disponivel { get; set; } = true;
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
}
